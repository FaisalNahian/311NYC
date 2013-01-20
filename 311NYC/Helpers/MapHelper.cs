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
using Microsoft.Phone.Controls.Maps.Core;
using Microsoft.Phone.Controls.Maps;

namespace _311NYC.Helpers
{
    public static class MapHelper
    {
        public static MapMode GetMapModeForString(string mode)
        {
            switch (mode)
            {
                case "Road":
                    return new RoadMode();
                case "Satellite":
                    return new AerialMode(false);
                case "Hybrid":
                    return new AerialMode(true);
                default:
                    break;
            }

            return null;
        }
    }
}
