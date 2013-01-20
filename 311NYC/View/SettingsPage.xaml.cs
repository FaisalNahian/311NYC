using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using _311NYC.Model;
using System.Collections.Generic;
using _311NYC.ViewModel;
using System.Linq;
using System;
using _311NYC.DataLoader;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls.Maps.Core;
using Microsoft.Phone.Controls.Maps;
using System.IO.IsolatedStorage;
using System.ComponentModel.Composition;
using _311NYC.Helpers;

namespace _311NYC.View
{
    /// <summary>
    /// Description for SettingsPage.
    /// </summary>
    public partial class SettingsPage : PhoneApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the SettingsPage class.
        /// </summary>
        public SettingsPage()
        {
            InitializeComponent();

            //init composition
            CompositionInitializer.SatisfyImports(this);
            DataContext = this.SettingsViewModel;

            //wire up events
            PageTransitionList.Completed += new EventHandler(PageTransitionList_Completed);
            this.listBox1.Loaded += new RoutedEventHandler(ListBox1_Loaded);
            //this.lbSocialLandMarks.Loaded += new RoutedEventHandler(lbSocialLandMarks_Loaded);

        }

        [Import(typeof(SettingsViewModel))]
        public SettingsViewModel SettingsViewModel { get; set; }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            //animate page out
            PageTransitionList.Begin();

            base.OnBackKeyPress(e);
        }

        private void PageTransitionList_Completed(object sender, EventArgs e)
        {
            SettingsViewModel.NavigationHelper.NavigateBack();
        }

        private void ListBox1_Loaded(object sender, RoutedEventArgs e)
        {
            //Select items that have previously been selected
            //TODO how to data bind this?
            this.listBox1.SelectionChanged -= new SelectionChangedEventHandler(filter_SelectionChanged);
            IEnumerable<PointGroup> selected = SettingsViewModel.SelectedLandmarks;
            foreach (PointGroup pg in this.listBox1.Items)
            {
                if (selected.Contains(pg))
                    this.listBox1.SelectedItems.Add(pg);
            }
            this.listBox1.SelectionChanged += new SelectionChangedEventHandler(filter_SelectionChanged);
        }

       /* private void lbSocialLandMarks_Loaded(object sender, RoutedEventArgs e)
        {
            //Select items that have previously been selected
            //TODO how to data bind this?
            this.lbSocialLandMarks.SelectionChanged -= new SelectionChangedEventHandler(filter_SelectionChanged);
            IEnumerable<SharedLandmarkGroup> selected = SettingsViewModel.SelectedSocialLandmarks;
            foreach (SharedLandmarkGroup pg in this.lbSocialLandMarks.Items)
            {
                if (selected.Contains(pg))
                    this.lbSocialLandMarks.SelectedItems.Add(pg);
            }
            this.lbSocialLandMarks.SelectionChanged += new SelectionChangedEventHandler(filter_SelectionChanged);
        }*/

        private void filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingsViewModel.PointGroupFilterChanged.Execute(e);
        }

        private void lbMapStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlock tb = lbMapStyle.SelectedItem as TextBlock;
            if (tb != null)
                SettingsViewModel.MapStyleChanged.Execute(tb.Text);
        }

    }
}