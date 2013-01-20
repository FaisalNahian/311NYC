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
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Collections.Generic;

namespace RedBit.WindowsPhone.Converters
{
    public class ThemedImageConverter : IValueConverter
    {
        private static  Dictionary<String, BitmapImage> imageCache = new Dictionary<string, BitmapImage>();
        private static Visibility currentTheme;
        private static string assetPath = "/Images/Light/";
       
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {           
            BitmapImage result = null;
            // Detect current theme
            this.DetectTheme((Color)parameter);
            // Path to the icon image
            string path = assetPath + (string)value;

            //create a new one
            Uri source = new Uri(path, UriKind.Relative);
            result = new BitmapImage(source);

            return result;
            //// Check if we already cached the image
            //if (!imageCache.TryGetValue(path, out result))
            //{
                
            //    imageCache.Add(path, result);
            //}

            //return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private void DetectTheme(Color color)
        {            
            Visibility lightThemeVisibility = Visibility.Collapsed;
            
            if (color == Colors.White)
            {
                lightThemeVisibility = Visibility.Visible;
            }

            // Check if the theme changed
            if (currentTheme != lightThemeVisibility)
            {
                currentTheme = lightThemeVisibility;

                if (lightThemeVisibility == Visibility.Visible)
                {
                    assetPath = "/Images/Light/";
                }
                else
                {
                    assetPath = "/Images/Dark/";
                }
            }
        }
    }

}
