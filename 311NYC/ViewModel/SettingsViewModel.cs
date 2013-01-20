using GalaSoft.MvvmLight;
using System.Collections.Generic;
using _311NYC.Model;
using _311NYC.DataLoader;
using System.Linq;
using System.ComponentModel.Composition;
using _311NYC.Helpers;
using System.Windows;
using _311NYC.DataLoader.VanGuide;
using GalaSoft.MvvmLight.Threading;
using _311NYC.DataLoader.Social;
using GalaSoft.MvvmLight.Command;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls.Maps;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls.Maps.Core;
using System.Windows.Controls;

namespace _311NYC.ViewModel
{
    [Export(typeof(SettingsViewModel))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class SettingsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the SettingsViewModel class.
        /// </summary>
        [ImportingConstructor]
        public SettingsViewModel(NavigationHelper navHelper, VanGuideDataLoader vanGuideDataLoader, SharedLandmarksDataLoader sharedLandmarksDataLoader, VanGuideRegions regionsDataLoader)
        {
            this.NavigationHelper = navHelper;

            this.VanGuideDataLoader = vanGuideDataLoader;
            this.VanGuideRegions = regionsDataLoader;
            //this.SharedLandmarksDataLoader = sharedLandmarksDataLoader;

            //create the relay commands
            MapStyleChanged = new RelayCommand<string>(MapStyleChangedExecute);
            PointGroupFilterChanged = new RelayCommand<SelectionChangedEventArgs>(PointGroupFilterChangedExecute);

            Messenger.Default.Register<bool>(this, MessageTokens.SocialPointAdded, (b) => 
            {
                DispatcherHelper.UIDispatcher.BeginInvoke(() => { 
                    //this.LoadSocialLandmarks();
                    //a new point group was selected
                    PointGroupFilterChangedExecute(new SelectionChangedEventArgs(new List<PointGroup>(), new List<PointGroup>() /*{ this.Social.First() as PointGroup }*/));
                }); 
            });
        }

        public NavigationHelper NavigationHelper { get; private set; }

        public RelayCommand<string> MapStyleChanged { get; set; }

        public RelayCommand<SelectionChangedEventArgs> PointGroupFilterChanged { get; set; }
        
        #region Title
        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string title = "311 NYC";

        /// <summary>
        /// Gets the Title property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (title == value)
                {
                    return;
                }

                var oldValue = title;
                title = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        #endregion Title

        #region Landmarks

        private IEnumerable<PointGroup> landmarks = null;
        /// <summary>
        /// Gets the Groups property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public IEnumerable<PointGroup> Landmarks
        {
            get
            {
                if (landmarks == null)
                {
                    LoadLandmarks();
                }
                    //Landmarks = DataLoaderProxy.GetAllGroups(false).Where(g => g.Type == PointGroupType.Landmarks).OrderBy(g => g.GetType().ToString());
                return landmarks;
            }

            set
            {
                if (landmarks == value)
                {
                    return;
                }

                var oldValue = landmarks;
                landmarks = value;

                // Update bindings, no broadcast
                RaisePropertyChanged("Landmarks");
            }
        }

        public IEnumerable<PointGroup> SelectedLandmarks
        {
            get
            {
                if (landmarks != null)
                    return Landmarks.Where(p => p.Selected);
                else
                    return new List<PointGroup>(0);
            }
        }

        /// <summary>
        /// Loads the landmarks from the server.  This does not load the actual point items.
        /// </summary>
        private void LoadLandmarks()
        {
            this.LoadingText = "Loading Places ...";
            this.Loading = true;

            VanGuideDataLoader.Initialize((args) =>
            {
                if (args.Error != null)
                {
                    //there was an error so attempt to navigate back
                    DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                    {
                        string msg = "Unable to load places from server.\r\n\r\nError: {0}";
                        MessageBox.Show(string.Format(msg, args.Error.Message), "Error", MessageBoxButton.OK);
                    });
                }
                else
                {
                    this.Landmarks = args.Results;
                    LoadRegions();
                }

                //hide the loading
                this.Loading = false;
            });
        }

