using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using System.Net;
using System;
using Newtonsoft.Json.Linq;
using _311NYC.Model;
using _311NYC.Controls;
using _311NYC.ViewModel;
using GalaSoft.MvvmLight.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.ComponentModel.Composition;
using System.Windows.Navigation;
using _311NYC.Helpers;
using GalaSoft.MvvmLight.Command;
using _311NYC.DataLoader;
using RedBit.WindowsPhone.ExternalApi.Twitter;

namespace _311NYC.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    [Export(typeof(DetailsViewModel))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class DetailsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the DetailsViewModel class.
        /// </summary>
        [ImportingConstructor]
        public DetailsViewModel(NavigationHelper navService, DetailsAddCommentViewModel dacvm, DetailsAddTagViewModel datvm)
        {
            //Messenger.Default.Register<DecoratedPushPin>(this,m => LoadFromPin(m));
            Messenger.Default.Register<PoiPushpin>(this, MessageTokens.PoiPushpinDetailsClicked, PoiPushpinDetailsClicked);
            Messenger.Default.Register<bool>(this, MessageTokens.TagAdded, HandleRefreshRequst);
            Messenger.Default.Register<bool>(this, MessageTokens.CommentAdded, HandleRefreshRequst);
            Messenger.Default.Register<bool>(this, MessageTokens.RatingAdded, HandleRefreshRequst);
            NavigationHelper = navService;

            //Create the relay commands
            /*AddTagToPoi = new RelayCommand<PoiPushpin>((pin) =>
                {
                    if (TwitterHelper.HasAuthenticated)
                    {
                        NavigationHelper.NavigateToAddTagPage();
                        Messenger.Default.Send<PoiPushpin>(pin, MessageTokens.PoiPushpinDetailsAddTagClicked);
                    }
                    else
                    {
                        MessageBox.Show("You must login via twitter before you are able to add a tag.", "Warning", MessageBoxButton.OK);
                    }
                });

            AddCommentToPoi = new RelayCommand<PoiPushpin>((pin) =>
                {
                    if (TwitterHelper.HasAuthenticated)
                    {
                        NavigationHelper.NavigateToAddCommentPage();
                        Messenger.Default.Send<PoiPushpin>(pin, MessageTokens.PoiPushpinDetailsAddCommentClicked);
                    }
                    else
                    {
                        MessageBox.Show("You have to login via twitter to add a comment.", "Warning", MessageBoxButton.OK);
                    }

                });

            AddRatingToPoi = new RelayCommand<int>((rating) =>
                {
                    if (TwitterHelper.HasAuthenticated)
                    {
                        if (MessageBox.Show(string.Format("Are you sure you want to rate the placemark {0} out of 5 Stars?", rating), "Confirm?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            DataUploaderProxy.AddRating(Pin.VanGuideId, Pin.Id, rating);
                    }
                    else
                    {
                        MessageBox.Show("You have to login via twitter to add a rating.", "Warning", MessageBoxButton.OK);
                    }
                });*/
        }

        internal NavigationHelper NavigationHelper { get; set; }


        private void HandleRefreshRequst(bool successful)
        {
            DispatcherHelper.UIDispatcher.BeginInvoke(() => 
            {
                Pin.Comments.Clear();
                Pin.Tags.Clear();
                this.Pin.CommentsLoaded = false;
                this.PoiPushpinDetailsClicked(this.Pin); 
            });
        }

        /// <summary>
        /// the current pin the details view is on
        /// </summary>
        public PoiPushpin Pin
        {
            get
            {
                return m_pin;
            }
            set
            {
                m_pin = value;
                RaisePropertyChanged("Pin");
            }
        }
        private PoiPushpin m_pin = null;

        public RelayCommand<PoiPushpin> AddTagToPoi { get; set; }

        public RelayCommand<PoiPushpin> AddCommentToPoi { get; set; }

        public RelayCommand<int> AddRatingToPoi { get; set; }


        /// <summary>
        /// Bind to the UI to show the loading visibility
        /// </summary>
        public Visibility LoadingVisibility
        {
            get { return this.Loading ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// The <see cref="Loading" /> property's name.
        /// </summary>
        private bool _loading = true;
        /// <summary>
        /// Gets the Loading property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public bool Loading
        {
            get
            {
                return _loading;
            }

            set
            {
                if (_loading == value)
                    return;

                _loading = value;

                // Update bindings, no broadcast
                RaisePropertyChanged("Loading");
                RaisePropertyChanged("LoadingVisibility");
            }
        }

        private string m_loadingText = "Loading, please wait...";
        public string LoadingText
        {
            get { return m_loadingText; }
            set
            {
                m_loadingText = value;
                RaisePropertyChanged("LoadingText");
            }
        }

        /// <summary>
        /// Called from main view model when details are wanted on pushpin
        /// </summary>
        /// <param name="pin"></param>
        private void PoiPushpinDetailsClicked(PoiPushpin pin)
        {
            Loading = true;
            Pin = pin;
            
            //set the loading text
            this.LoadingText = "Loading Ratings...";

            if (!Pin.CommentsLoaded || !Pin.TagsLoaded)
            {
                //make the web request
                WebClient wc = new WebClient();
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(SummaryDownloadComplete);
                wc.DownloadStringAsync(new Uri("http://vanguide.cloudapp.net/Summaries/ShowByGuid.json?guid=" + pin.Id + "&nocache=" + Guid.NewGuid().ToString()));
            }
            else
            {
                this.Loading = false;
            }
        }

        void SummaryDownloadComplete(object sender, DownloadStringCompletedEventArgs e)
        {
            
            if (e.Error != null)
            {
                //Just navigate back on an error
                //DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                //{
                //    string msg = "There was a problem downloading the summary for {0}.\r\n\r\nError Message:{1}";
                //    MessageBox.Show(string.Format(msg, Pin.Category, e.Error.Message));
                //    NavigationHelper.NavigateBack();
                //});
                SetCommentsTagsToDefaultMessage();
                this.Loading = false;
                return;
            }

            
            JObject o = JObject.Parse(e.Result);
            Pin.VanGuideId = o.SelectToken("Id").ToString();
            Pin.Description = o.SelectToken("Name").ToString();

            string tagCol = o.SelectToken("Tag").ToString().Replace("\"","");

            Pin.RatingCount = Int32.Parse(o.SelectToken("RatingCount").ToString());
            Pin.RatingTotal = Int32.Parse(o.SelectToken("RatingTotal").ToString());

            if (string.IsNullOrEmpty(tagCol))
                Pin.Tags.Add("This Landmark has not been tagged");
            else
            {
                foreach (string tag in tagCol.Split(','))
                {
                    Pin.Tags.Add(tag);
                }
            }

            //mark the pin as loaded
            Pin.TagsLoaded = true;

            //update the ui loading text
            DispatcherHelper.UIDispatcher.BeginInvoke(() => LoadingText = "Loading comments ...");

            WebClient commentWC = new WebClient();
            commentWC.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadCommentsCompleted);
            commentWC.DownloadStringAsync(new Uri("http://vanguide.cloudapp.net/Comments/List.json/" + Pin.VanGuideId + "?nocache=" + Guid.NewGuid().ToString()));
        }

        private void SetCommentsTagsToDefaultMessage()
        {
            Pin.Comments.Clear();
            Pin.Tags.Clear();
            Pin.Comments.Add(new Comment() { Text = "No Comments Found" });
            Pin.Tags.Add("This Landmark has not been tagged");
        }
        void DownloadCommentsCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //Just navigate back on an error
                //DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                //{
                //    string msg = "There was a problem downloading the comments for {0}.\r\n\r\nError Message:{1}";
                //    MessageBox.Show(string.Format(msg, Pin.Category, e.Error.Message));
                //    NavigationHelper.NavigateBack();
                //});
                SetCommentsTagsToDefaultMessage();
                this.Loading = false;
                return;
            }

            JArray cmts = JArray.Parse(e.Result);

            foreach (var obj in cmts.Children())
            {
                JObject o = JObject.Parse(obj.ToString());
                string text = (string)o.SelectToken("Comment.Text");
                string auth = (string)o.SelectToken("CommentAuthor");
                Pin.Comments.Add(new Comment() { Text = text, Author = auth });
            }

            if (Pin.Comments.Count == 0)
                Pin.Comments.Add(new Comment() { Text = "No Comments Found" });

            //mark the pin as comments loaded
            Pin.CommentsLoaded = true;

            //Update the UI that we are done
            this.Loading = false;
        }

    }

    [Export(typeof(DetailsAddTagViewModel))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class DetailsAddTagViewModel : ViewModelBase
    {
        [ImportingConstructor]
        public DetailsAddTagViewModel(NavigationHelper navService)
        {
            Messenger.Default.Register<PoiPushpin>(this,MessageTokens.PoiPushpinDetailsAddTagClicked, (pin) => LoadFromPin(pin));
            NavigationHelper = navService;

            //Create the relay commands
            this.AddNewTag = new RelayCommand<string>(AddNewTagExecute);
        }

        /// <summary>
        /// Command to execute when a tag is to be added
        /// </summary>
        public RelayCommand<string> AddNewTag { get; private set; }
        private void AddNewTagExecute(string value)
        {
            if (value == null || value.Length == 0)
            {
                MessageBox.Show("You have not entered a tag value to save! Please type one in before continuing", "Warning", MessageBoxButton.OK);
                return;
            }

            //Try and upload the tag
            try
            {
                DataUploaderProxy.AddTag(Pin.VanGuideId, Pin.Id, value);
            }
            catch (Exception ex)
            {
                string msg = "Unable to add tag!\r\n\r\nError : {0}";
                MessageBox.Show(string.Format(msg, ex.Message));
            }
        }


        /// <summary>
        /// called when a pin is selected to add a tag to
        /// </summary>
        /// <param name="pin"></param>
        private void LoadFromPin(PoiPushpin pin)
        {
            Pin = pin;
        }

        /// <summary>
        /// The pushpin selected
        /// </summary>
        public PoiPushpin Pin { get; set; }

        /// <summary>
        /// The application title to display in the UI
        /// </summary>
        public string ApplicationTitle
        {
            get
            {
                return Pin.Category;
            }
        }

        /// <summary>
        /// The page title to display in the ui
        /// </summary>
        public string PageTitle
        {
            get
            {
                return "add new tag";
            }
        }

        /// <summary>
        /// Navigation service
        /// </summary>
        internal NavigationHelper NavigationHelper { get; set; }
    }

    [Export(typeof(DetailsAddCommentViewModel))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class DetailsAddCommentViewModel : ViewModelBase
    {
        [ImportingConstructor]
        public DetailsAddCommentViewModel(NavigationHelper navService)
        {
            Messenger.Default.Register<PoiPushpin>(this, MessageTokens.PoiPushpinDetailsAddCommentClicked, (pin) => LoadFromPin(pin));
            NavigationHelper = navService;

            //Create the relay commands
            this.AddNewComment = new RelayCommand<string>(AddNewCommentExecute);
        }

        /// <summary>
        /// Command to execute when a tag is to be added
        /// </summary>
        public RelayCommand<string> AddNewComment { get; private set; }
        private void AddNewCommentExecute(string value)
        {
            if (value == null || value.Length == 0)
            {
                MessageBox.Show("You have not entered a comment to save! Please type one in before continuing!", "Warning", MessageBoxButton.OK);
                return;
            }

            //Try and upload the tag
            try
            {
                DataUploaderProxy.AddComment(Pin.VanGuideId, Pin.Id, value);
            }
            catch (Exception ex)
            {
                string msg = "Unable to add comment!\r\n\r\nError : {0}";
                MessageBox.Show(string.Format(msg, ex.Message));
            }
        }

        /// <summary>
        /// called when a pin is selected to add a tag to
        /// </summary>
        /// <param name="pin"></param>
        private void LoadFromPin(PoiPushpin pin)
        {
            Pin = pin;
        }

        /// <summary>
        /// The pushpin selected
        /// </summary>
        public PoiPushpin Pin { get; set; }

        /// <summary>
        /// The application title to display in the UI
        /// </summary>
        public string ApplicationTitle
        {
            get
            {
                return Pin.Category;
            }
        }

        /// <summary>
        /// The page title to display in the ui
        /// </summary>
        public string PageTitle
        {
            get
            {
                return "new comment";
            }
        }

        /// <summary>
        /// Navigation service
        /// </summary>
        internal NavigationHelper NavigationHelper { get; set; }
    }
}