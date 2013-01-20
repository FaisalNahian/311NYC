using GalaSoft.MvvmLight;
using System.ComponentModel.Composition;
using _311NYC.Helpers;
using System.Collections.ObjectModel;
using _311NYC.Model;
using _311NYC.Controls;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Core;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System.IO.IsolatedStorage;
using System.Windows;
using _311NYC.DataLoader;
using System.Collections.Generic;
using System.Threading;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps.Platform;
using RedBit.WindowsPhone.ExternalApi.Twitter;
using System.Windows.Media;

namespace _311NYC.ViewModel
{
    [Export(typeof(MainViewModel))]
    public class MainViewModel : ViewModelBase
    {
        //private const string TWITTER_LOGIN_TEXT = "Log in using Twitter";
        //private const string TWITTER_LOGOUT_TEXT = "Log out of Twitter";
        private MapMode m_currentMapMode = new RoadMode();
        private PoiPushpin m_selectedPin = null;
        private ObservableCollection<PointGroup> m_pointGroups;
        private Location m_CurrentDevicePosition;
        private Visibility m_CurrentDevicePositionVisible = Visibility.Collapsed;

        [ImportingConstructor]
        public MainViewModel(NavigationHelper navHelper, LocationHelper lh)
        {
            this.NavigationHelper = navHelper;

            //setup the relay commands
            this.NavigateToSettings = new RelayCommand(() =>NavigationHelper.NavigateToSettingsPage());
            this.NavigateToDetails = new RelayCommand<PoiPushpin>((pin) =>
                {
                    //Notify the details view model page that a pushpin has been clicked
                    Messenger.Default.Send(pin, MessageTokens.PoiPushpinDetailsClicked);
                    NavigationHelper.NavigateToDetailsPage();
                });
            //TwitterLoginClicked = new RelayCommand(TwitterLoginClickedExecute);
            FindLocationClicked = new RelayCommand(FindLocationClickedExecute);
            //AddSharedLandmarkCanceledClicked = new RelayCommand(() => NewSharedLandmarkItem = null);
            //AddSharedLandmarkClicked = new RelayCommand(() => NewSharedLandmarkItem = new SharedLandmark());
            //UploadNewSharedLandmarkClicked = new RelayCommand(UploadNewSharedLandmarkClickedExecute);
            AboutHelpClickedCommand = new RelayCommand(() =>
                {
                    NavigationHelper.NavigateToHelpPage();
                });


            //set the bing maps key
            BingMapsKey = "AjFLV9iriOBIfKdOaYMAHrWjIwdNvVXkQ11DzvzM6dYttwa7ClfDF-G0xbGmF6zj";
            LoadMapMode();

            //Register for messages
            Messenger.Default.Register<MapMode>(this,MessageTokens.MapModeChanged,(mapMode) => this.CurrentMapMode = mapMode);
            //Messenger.Default.Register<bool>(this, MessageTokens.TwitterLoginValueChanged,(twitterLogedIn) => RaisePropertyChanged("TwitterMenuItemText"));
            Messenger.Default.Register<PoiPushpin>(this, MessageTokens.PoiPushpinClicked, PoiPushpinClicked);
            Messenger.Default.Register<PointGroup>(this, MessageTokens.MapLayerAdded, MapLayerAdded);

            //setup the location helper
            LocationHelper = lh;
            Messenger.Default.Register<Location>(this, MessageTokens.CurrentPositionUpdated,LocationFoundCallback);
                
        }

        /// <summary>
        /// Relay command to navigat to settings page
        /// </summary>
        public RelayCommand NavigateToSettings { get; private set; }

        /// <summary>
        /// Relay command to navigat to details page
        /// </summary>
        public RelayCommand<PoiPushpin> NavigateToDetails { get; private set; }

        /// <summary>
        /// COmmand to execute when the user wants to find their location
        /// </summary>
        public RelayCommand FindLocationClicked { get; private set; }

        /// <summary>
        /// Command to execute when the about or help is clicked
        /// </summary>
        public RelayCommand AboutHelpClickedCommand { get; set; }

        /// <summary>
        /// Helper class for navigation to different pages
        /// </summary>
        public NavigationHelper NavigationHelper { get; private set; }

        /// <summary>
        /// Helper object to get device location
        /// </summary>
        public LocationHelper LocationHelper { get; private set; }

        /// <summary>
        /// The application title
        /// </summary>
        public string ApplicationTitle
        {
            get
            {
                return "VanGuide";
            }
        }

