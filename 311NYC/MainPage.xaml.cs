using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using System.ServiceModel.Syndication;
using Microsoft.Phone.Tasks;
using Microsoft.WindowsAzure.Samples.Phone.Identity.AccessControl;
using Microsoft.Phone.Net.NetworkInformation;


namespace _311NYC
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            
            //Data Service Requirement
            Loaded += (s, e) =>
            {
                if (!NetworkInterface.GetIsNetworkAvailable() || NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
                {
                    MessageBox.Show("This application requires a network connection to function properly. Please fix your internet connection and re-launch the app.", "Network Error", MessageBoxButton.OK);
                }
                else
                {

                }
            };
        }
        
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Weather

            //Loading today's & twitter feed itmes
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
            //Needs to be updated as 311NYC website changed their feed into rss recently after the app was created. 
            //wc.OpenReadAsync(new Uri("http://www.nyc.gov/apps/311/311Today.txt"));
            wc.OpenReadAsync(new Uri("https://api.twitter.com/1/statuses/user_timeline.rss?screen_name=311NYC"));

            // Load data for the ComplaintData ViewModel Items 
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

        }

        void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            SyndicationFeed feed;
            using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(e.Result))
            {
                feed = SyndicationFeed.Load(reader);
                //TodaysFeedList.ItemsSource = feed.Items;
                TwitterFeedList.ItemsSource = feed.Items;
            }
        }

        // Handle selection changed on ListBox
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected index is -1 (no selection) do nothing
            if (MainListBox.SelectedIndex == -1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + MainListBox.SelectedIndex, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1; 
            /*if (MainListBox.SelectedIndex == -1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/WherePage.xaml?selectedItem=" + MainListBox.SelectedIndex, UriKind.Relative));

            if (MainListBox.SelectedIndex >= 1)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + MainListBox.SelectedIndex, UriKind.Relative));

            if (MainListBox.SelectedIndex == 5)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/WherePage.xaml?selectedItem=" + MainListBox.SelectedIndex, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
                        */

        }


        // Featured Items Control
        private void StreetClosuresTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://gis.nyc.gov/streetclosure/", UriKind.Absolute);
            webBrowserTask.Show();
        }

        private void OEMTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.nyc.gov/oem/", UriKind.Absolute);
            webBrowserTask.Show(); 
        }

        private void OSFNSTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.opt-osfns.org/osfns/", UriKind.Absolute);
            webBrowserTask.Show(); 
        }

        private void CareersTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.nyc.gov/careers/", UriKind.Absolute);
            webBrowserTask.Show(); 
        }

        private void DOTTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.nyc.gov/dot/", UriKind.Absolute);
            webBrowserTask.Show(); 
        }

        private void HopeTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("https://a071-hope.nyc.gov/hope/", UriKind.Absolute);
            webBrowserTask.Show(); 
        }

        private void TeenTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.nyc.gov/html/doh/teen/html/home/home.shtml", UriKind.Absolute);
            webBrowserTask.Show(); 
        }

        private void FreeTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri("http://www.nycgo.com/free/", UriKind.Absolute);
            webBrowserTask.Show(); 
        }

        // Locate Page
        private void Locate_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("//View/LocatePage.xaml", UriKind.Relative));
        }

        private void Stats_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("//StatisticsPage.xaml", UriKind.Relative));
        }

        private void NearbyEvents_Click(object sender, EventArgs e)
        {
            BingMapsTask bingMapsTask = new BingMapsTask();

            //Omit the Center property to use the user's current location.
            //bingMapsTask.Center = new GeoCoordinate(40.7547, -073.9091);

            bingMapsTask.SearchTerm = "New York City";
            bingMapsTask.ZoomLevel = 2;
            bingMapsTask.Show();
        }


        private void TextButton_Click(object sender, RoutedEventArgs e)
        {
            SmsComposeTask composeSMS = new SmsComposeTask();
            composeSMS.Body = "This SMS is sent using 311 NYC Windows Phone 7 App.";
            composeSMS.To = "311692";
            composeSMS.Show();
        }

    }
}