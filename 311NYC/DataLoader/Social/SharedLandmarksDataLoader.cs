using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using _311NYC.DataLoader;
using _311NYC.Model;
using _311NYC.Helpers;
using System.IO;
using System.IO.IsolatedStorage;

namespace _311NYC.DataLoader.Social
{
    [Export(typeof(SharedLandmarksDataLoader))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class SharedLandmarksDataLoader : IPointGroupDataLoader
    {

        public void Initialize(SharedLandmarkGroupLoadedCallback callback)
        {
            List<SharedLandmarkGroup> items = new List<SharedLandmarkGroup>()
            {
                new SharedLandmarkGroup()
                {
                    Selected = IsolatedStorageSettings.ApplicationSettings.Contains("SocialLoaded"),
                    Type = PointGroupType.Social,
                    Title = "Shared Landmarks",
                    IconUri = "http://vanguide.cloudapp.net/Images/CommunityPoint.png",
                    DataLoader = this,
                    Id = Guid.NewGuid().ToString()
                },
            };


            if (callback != null)
                callback(new EntityResultsArgs<SharedLandmarkGroup>(items));


        }

        public void LoadPointGroup(PointGroup pg)
        {
            if (pg.Selected)
            {
                WebClient wc = new WebClient();
                wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(SharedLandmarksDownloadComplete);
                wc.DownloadStringAsync(new Uri("http://vanguide.cloudapp.net/Summaries/ShowForCommunityByActivity.json?page=0&page_size=100&nocache=" + Guid.NewGuid().ToString()), pg);
            }
            else
            {
                //unload all the children frm the map layer
                pg.RemovePinsFromLayer();
            }
        }

        void SharedLandmarksDownloadComplete(object sender, DownloadStringCompletedEventArgs e)
        {
            JArray cmts = JArray.Parse(e.Result);

            PointGroup dg = e.UserState as PointGroup;

            if (dg != null)
            {

                List<PointOfInterest> p = new List<PointOfInterest>();

                foreach (var obj in cmts.Children())
                {
                    JObject o = JObject.Parse(obj.ToString());
                    PointOfInterest poi = new PointOfInterest();
                    poi.Id = (string)o.SelectToken("Guid");
                    poi.IconUri = new Uri("http://vanguide.cloudapp.net/Images/CommunityPoint.png");
                    poi.Selectable = true;
                    poi.Description = (string)o.SelectToken("Description");
                    string lat = o.SelectToken("Latitude").ToString();
                    string lng = o.SelectToken("Longitude").ToString();
                    poi.Coordinates = string.Format("{0}, {1}", lng, lat);


                    p.Add(poi);
                }

                dg.Points = p;

                dg.AddPinsToLayer();

                dg.Loaded = true;


            }
        }
    }
}
