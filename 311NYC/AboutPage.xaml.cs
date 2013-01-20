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
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void Email_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Microsoft.Phone.Tasks.EmailComposeTask ect = new Microsoft.Phone.Tasks.EmailComposeTask();
            ect.Subject = "311 NYC for Windows Phone 7 Feedback";
            ect.To = "buzz@heyfaisal.com";
            ect.Show();
        }
    }
}