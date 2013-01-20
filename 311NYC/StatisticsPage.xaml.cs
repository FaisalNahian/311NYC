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

using System.Collections.ObjectModel;

namespace _311NYC
{

    public partial class StatisticsPage : PhoneApplicationPage
    {
        // Constructor
        public StatisticsPage()
        {
            InitializeComponent();
        }
        /*
        private ObservableCollection<TestDataItem> _data = new ObservableCollection<TestDataItem>()
        {
            new TestDataItem() { cat1 = "cat1", val1=5, val2=15, val3=12},
            new TestDataItem() { cat1 = "cat2", val1=15.2, val2=1.5, val3=2.1},
            new TestDataItem() { cat1 = "cat3", val1=25, val2=5, val3=2},
            new TestDataItem() { cat1 = "cat4", val1=8.1, val2=1, val3=22},
        };

        public ObservableCollection<TestDataItem> Data { get { return _data; } }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }

    }

    public class TestDataItem
    {
        public string cat1 { get; set; }
        public double val1 { get; set; }
        public double val2 { get; set; }
        public double val3 { get; set; }
    }*/

        // Pie Charts

        private ObservableCollection<PData> _data = new ObservableCollection<PData>()
        {
            new PData() { title = "Complaint Created", value = 954},
            new PData() { title = "Complaint Closed", value = 488},
        };

/*
        private ObservableCollection<PData> _data = new ObservableCollection<PData>()
        {
            new PData() { title = "Taxi Complaints", value = 58 },
            new PData() { title = "Street Condition", value = 42 },
            new PData() { title = "Heating", value = 42 },
            new PData() { title = "Consumer Complaints", value = 20 },
            new PData() { title = "Noise", value = 74 },
            new PData() { title = "Paint-Plaster", value = 74 },
            new PData() { title = "Plumbing", value = 154 },
            new PData() { title = "Damaged Tree", value = 71 },
            new PData() { title = "General Construction", value = 129 },
        };*/

        public ObservableCollection<PData> Data { get { return _data; } }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }
    }

    public class PData
    {
        public string title { get; set; }
        public double value { get; set; }
    }
    
}