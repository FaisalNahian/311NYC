using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Shapes;
using Microsoft.Phone.Controls.Maps;
using _311NYC.Model;
using System.Collections.ObjectModel;

namespace _311NYC.Controls
{
    /// <summary>
    /// A push pin to display on a map
    /// </summary>
    public class PoiPushpin
    {
        /// <summary>
        /// since we cannot inherit from pushpin we set a reference here
        /// </summary>
        public Pushpin Pushpin { get; set; }

        public PoiPushpin(PointOfInterest poi)
        {
            Pushpin = new Microsoft.Phone.Controls.Maps.Pushpin();

            PointOfInterest = poi;

            Comments = new ObservableCollection<Comment>();
            Tags = new ObservableCollection<string>();
        }

        /// <summary>
        /// An Id that is loaded when the comments are loaded
        /// </summary>
        public string VanGuideId { get; set; }

        /// <summary>
        /// Ratings that is loaded only when the item is opened in details view
        /// </summary>
        public int Rating
        {
            get
            {
                return this.RatingTotal / 60;
            }
        }

        /// <summary>
        /// The text to display for the rating
        /// </summary>
        public string RatingText
        {
            get
            {
                return string.Format("Rated {0}/5 by {1} people.", Rating, this.RatingCount);
            }
        }

        /// <summary>
        /// Number of people that have rated the poi
        /// </summary>
        public int RatingCount { get; set; }

        /// <summary>
        /// Total ratings for the POI
        /// </summary>
        public int RatingTotal { get; set; }

        /// <summary>
        /// Comments for the poi only loaded when opened in details view
        /// </summary>
        public ObservableCollection<Comment> Comments { get; set; }
        
        /// <summary>
        /// Value to determin if comments have already been loaded
        /// </summary>
        public bool CommentsLoaded { get; set; }

        /// <summary>
        /// Tags associated with the POI only loaded when opened in details view
        /// </summary>
        public ObservableCollection<string> Tags { get; set; }

        /// <summary>
        /// Determins if tags have been loaded
        /// </summary>
        public bool TagsLoaded { get; set; }

        /// <summary>
        /// The id of the poi
        /// </summary>
        public string Id { get { return PointOfInterest.Id; } }

        /// <summary>
        /// The description for the pushpin.  This is again set when loaded in details view
        /// </summary>
        public string Description { get { return PointOfInterest.Description; } set { PointOfInterest.Description = value; } }

        /// <summary>
        /// The category for the pushpin
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// the actual object that gets loaded when the category is selected in the settings page
        /// </summary>
        public PointOfInterest PointOfInterest { get; private set; }

        
    }
}