        private void LoadRegions()
        {
            this.LoadingText = "Loading Regions ...";
            this.Loading = true;

            VanGuideRegions.Initialize((args) =>
                {
                    if (args.Error != null)
                    {
                        //there was an error so attempt to navigate back
                        DispatcherHelper.UIDispatcher.BeginInvoke(() =>
                        {
                            string msg = "Unable to load regions from server.\r\n\r\nError: {0}";
                            MessageBox.Show(string.Format(msg, args.Error.Message), "Error", MessageBoxButton.OK);
                        });
                    }
                    else
                    {
                        if (this.Landmarks != null)
                        {
                            List<PointGroup> items = this.Landmarks.ToList();
                            foreach (RegionGroup rg in args.Results)
                            {
                                if (!items.Contains(rg))
                                    items.Add(rg as PointGroup);
                            }
                            this.Landmarks = items;
                        }
                    }

                    //hide the loading
                    this.Loading = false;
                });
        }

        #endregion Landmarks
    
        #region Loading
        /// <summary>
        /// Bind to the UI to show the loading visibility
        /// </summary>
        public Visibility LoadingVisibility
        {
            get { return this.Loading ? Visibility.Visible : Visibility.Collapsed; }
        }

        private bool _loading = true;
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

        #region MapStyle Settings
        private string m_mapStyleString;
        public string MapStyleString
        {
            get
            {
                if (m_mapStyleString == null)
                    MapStyleString = LoadMapStyle();
                return m_mapStyleString;
            }
            set
            {
                m_mapStyleString = value;
                RaisePropertyChanged("MapStyleString");
            }
        }

        private void MapStyleChangedExecute(string value)
        {
            if (value == null && value.Length == 0)
            {
                string msg = "Unable to set map style.  Value cannot be null or zero length.";
                MessageBox.Show(msg, "Error", MessageBoxButton.OK);
            }
            else
            {
                switch (value)
                {
                    case "Road":
                        Messenger.Default.Send<MapMode>(new RoadMode(), MessageTokens.MapModeChanged);
                        break;
                    case "Satellite":
                        Messenger.Default.Send<MapMode>(new AerialMode(false), MessageTokens.MapModeChanged);
                        break;
                    case "Hybrid":
                        Messenger.Default.Send<MapMode>(new AerialMode(true), MessageTokens.MapModeChanged);
                        break;
                    default:
                        break;
                }

                //save to storage
                SaveMapStyle(value);

                //update the property
                MapStyleString = value;
            }


        }

        private void SaveMapStyle(string value)
        {
            IsolatedStorageSettings.ApplicationSettings[ApplicationSettingKeys.MapModeKey] = value;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        private string LoadMapStyle()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(ApplicationSettingKeys.MapModeKey))
                return IsolatedStorageSettings.ApplicationSettings[ApplicationSettingKeys.MapModeKey] as string;
            else
                return "Road"; //default to road mode
        } 
        #endregion

        private void PointGroupFilterChangedExecute(SelectionChangedEventArgs args)
        {
            PointGroup pg = null;
            if (args.AddedItems.Count > 0)
            {
                //a new point group was selected
                pg = args.AddedItems[0] as PointGroup;
                pg.Selected = true;
                Messenger.Default.Send<PointGroup>(pg, MessageTokens.MapLayerAdded);
            }
            else if (args.RemovedItems.Count > 0)
            {
                //a point group was removed
                pg = args.RemovedItems[0] as PointGroup;
                pg.Selected = false;
                Messenger.Default.Send<PointGroup>(pg, MessageTokens.MapLayerRemoved);
            }
         
            if (pg != null && pg.DataLoader != null)
                pg.DataLoader.LoadPointGroup(pg);
        }

        private VanGuideDataLoader VanGuideDataLoader { get; set; }

        //private SharedLandmarksDataLoader SharedLandmarksDataLoader { get; set; }

        private VanGuideRegions VanGuideRegions { get; set; }

       

    }
}