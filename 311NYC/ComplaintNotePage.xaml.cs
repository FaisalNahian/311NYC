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
    public partial class ComplaintNotePage : PhoneApplicationPage
    {
        public ComplaintNotePage()
        {
            InitializeComponent();
        }

        private void DatePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

        }

        private void TimePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

        }

        // Warn users if back button is pressed
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            if (ComplaintTextBox.Text != string.Empty)
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
        private void NextButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("//ComplaintSummaryPage.xaml", UriKind.Relative));
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