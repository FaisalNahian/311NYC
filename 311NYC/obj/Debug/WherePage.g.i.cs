﻿#pragma checksum "C:\Users\Faisal Nahian\Desktop\311NYC v1.1 by Faisal Nahian\311NYC\WherePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7B7232F95D579712458084191D7CF052"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
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


namespace _311NYC {
    
    
    public partial class WherePage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid Where;
        
        internal Microsoft.Phone.Controls.PhoneTextBox streetAddressTextBox;
        
        internal Microsoft.Phone.Controls.PhoneTextBox cityTextBox;
        
        internal Microsoft.Phone.Controls.PhoneTextBox ZipCodeTextBox;
        
        internal System.Windows.Controls.TextBlock textBlock4;
        
        internal System.Windows.Controls.Button currentLocation;
        
        internal System.Windows.Controls.TextBlock textBlock1;
        
        internal System.Windows.Controls.TextBlock textBlock2;
        
        internal System.Windows.Controls.TextBlock LongitudeTextBlock;
        
        internal System.Windows.Controls.TextBlock LatitudeTextBlock;
        
        internal System.Windows.Controls.TextBlock textBlock3;
        
        internal System.Windows.Controls.TextBlock statusTextBlock;
        
        internal Microsoft.Phone.Controls.Maps.Map myMap;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/311NYC;component/WherePage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.Where = ((System.Windows.Controls.Grid)(this.FindName("Where")));
            this.streetAddressTextBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("streetAddressTextBox")));
            this.cityTextBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("cityTextBox")));
            this.ZipCodeTextBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("ZipCodeTextBox")));
            this.textBlock4 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock4")));
            this.currentLocation = ((System.Windows.Controls.Button)(this.FindName("currentLocation")));
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock1")));
            this.textBlock2 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock2")));
            this.LongitudeTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("LongitudeTextBlock")));
            this.LatitudeTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("LatitudeTextBlock")));
            this.textBlock3 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock3")));
            this.statusTextBlock = ((System.Windows.Controls.TextBlock)(this.FindName("statusTextBlock")));
            this.myMap = ((Microsoft.Phone.Controls.Maps.Map)(this.FindName("myMap")));
        }
    }
}
