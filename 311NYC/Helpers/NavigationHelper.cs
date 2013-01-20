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
using System.ComponentModel.Composition;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace _311NYC.Helpers
{
    /// <summary>
    /// Navigation for the app to go from page to page.  
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(NavigationHelper))]
    public class NavigationHelper
    {
        private const string SettingsPageUri = "/View/SettingsPage.xaml";
        private const string DetailsPageUri = "/View/DetailsPage.xaml";
        private const string AboutPage = "/View/About/About.xaml";

        public void NavigateToSettingsPage()
        {
            //navigate to the page
            NavigateToPage(new Uri(SettingsPageUri, UriKind.Relative));
        }

        public void NavigateToDetailsPage()
        {
            NavigateToPage(new Uri(DetailsPageUri, UriKind.Relative));
        }

        public void NavigateBack()
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void NavigateToPage(Uri uri)
        {
            this.NavigationService.Navigate(uri);
        }

        private NavigationService NavigationService
        {
            get
            {
                PhoneApplicationFrame root = Application.Current.RootVisual as PhoneApplicationFrame;
                PhoneApplicationPage page = root.Content as PhoneApplicationPage;
                if (root != null)
                    return page.NavigationService;
                else
                    return null;
            }
        }

        public void NavigateToHelpPage()
        {
            NavigateToPage(new Uri(AboutPage, UriKind.Relative));
        }
    }
}
