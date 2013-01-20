using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace _311NYC
{
    public class FeaturedComplaints : INotifyPropertyChanged
    {
        public FeaturedComplaints()
        {
            this.Items = new ObservableCollection<FeaturedItemViewModel>();
        }

        /// <summary>
        /// A collection for FeaturedItemViewModel objects.
        /// </summary>
        public ObservableCollection<FeaturedItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few FeaturedItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            this.Items.Add(new FeaturedItemViewModel() { ComplaintType = "Yellow Taxi Complaint", ComplaintSummary="Report taxi complaints and lost property", ComplaintDescription= "The City accepts complaints about yellow taxi drivers and their vehicles from passengers, observers, or people who were refused a pick-up. It is against the law for a taxi driver to refuse to pick you up because of race, disability, or destination within New York City while on duty. A taxi driver is required to drive you to any destination in the five boroughs, the counties of Nassau and Westchester and Newark/Liberty Airport."});
            this.Items.Add(new FeaturedItemViewModel() { ComplaintType = "Pothole", ComplaintSummary = "Report an street pothole", ComplaintDescription = "The City accepts requests and reports for the following conditions: Potholes - Circular-shaped shallow holes where the pavement has worn away. Cave-ins - Irregular-shaped deep holes where the street has collapsed. Utility damage - Street surfaces opened by utility companies and not restored to their original condition (i.e. asphalt is piled too high or too low or repaired patch has developed a pothole or cave-in.) Manhole covers and other street hardware defects - Missing, damaged, raised, sunken or noisy street hardware, as well as asphalt or concrete defects around them. The City also accepts requests to resurface streets that are rough, cracked, or pitted beyond repair." });
            this.Items.Add(new FeaturedItemViewModel() { ComplaintType = "Traffic Sign", ComplaintSummary="Report damaged and missing street sign", ComplaintDescription="The City accepts reports for the following street sign defects: - Street signs that are damaged, blocked, twisted, faded, defaced or incorrect - Signs that are dangling or leaning, including green rectangular signs dangling over intersections - Street signs that are missing completely - Sign stumps or poles sticking out of the ground - Missing signage at a construction site where work continues for more than three months" });
            this.Items.Add(new FeaturedItemViewModel() { ComplaintType = "Street Light", ComplaintSummary="Report a problem with street light", ComplaintDescription="/FeaturedComplaints_Files/appbar.city.png" });
            this.Items.Add(new FeaturedItemViewModel() { ComplaintType = "Open Fire Hydrant", ComplaintSummary="Report an open fire hydrant", ComplaintDescription="/FeaturedComplaints_Files/appbar.cone.png" });
            this.Items.Add(new FeaturedItemViewModel() { ComplaintType = "Graffiti", ComplaintSummary = "Report graffiti on buildings", ComplaintDescription = "You can report graffiti on buildings." });
            this.Items.Add(new FeaturedItemViewModel() { ComplaintType = "Dirty Vacant Lot", ComplaintSummary = "Report litter or debris for vacant lots", ComplaintDescription = "Report litter or debris for vacant lots only, those without a building or a building foundation." });
            this.Items.Add(new FeaturedItemViewModel() { ComplaintType = "Damaged Tree", ComplaintSummary = "Request removal of fallen or cracked trees", ComplaintDescription = "Request removal of fallen or cracked street trees or branches, including uprooted or leaning trees." });
            this.Items.Add(new FeaturedItemViewModel() { ComplaintType = "Street Condition", ComplaintSummary = "Report failed street repairs or defective street", ComplaintDescription = "You can report failed street repairs, defective street hardware or street plates, and rough, pitted, or cracked roads." });
            this.Items.Add(new FeaturedItemViewModel() { ComplaintType = "More", ComplaintSummary = "All other complaint types from A-Z", ComplaintDescription = "/FeaturedComplaints_Files/appbar.next.rest.png" });

            
            
            

            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}