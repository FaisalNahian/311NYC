﻿#pragma checksum "C:\Users\Faisal Nahian\Desktop\311NYC - 2-1 422am - adding login page\311NYC\Pages\LoginPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "03C0CA610331AD7DE3939892E4DC0C9C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.WindowsAzure.Samples.Phone.Identity.AccessControl;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace _311NYC.Pages {
    
    
    public partial class LoginPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Media.Animation.Storyboard PageTransitionReset;
        
        internal System.Windows.Media.Animation.Storyboard PageTransitionIn;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.WindowsAzure.Samples.Phone.Identity.AccessControl.AccessControlServiceSignIn SignInControl;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/311NYC;component/Pages/LoginPage.xaml", System.UriKind.Relative));
            this.PageTransitionReset = ((System.Windows.Media.Animation.Storyboard)(this.FindName("PageTransitionReset")));
            this.PageTransitionIn = ((System.Windows.Media.Animation.Storyboard)(this.FindName("PageTransitionIn")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.SignInControl = ((Microsoft.WindowsAzure.Samples.Phone.Identity.AccessControl.AccessControlServiceSignIn)(this.FindName("SignInControl")));
        }
    }
}

