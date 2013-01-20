using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using _311NYC.Helpers;
using _311NYC.Controls;
using System;
using System.Windows;
using _311NYC.DataLoader;
using _311NYC.ViewModel;
using Microsoft.Phone;
using System.Collections.Generic;
using _311NYC.Model;
using System.Threading;
using Microsoft.Phone.Controls.Maps.Core;
using GalaSoft.MvvmLight.Messaging;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;
using System.Linq;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel.Composition;
using Microsoft.Phone.Controls.Maps.Platform;

namespace _311NYC
{
    public partial class LocatePage : PhoneApplicationPage
    {
        //private Pushpin m_sharedLandmarkPushpin = null;
        private const string DefaultPushPinDetailsStateValue = "DefaultPushPinDetailsState";

        public LocatePage()
        {
            InitializeComponent();

            //unload the map when navigated away
            this.ContentGrid.Unloaded += (s, e) => UnloadMap();

            //init composition
            CompositionInitializer.SatisfyImports(this);
            DataContext = this.MainViewModel;

            this.Loaded += new RoutedEventHandler(LocatePage_Loaded);

            //init the dispatch helper
            GalaSoft.MvvmLight.Threading.DispatcherHelper.Initialize();
        }

        void LocatePage_Loaded(object sender, RoutedEventArgs e)
        {

            // HACK binding to the ApplicationBarMenuItem.Text into the MainViewModel does not seem to work get an AG_E_PARSER_BAD_PROPERTY_VALUE error
            //(this.ApplicationBar.MenuItems[0] as ApplicationBarMenuItem).Text = MainViewModel.TwitterMenuItemText;

            if (!PreviouslyLoaded)
            {
                ListenForPropertyChanges();

                //Register for messages

                Messenger.Default.Register<Location>(this, MessageTokens.CurrentPositionUpdated, (location) =>
                    {
                        mapMain.Center = location;
                    });
                Messenger.Default.Register<PointGroup>(this, MessageTokens.MapLayerAdded, (pg) =>
                    {
                        if (typeof(RegionGroup).IsAssignableFrom(pg.GetType()))
                        {
                            //it's a region so insert it at the beginning so it's on the bottom
                            this.mapLayer.Children.Insert(0, pg.MapLayer);
                        }
                        else
                            this.mapLayer.Children.Add(pg.MapLayer);
                    });

                Messenger.Default.Register<PointGroup>(this, MessageTokens.MapLayerRemoved, (pg) =>
                {
                    if (this.mapLayer.Children.Contains(pg.MapLayer))
                        this.mapLayer.Children.Remove(pg.MapLayer);
                });
                Messenger.Default.Register<PoiPushpin>(this, MessageTokens.PoiPushpinClicked, (pin) =>
                    {
                        //show the details grid
                        if (MainViewModel.SelectedPushPin != null)
                        {
                            VisualStateManager.GoToState(this, "ExandedPushPinDetailsState", true);
                            //btnAddLandmark.IsEnabled = false;
                        }
                    });

                //Setup the main map
                CreateBingMapControl();

                PreviouslyLoaded = true;
            }
            else
            {
                //CreateBingMapControl();
            }
        }

        [Import(typeof(MainViewModel))]
        public MainViewModel MainViewModel { get; set; }

        [Import(typeof(DetailsViewModel))]
        public DetailsViewModel DetailsViewModel { get; set; }

        /// <summary>
        /// value to determine if the page has already been loaded
        /// </summary>
        private bool PreviouslyLoaded { get; set; }

        private void SetupMap()
        {
            this.mapMain.CredentialsProvider = new ApplicationIdCredentialsProvider(MainViewModel.BingMapsKey);
            this.mapMain.Center = MainViewModel.InitialCenterLocation;
            this.mapMain.Mode = MainViewModel.CurrentMapMode;
        }

        private void ListenForPropertyChanges()
        {
            
            MainViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler((obj, args) =>
            {
                if (args.PropertyName.Equals("CurrentMapMode"))
                {
                    // HACK binding to the MapControl.Mode into the MainViewModel does not seem to work get an AG_E_PARSER_BAD_PROPERTY_VALUE error
                    //this.mapMain.Mode = MainViewModel.CurrentMapMode;
                }

                else if (args.PropertyName.Equals("SelectedPushPin"))
                {
                    //the selected push pin has changed.  if it's null then it was unselected
                    if (MainViewModel.SelectedPushPin == null)
                    {
                        VisualStateManager.GoToState(this, DefaultPushPinDetailsStateValue, true);
                        //this.btnAddLandmark.IsEnabled = true;
                    }
                }
            });
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (MainViewModel.SelectedPushPin != null)
            {
                //the top details part is showing so animate out
                e.Cancel = true;
                MainViewModel.SelectedPushPin = null;
            }

            base.OnBackKeyPress(e);
        }

        private void LocateClick(object sender, System.EventArgs e)
        {
            MainViewModel.FindLocationClicked.Execute(null);
            HidePushPinDetails();
        }

        private void ZoomOutClick(object sender, System.EventArgs e)
        {
            this.mapMain.ZoomLevel--;
        }

        private void SettingsClick(object sender, EventArgs e)
        {
            MainViewModel.NavigateToSettings.Execute(null);
            HidePushPinDetails();
        }

        private void NavigateToDetailsClicked(object sender, RoutedEventArgs e)
        {
            MainViewModel.NavigateToDetails.Execute(MainViewModel.SelectedPushPin);
        }

        private void HidePushPinDetails()
        {
            MainViewModel.SelectedPushPin = null;
        }

    }
}