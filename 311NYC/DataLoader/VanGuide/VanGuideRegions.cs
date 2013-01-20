using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using System.Threading;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Messaging;
using _311NYC.Model;
using _311NYC.Helpers;

namespace _311NYC.DataLoader.VanGuide
{
    [Export(typeof(VanGuideRegions))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class VanGuideRegions : IPointGroupDataLoader
    {
        private static Uri SourceUri = new Uri("http://vanguide.cloudapp.net/RegionSources.xml");

        public void Initialize(RegionGroupLoadedCallback callback)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(SourceUri, callback);
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            RegionGroupLoadedCallback callback = e.UserState as RegionGroupLoadedCallback;

            try
            {
                XDocument xDoc = XDocument.Parse(e.Result);
                XNamespace ns = xDoc.Root.Attributes("xmlns").First().Value;

                //parse out all the items
                var elements = from ele in xDoc.Descendants(ns + "entry")
                               from kml in ele.Descendants(ns + "link")
                               where (kml.Attribute("type") != null && kml.Attribute("type").Value == "application/vnd.google-earth.kml+xml" && kml.Attribute("href").Value.StartsWith("feeds"))
                               from pic in ele.Descendants(ns + "link")
                               where (pic.Attribute("type") != null && pic.Attribute("type").Value == "image/png")
                               select new RegionGroup
                               {
                                   Title = ele.Descendants(ns + "title").FirstOrDefault().Value,
                                   DataUrl = kml.Attribute("href").Value,
                                   IconUri = "http://vanguide.cloudapp.net/" + pic.Attribute("href").Value,
                                   Type = PointGroupType.Landmarks,
                                   Selected = false,
                                   DataLoader = this,
                                   Id = ele.Descendants(ns + "id").FirstOrDefault().Value
                               };

                if (callback != null)
                    callback(new EntityResultsArgs<RegionGroup>(elements));

            }
            catch (WebException we)
            {
                if (callback != null)
                    callback(new EntityResultsArgs<RegionGroup>(we));
            }
            catch (Exception ex)
            {
                if (callback != null)
                    callback(new EntityResultsArgs<RegionGroup>(ex));
            }

        }


        public void LoadPointGroup(PointGroup pg)
        {
            if (pg.Selected)
            {
                WebClient client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(dataElement_DownloadStringCompleted);
                client.DownloadStringAsync(new Uri("http://vanguide.cloudapp.net/" + pg.DataUrl), pg);
            }
             else
            {
                //unload all the children frm the map layer
                pg.RemovePinsFromLayer();
            }
        }


        void dataElement_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message + Environment.NewLine + e.Error.StackTrace,
                    "Error", MessageBoxButton.OK);
                return;
            }
            RegionGroup dg = e.UserState as RegionGroup;
            XDocument xDoc = XDocument.Parse(e.Result);

            XNamespace ns = xDoc.Root.Attributes("xmlns").First().Value;
            var ret =
            from pm in xDoc.Descendants(ns + "Placemark")
            select new PointOfInterest
            {
                Id = pm.Descendants(ns + "description").First().Value,
                Selectable = true,
                Description = pm.Descendants(ns + "description").First().Value,
                Coordinates = pm.Descendants(ns + "coordinates").First().Value,
                IconUri = new Uri(dg.IconUri)

            };


            dg.Points = ret;
            dg.AddPinsToLayer();

            dg.Loaded = true;

        }
    }
}
