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
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;

namespace _311NYC.Model
{
    public class PointOfInterest
    {
        public string Id { get; set; }
        public bool Selectable { get; set; }
        public string Description { get; set; }
        public Uri IconUri { get; set; }
        public string Coordinates { get; set; }


        public Location Location 
        {
            get
            {
                string[] vals = Coordinates.Split(',');
                return new Location() { Latitude = double.Parse(vals[1]), Longitude = double.Parse(vals[0]) };
            }
        }
    }
}
