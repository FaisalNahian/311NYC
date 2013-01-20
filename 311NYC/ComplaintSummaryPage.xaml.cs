using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace _311NYC
{
    public partial class ComplaintSummaryPage : PhoneApplicationPage
    {
        public ComplaintSummaryPage()
        {
            InitializeComponent();
        }

        // Brings All User Input Details for Submission
        // Need to implement IO Storage. 
         protected override void OnNavigatedTo
        (System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string msg = "";
            if (NavigationContext.QueryString.TryGetValue("msg", out msg))
                SummaryTB.Text = msg;
        }

        // Warn users if back button is pressed
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            {
                var result = MessageBox.Show("You are about to discard your changes. Continue?",
                "Warning", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK)
                {
                    e.Cancel = true;
                }
            }
        }

        // Navigation Bar
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Thanks for submitting your complaint and taking part in improving your city. Go to Main Page?",
                "Warning", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.Cancel)
            {
                NavigationService.Navigate(new Uri("//MainPage.xaml", UriKind.Relative));
             }
            //NavigationService.Navigate(new Uri("//MainPage.xaml", UriKind.Relative));
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("//AddPicturePage.xaml", UriKind.Relative));
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("//View/SettingsPage.xaml", UriKind.Relative));
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("//AboutPage.xaml", UriKind.Relative));
        }
    }
}