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
    public class RegionGroup : PointGroup
    {
        public RegionGroup() : base() { }

        private static Color[] _Colors = { Colors.Purple, Colors.Red, Colors.Brown, Colors.Cyan, Colors.Green, Colors.Magenta, Colors.Orange, Colors.Yellow, Colors.Blue };
        private static int colorIndex = 0;

        public override void AddPinsToLayer()
        {
            foreach (PointOfInterest poi in Points)
            {
                MapPolygon polygon = new MapPolygon();
                polygon.Fill = new SolidColorBrush(_Colors[colorIndex++ % _Colors.Length]); 
                polygon.Opacity = 0.25;
                
                LocationCollection locCol = new LocationCollection();

                foreach( string line in  poi.Coordinates.Split('\n') )
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] vals = line.Split(',');
                        locCol.Add(
                            new Location()
                            {
                                Latitude = double.Parse(vals[1]),
                                Longitude = double.Parse(vals[0]),
                                Altitude = 0
                            });
                    }
                }
                polygon.Locations = locCol;
                MapLayer.Children.Add(polygon);
            }

        }
    }
}
