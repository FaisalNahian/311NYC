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
    public class SharedLandmark
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
    }
}
