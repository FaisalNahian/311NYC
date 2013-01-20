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

using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using System.Threading;
using Microsoft.Phone.Tasks;

namespace _311NYC
{
    public partial class WherePage : PhoneApplicationPage
    {
        GeoCoordinateWatcher watcher;
        bool trackingOn = false;
        Pushpin myPushpin = new Pushpin();

        public WherePage()
        {
            InitializeComponent();

            //Instatiate watcher, setting its accuracy level and movement threshold.
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High); // using high accuracy.
            watcher.MovementThreshold = 10.0f; // meters of change before "PositionChanged" event fires.

            // wire up event handlers
            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);

            // start up the Location Service on app startup. watcher_StatusChanged
            // will fire when start up of LocServ is complete.
            new Thread(startLocServInBackground).Start();
            statusTextBlock.Text = "Starting Location Service...";
        }

        void startLocServInBackground()
        {
            watcher.TryStart(true, TimeSpan.FromSeconds(60));
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    // The Location Service is disabled or unsupported.
                    // Check to see if the user has disabled the Location Service.
                    if (watcher.Permission == GeoPositionPermission.Denied)
                    {
                        // The user has disabled the Location Service on their device.
                        statusTextBlock.Text = "You have disabled Location Service.";
                    }
                    else
                    {
                        statusTextBlock.Text = "Location Service is not functioning.";
                    }
                    break;
                
                case GeoPositionStatus.Initializing:
                    statusTextBlock.Text = "Location Service is retrieving data..";
                    // The Location Service is initializing.
                    break;
                case GeoPositionStatus.NoData:
                    // The Location Service is working, but it cannot get location data.
                    statusTextBlock.Text = "Location data is not available.";
                    break;
                case GeoPositionStatus.Ready:
                    // The Location Service is working and is receiving location data.
                    statusTextBlock.Text = "Location data is available.";
                    break;
            }
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            // update the textblock readouts.
            LatitudeTextBlock.Text = e.Position.Location.Latitude.ToString("0.0000000000");
            LongitudeTextBlock.Text = e.Position.Location.Longitude.ToString("0.0000000000");

            // update the map if the user has asked to be tracked.
            if (trackingOn)
            {
                // center the pushpin and map on the current position
                myPushpin.Location = e.Position.Location;
                myMap.Center = e.Position.Location;

                // if this is the first time that myPushPin is being plotted, add it to the object
                // hierarchy!
                if (myMap.Children.Contains(myPushpin) == false) { myMap.Children.Add(myPushpin); };
            }
        }

        private void currentLocation_Click(object sender, RoutedEventArgs e)
        {
            if (trackingOn)
            {
                currentLocation.Content = "Track Me On Map";
                trackingOn = false;
                myMap.ZoomLevel = 1.0f; // zoom out to see whole world.
            }
            else
            {
                currentLocation.Content = "Stop Tracking";
                trackingOn = true;
                myMap.ZoomLevel = 16.0f; // zoom to street level.
            }
        }

        // Navigation Bar
        private void NextButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("//AddPicturePage.xaml", UriKind.Relative));
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("//MainPage.xaml", UriKind.Relative));
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("//View/SettingsPage.xaml", UriKind.Relative));
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("//AboutPage.xaml", UriKind.Relative));
        }
    }
}