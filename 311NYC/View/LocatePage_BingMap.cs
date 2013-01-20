using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System.Windows.Threading;
using RedBit.WindowsPhone;
using System.Windows.Data;
using Microsoft.Phone.Controls.Maps.Core;

namespace _311NYC
{
    public partial class LocatePage : PhoneApplicationPage
    {
        private Map mapMain;
        private MapLayer mapLayer;
        private Pushpin deviceLocation;
        private DispatcherTimer m_isDownloadingTimer;

        private static string BING_MAP_XAML = @"<m:Map xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                                                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                                                                    xmlns:m=""clr-namespace:Microsoft.Phone.Controls.Maps;assembly=Microsoft.Phone.Controls.Maps"" 
                                                                    x:Name=""mapMain"" 
            		ZoomLevel=""10"" 
            		ZoomBarVisibility=""Collapsed"" CacheMode=""BitmapCache"">
                <m:MapLayer x:Name=""mapLayer"" />
                <m:Pushpin x:Name=""deviceLocation"" Background=""Black"" Visibility=""{Binding CurrentDevicePositionVisible}"" Location=""{Binding CurrentDevicePosition}""/>
            </m:Map>";

        private void CreateBingMapControl()
        {

            //// HACK need to create this control in code after onload because of a bug with the bing map control
            if (mapMain == null)
            {
                mapMain = System.Windows.Markup.XamlReader.Load(BING_MAP_XAML) as Map;

                if (mapLayer == null)
                {
                    mapLayer = mapMain.FindVisualChild("mapLayer") as MapLayer;
                    deviceLocation = mapMain.FindVisualChild("deviceLocation") as Pushpin;
                    deviceLocation.SetBinding(UIElement.VisibilityProperty, new Binding("CurrentDevicePositionVisible"));
                    deviceLocation.SetBinding(Pushpin.LocationDependencyProperty, new Binding("CurrentDevicePosition"));
                }
                else
                {
                    MapLayer temp = mapMain.FindVisualChild("mapLayer") as MapLayer;
                    mapMain.Children.Remove(temp);
                    mapMain.Children.Add(mapLayer);
                    Pushpin p = mapMain.FindVisualChild("deviceLocation") as Pushpin;
                    mapMain.Children.Remove(p);
                    mapMain.Children.Add(deviceLocation);
                }

                //start the is downloading timer
                //HACK need to shwo a progress bar when tiles are downloading as requested from MSFT
                mapMain.ViewChangeStart += ViewChangeStart_IsDownloadingProgressHandler;
                if (m_isDownloadingTimer == null)
                {
                    m_isDownloadingTimer = new DispatcherTimer();
                    m_isDownloadingTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
                    m_isDownloadingTimer.Tick += (s1, e1) =>
                    {
                        m_isDownloadingTimer.Stop();
                        if (mapMain == null)
                        {
                            //pb_mapDownloading.IsIndeterminate = false;
                            //pb_mapDownloading.Visibility = System.Windows.Visibility.Collapsed;
                            m_isDownloadingTimer.Stop();
                        }
                        else
                        {
                            if (mapMain.Mode.IsDownloading)
                            {
                                //pb_mapDownloading.IsIndeterminate = true;
                                //pb_mapDownloading.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                            {
                                //pb_mapDownloading.IsIndeterminate = false;
                                //pb_mapDownloading.Visibility = System.Windows.Visibility.Collapsed;
                                m_isDownloadingTimer.Stop();
                                return;
                            }
                        }

                        //restart the timer but change the interval to every 2 secs
                        m_isDownloadingTimer.Interval = new TimeSpan(0, 0, 2);
                        m_isDownloadingTimer.Start();
                    };
                }

                //add to the content grid
                ContentGrid.Children.Insert(0, mapMain);

                SetupMap();
            }
        }

        private void ViewChangeStart_IsDownloadingProgressHandler(object sender, EventArgs e)
        {
            if (!m_isDownloadingTimer.IsEnabled)
            {
                m_isDownloadingTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
                m_isDownloadingTimer.Start();
            }
        }

        private void UnloadMap()
        {
            //if (mapMain != null && ContentGrid.Children.Contains(mapMain))
            //{
            //    mapMain.ViewChangeStart -= ViewChangeStart_IsDownloadingProgressHandler;
            //    this.mapMain.Mode = new MercatorMode();
            //    this.mapMain.Children.Remove(mapLayer);
            //    this.mapMain.Children.Remove(deviceLocation);
            //    ContentGrid.Children.Remove(mapMain);
            //    mapMain = null;
            //}
        }
    }
}