        /// <summary>
        /// The point groups that have been selected and loaded by the user
        /// </summary>
        public ObservableCollection<PointGroup> PointGroups
        {
            get
            {
                return m_pointGroups;
            }
            private set
            {
                m_pointGroups = value;
                RaisePropertyChanged("PointGroups");
            }
        }

        /// <summary>
        /// The currently selected pushpin on the map
        /// </summary>
        public PoiPushpin SelectedPushPin
        {
            get
            {
                return m_selectedPin;
            }
            set
            {
                if (m_selectedPin != null)
                    ResetPin(m_selectedPin);
                m_selectedPin = value;
                RaisePropertyChanged("SelectedPushPin");
            }
        }

        /// <summary>
        /// Key for bing maps
        /// </summary>
        public string BingMapsKey { get; private set; }

        /// <summary>
        /// The initial center location for the map
        /// </summary>
        public GeoCoordinate InitialCenterLocation
        {
            get
            {
                //return new GeoCoordinate(49.25263, -123.07759); //Vancouver
                return new GeoCoordinate(40.71445, -73.82996); //NYC
            }
        }

        /// <summary>
        /// The current map mode for the bing map control
        /// </summary>
        public MapMode CurrentMapMode
        {
            get
            {
                return m_currentMapMode;
            }
            set
            {
                m_currentMapMode = value;
                RaisePropertyChanged("CurrentMapMode");
            }
        }

        /// <summary>
        /// Gets the current position for the device
        /// </summary>
        public Location CurrentDevicePosition
        {
            get
            {
                return m_CurrentDevicePosition;
            }
            set
            {
                if (value != null)
                {
                    m_CurrentDevicePosition = value;
                    RaisePropertyChanged("CurrentDevicePosition");
                }
            }
        }

        /// <summary>
        /// Determins whether the device push pin should be visible
        /// </summary>
        public Visibility CurrentDevicePositionVisible
        {
            get
            {
                return m_CurrentDevicePositionVisible;
            }
            set
            {
                m_CurrentDevicePositionVisible = value;
                RaisePropertyChanged("CurrentDevicePositionVisible");
            }
        }

        private SharedLandmark m_NewSharedLandmarkItem;
        /// <summary>
        /// Shared landmark object to be databound
        /// </summary>
        public SharedLandmark NewSharedLandmarkItem
        {
            get { return m_NewSharedLandmarkItem; }
            set
            {
                m_NewSharedLandmarkItem = value;
                RaisePropertyChanged("NewSharedLandmarkItem");
            }
        }

        #region Loading
        /// <summary>
        /// Bind to the UI to show the loading visibility
        /// </summary>
        public Visibility LoadingVisibility
        {
            get { return this.Loading ? Visibility.Visible : Visibility.Collapsed; }
        }

        private bool _loading = false;
        /// <summary>
        /// Determins whether anything is loading or not
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
        /// <summary>
        /// The text to display on the loading screen
        /// </summary>
        public string LoadingText
        {
            get { return m_loadingText; }
            set
            {
                m_loadingText = value;
                RaisePropertyChanged("LoadingText");
            }
        }

        #endregion

        /// <summary>
        /// Command to execute when uploading a new shared landmark
        /// </summary>
        public RelayCommand UploadNewSharedLandmarkClicked { get; set; }
        private void UploadNewSharedLandmarkClickedExecute()
        {
            if (!TwitterHelper.HasAuthenticated)
                MessageBox.Show("You must be logged into Twitter before you can add Locations.  Please Login and then try again.", "Not Logged in", MessageBoxButton.OK);
            else
            {
                this.LoadingText = "Uploading new landmark...";
                this.Loading = true;
                DataUploaderProxy.AddNewSharedLandmark(this.NewSharedLandmarkItem,UploadSharedLandmarkCallback);
                
            }
        }

