using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Device.Location;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls.Maps;
using _311NYC.Model;
using Microsoft.Phone.Controls.Maps.Platform;

namespace _311NYC.Helpers
{
    [Export(typeof(LocationHelper))]
    public class LocationHelper
    {
        private GeoCoordinateWatcher m_watcher;
        private GeoCoordinate m_currentPosition;

        public LocationHelper()
        {
        }

        private GeoCoordinateWatcher Watcher
        {
            get
            {
                if (m_watcher == null)
                {
                    // TODO Figure Out if we need to wait for the GPS and if so change this to high
                    m_watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                    m_watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
                    m_watcher.MovementThreshold = 10; //distance in meters that must be travelled for event to be raised
                }
                return m_watcher;
            }
        }

        public void Start()
        {
            Watcher.Start();
        }

        public void Stop()
        {
            Watcher.Stop();
        }

        /// <summary>
        /// The current position of the device
        /// </summary>
        public GeoCoordinate CurrentPosition
        {
            get
            {
                return m_currentPosition;
            }
            set
            {
                if (value != null)
                {
                    m_currentPosition = value;
                    Messenger.Default.Send<Location>(GeoCoordinateToLocation(m_currentPosition), MessageTokens.CurrentPositionUpdated);
                }
            }
        }

        private Location GeoCoordinateToLocation(GeoCoordinate geoCoord)
        {
            if (geoCoord.IsUnknown)
                return null;
            else
                return new Location() { Latitude = geoCoord.Latitude, Longitude = geoCoord.Longitude};
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (!e.Position.Location.IsUnknown)
            {
                CurrentPosition = e.Position.Location;
                Watcher.Stop();
            }
        }

    }
}
