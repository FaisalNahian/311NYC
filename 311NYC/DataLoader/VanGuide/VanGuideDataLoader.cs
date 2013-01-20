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
using System.Xml.Linq;
using System.Linq;
using _311NYC.Model;
using System.Threading;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls.Maps;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using _311NYC.Helpers;


namespace _311NYC.DataLoader.VanGuide
{
    [Export(typeof(VanGuideDataLoader))]
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    public class VanGuideDataLoader : IPointGroupDataLoader
    {
        //private static Uri SourceUri = new Uri("http://vanguide.cloudapp.net/PointSources.xml");
        private static Uri SourceUri = new Uri("http://heyfaisal.com/apps/311NYC/FilterNames.xml");
    
        public void Initialize(PointGroupLoadedCallback callback)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(SourceUri, callback);
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            PointGroupLoadedCallback callback = e.UserState as PointGroupLoadedCallback;

            try
            {
                XDocument xDoc = XDocument.Parse(e.Result);
                XNamespace ns = xDoc.Root.Attributes("xmlns").First().Value;

                //parse out all the items
                var elements = from ele in xDoc.Descendants(ns + "entry")
                               from kml in ele.Descendants(ns + "link")
                               where (kml.Attribute("type") != null && kml.Attribute("type").Value == "application/vnd.google-earth.kml+xml" && kml.Attribute("href").Value.StartsWith("http"))
                               from pic in ele.Descendants(ns + "link")
                               where (pic.Attribute("type") != null && pic.Attribute("type").Value == "image/png")
                               select new PointGroup
                               {
                                   Title = ele.Descendants(ns + "title").FirstOrDefault().Value,
                                   DataUrl = kml.Attribute("href").Value,
                                   IconUri = "http://vanguide.cloudapp.net/" + pic.Attribute("href").Value,
                                   Type = PointGroupType.Landmarks,
                                   Loaded = false,
                                   Selected = false,
                                   DataLoader = this,
                                   Id = ele.Descendants(ns + "id").FirstOrDefault().Value
                               };

                if (callback != null)
                    callback(new EntityResultsArgs<PointGroup>(elements));

            }
            catch (WebException we)
            {
                if (callback != null)
                    callback(new EntityResultsArgs<PointGroup>(we));
            }
            catch (Exception ex)
            {
                if (callback != null)
                    callback(new EntityResultsArgs<PointGroup>(ex));
            }
        }

        public  void LoadPointGroup(PointGroup pg)
        {
            if (pg.Selected)
            {
                WebClient client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(dataElement_DownloadStringCompleted);
                client.DownloadStringAsync(new Uri(pg.DataUrl), pg);
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
            PointGroup dg = e.UserState as PointGroup;
            XDocument xDoc = XDocument.Parse(e.Result);

            XNamespace ns = xDoc.Root.Attributes("xmlns").First().Value;
            var ret =
            from pm in xDoc.Descendants(ns + "Placemark")
	            from point in pm.Descendants( ns + "Point")
            select new PointOfInterest
            {
	            Id = pm.Descendants( ns + "description").First().Value,
                Selectable = true,
	            Description = pm.Descendants( ns + "name").First().Value,
	            Coordinates = point.Descendants(ns + "coordinates").First().Value,
                IconUri = new Uri( dg.IconUri )
	
            };


            dg.Points = ret;
            dg.AddPinsToLayer();

            dg.Loaded = true;
        }
    }
}