        private void UploadSharedLandmarkCallback(DataUploadResultsArgs args)
        {
            if (args.Error != null)
            {
                //there was an error so attempt to navigate back
                DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                {
                    string msg = "Unable to upload shared landmark to server.\r\n\r\nError: {0}";
                    MessageBox.Show(string.Format(msg, args.Error.Message), "Error", MessageBoxButton.OK);
                    this.Loading = false;
                });
            }
            else
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("Successfully uploaded new shared landmark!", "Success", MessageBoxButton.OK);
                        this.Loading = false;
                    });
            }
        }

        private void LoadMapMode()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(ApplicationSettingKeys.MapModeKey))
            {
                string mode = IsolatedStorageSettings.ApplicationSettings[ApplicationSettingKeys.MapModeKey] as string;
                this.CurrentMapMode = MapHelper.GetMapModeForString(mode);
            }
        }

        private void FindLocationClickedExecute()
        {

            if (!IsolatedStorageSettings.ApplicationSettings.Contains("useGps"))
            {
                if (MessageBox.Show("You currently have location support turned off.  Click OK to turn on location or cancel to keep it off.", "Question", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    IsolatedStorageSettings.ApplicationSettings["useGps"] = true;
                    IsolatedStorageSettings.ApplicationSettings.Save();
                }
                else
                {
                    return;
                }
            }

            if (Microsoft.Devices.Environment.DeviceType == Microsoft.Devices.DeviceType.Device)
            {
                if (CurrentDevicePositionVisible == Visibility.Collapsed)
                {
                    this.Loading = true;
                    this.LoadingText = "Finding your location ...";
                    this.LocationHelper.Start();
                }
                else
                {
                    CurrentDevicePositionVisible = Visibility.Collapsed;
                }
                //Location gps = new Location(LocationHelper.Current.Latitude, LocationHelper.Current.Longitude);
                //Ellipse ellipse = new Ellipse();
                //ellipse.Opacity = 0.3;
                //ellipse.Height = 48;
                //ellipse.Width = 48;
                //ellipse.Fill = new SolidColorBrush(Colors.Black);

                //MapLayer.SetPosition(ellipse, gps);
                //this.mapMain.Children.Add(ellipse);
                //this.mapMain.Center = gps;
            }
            else
                MessageBox.Show("You are currently on the emulator, location data is not supported.", "Warning", MessageBoxButton.OK);
        
        }

        private void LocationFoundCallback(Location location)
        {
            CurrentDevicePosition = location;
            CurrentDevicePositionVisible = Visibility.Visible;
            this.Loading = false;
            this.LoadingText = "Your location found!";
        }

        private void PoiPushpinClicked(PoiPushpin pin)
        {
            if (pin == this.SelectedPushPin)
                pin = null; //same pin was selected

            if (pin != null)
                //expand the selected pin
                ExpandPin(pin);

            ////reset the previous pin
            //if (this.SelectedPushPin != null)
            //    ResetPin(this.SelectedPushPin);

            //set the selected pin
            this.SelectedPushPin = pin;
        }

        private TransformGroup m_expandPinTransformGroup = null;
        private void ExpandPin(PoiPushpin pin)
        {
            if (m_expandPinTransformGroup == null)
            {
                ScaleTransform st = new ScaleTransform()
                {
                    ScaleX = 2,
                    ScaleY = 2,
                };

                TranslateTransform tt = new TranslateTransform()
                {
                    Y = -42,
                    X = -18,
                };

                m_expandPinTransformGroup = new TransformGroup();
                m_expandPinTransformGroup.Children.Add(st);
                m_expandPinTransformGroup.Children.Add(tt);
            }
            pin.Pushpin.RenderTransform = m_expandPinTransformGroup;
        }

        private void ResetPin(PoiPushpin pin)
        {
            pin.Pushpin.RenderTransform = null;
        }

        private Queue<PointGroup> m_pointGroupsLoading = new Queue<PointGroup>();
        private Thread m_thread;
        private void MapLayerAdded(PointGroup pg)
        {
            if (!pg.Loaded)
            {
                lock (m_pointGroupsLoading)
                    m_pointGroupsLoading.Enqueue(pg);

                if (m_thread != null)
                    if (m_thread.ThreadState == ThreadState.Stopped)
                        m_thread = null;

                if (m_thread == null)
                {
                    m_thread = new Thread(new ThreadStart(() =>
                        {
                            while (m_pointGroupsLoading.Count > 0)
                            {
                                PointGroup pgItem;
                                lock (m_pointGroupsLoading)
                                    pgItem = m_pointGroupsLoading.Dequeue();

                                DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                                    {
                                        //update the loading
                                        this.LoadingText = string.Format("Loading {0} locations...",pgItem.Title);
                                        this.Loading = true;
                                    });

                                while (!pgItem.Loaded)
                                    Thread.Sleep(700);
                            }

                            DispatcherHelper.UIDispatcher.BeginInvoke(() =>this.Loading = false);
                        }));
                    m_thread.IsBackground = true;
                    m_thread.Name = "PGsLoadingThread";
                }

                try
                {
                    //Start the thread
                    m_thread.Start();
                }
                catch { }
            }
        }
    }

    
}