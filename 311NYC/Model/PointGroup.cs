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
using System.Collections;
using System.Collections.Generic;
using Microsoft.Phone.Controls.Maps;
using _311NYC.Controls;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Messaging;
using _311NYC.DataLoader;

namespace _311NYC.Model
{
    public class PointGroup : IEqualityComparer<PointGroup>
    {
        public PointGroup()
        {
            MapLayer = new MapLayer();
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string DataUrl { get; set; }
        public string IconUri { get; set; }
        public PointGroupType Type { get; set; }
        public bool Selected { get; set; }
        public bool Loaded { get; set; }

        /// <summary>
        /// The object that will load the data
        /// </summary>
        public IPointGroupDataLoader DataLoader { get; set; }

        public IEnumerable<PointOfInterest> Points { get; set; }
        public MapLayer MapLayer { get; set; }

        public virtual void AddPinsToLayer()
        {
            //create an new brush
            //Image image = null;
            ////ImageBrush brush = null;
            //try
            //{
            //    if (this.IconUri != null)
            //    {
            //        //brush = new ImageBrush();
            //        //brush.ImageSource = new BitmapImage(new Uri(this.IconUri));
            //        image = new Image() { Source = new BitmapImage(new Uri(this.IconUri)) };
            //    }
            //}
            //catch { /* ignore errors here */}

            //create all the pushpins for the point group
            foreach (PointOfInterest poi in Points)
            {
                PoiPushpin p = new PoiPushpin(poi)
                {
                    Category = this.Title,
                };
                p.Pushpin.Background = new SolidColorBrush(Color.FromArgb(127, 0, 0, 0));
                //p.Pushpin.Content= brush;
                p.Pushpin.Content = new Image() { Source = new BitmapImage(new Uri(this.IconUri)) , Stretch= Stretch.None }; 
                p.Pushpin.Location = poi.Location;

                p.Pushpin.MouseLeftButtonDown += new MouseButtonEventHandler(MouseLeftButtonDown);
                p.Pushpin.Tag = p;
 
                MapLayer.Children.Add(p.Pushpin);
            }

        }

        public virtual void RemovePinsFromLayer()
        {
            MapLayer.Children.Clear();
        }


        void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Pushpin pin = sender as Pushpin;
            if (pin != null)
            {

                PoiPushpin poi = pin.Tag as PoiPushpin;
                if (pin != null)
                {
                    MapLayer.Children.Remove(pin);
                    MapLayer.Children.Add(pin);
                    Messenger.Default.Send<PoiPushpin>(poi, MessageTokens.PoiPushpinClicked);
                }
            }
        }

        #region IEqualityComparer<PointGroup> Members

        public bool Equals(PointGroup x, PointGroup y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(PointGroup obj)
        {
            return base.GetHashCode();
        }

        #endregion


        /*
         <VisualStateManager.VisualStateGroups>
            <VisualStateGroup x:Name="VisualStateGroup">
                <VisualStateGroup.Transitions>
                    <VisualTransition GeneratedDuration="0:0:0.5">
                        <VisualTransition.GeneratedEasingFunction>
                            <BackEase EasingMode="EaseOut"/>
                        </VisualTransition.GeneratedEasingFunction>
                    </VisualTransition>
                </VisualStateGroup.Transitions>
                <VisualState x:Name="DefaultPinVisualState">
                    <Storyboard>
                        <DoubleAnimation Duration="0" To="0" Storyboard.TargetProperty="(UIElement.RenderTransform).(CompositeTransform.TranslateY)" Storyboard.TargetName="test" d:IsOptimized="True"/>
                    </Storyboard>
                </VisualState>
                <VisualState x:Name="SelectedPinVisualState">
                    <Storyboard>
                        <DoubleAnimation Duration="0" To="2" Storyboard.TargetProperty="(UIElement.RenderTransform).(CompositeTransform.ScaleX)" Storyboard.TargetName="test" d:IsOptimized="True"/>
                        <DoubleAnimation Duration="0" To="2" Storyboard.TargetProperty="(UIElement.RenderTransform).(CompositeTransform.ScaleY)" Storyboard.TargetName="test" d:IsOptimized="True"/>
                        <DoubleAnimation Duration="0" To="0" Storyboard.TargetProperty="(UIElement.RenderTransform).(CompositeTransform.TranslateX)" Storyboard.TargetName="test" d:IsOptimized="True"/>
                        <DoubleAnimation Duration="0" To="-21" Storyboard.TargetProperty="(UIElement.RenderTransform).(CompositeTransform.TranslateY)" Storyboard.TargetName="test" d:IsOptimized="True"/>
                    </Storyboard>
                </VisualState>
            </VisualStateGroup>
        </VisualStateManager.VisualStateGroups>
         */

    }

    public enum PointGroupType
    {
        Landmarks,
        Social
    }
}
