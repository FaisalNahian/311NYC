using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using GalaSoft.MvvmLight;
using _311NYC.Helpers;
using System.Reflection;
using System.IO.IsolatedStorage;

namespace _311NYC.ViewModel
{
    [Export("AboutViewModel", typeof(AboutViewModel))]
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
        }



        private const string m_aboutTextFormat2 = "{1} v{0}. Designed and engineered by RedBit Development. \r\n\r\n\r\n© 2010 All Rights Reserved.";
        private const string m_aboutTextFormat = "Vanguide for Windows Phone 7 allows a user to view various landmarks in and around the Vancouver area.  The user has the ability to view comments, ratings or tags to the landmark and add their own comments, ratings or tags. VanGuide for Windows Phone 7 is an extension to the current version of VanGuide for web available and is based on the Open Data Application Framework.  From more information on VanGuide or to find out more about RedBit Development please visit the following link.";
        private string m_AboutText;
        private string m_AboutText2;
        public string AboutText
        {
            get
            {
                if (m_AboutText == null)
                    m_AboutText = m_aboutTextFormat;
                return m_AboutText;
            }
        }

        public string AboutText2
        {
            get
            {
                if (m_AboutText2 == null)
                    m_AboutText2 = string.Format(m_aboutTextFormat2, Version, "VanGuide");
                return m_AboutText2;
            }
        }

        private string Version
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly().FullName;
                return assembly.Split('=')[1].Split(',')[0];
            }
        }

        public bool UseUserLocation
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("useGps");
            }
            set
            {
                if (value)
                    IsolatedStorageSettings.ApplicationSettings.Add("useGps", value);
                else
                    IsolatedStorageSettings.ApplicationSettings.Remove("useGps");

                IsolatedStorageSettings.ApplicationSettings.Save();

                RaisePropertyChanged("UseUserLocation");
            }
        }

        /// <summary>
        /// Helper class for navigation to different pages
        /// </summary>
        public NavigationHelper NavigationHelper { get; private set; }

    }
}
