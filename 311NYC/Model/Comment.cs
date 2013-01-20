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
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace _311NYC.Model
{
    public class Comment : INotifyPropertyChanged
    {
        private string text;
        private Uri picUrl;
        private string author;



        public string Text
        {
            get { return text; }
            set
            {
                if (value != text)
                {
                    text = value;
                    onPropertyChanged(this, "Text");
                }
            }
        }

        public string Author
        {
            get { return author; }
            set
            {
                if (value != author)
                {
                    author = value;
                    onPropertyChanged(this, "Author");
                    DeterminePicFromName();
                }
            }
        }

        private void DeterminePicFromName()
        {
            //TODO: Implement a Avatar cache.
            if (string.IsNullOrEmpty(Author))
                return;
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(DownloadPicCompleted);
            client.DownloadStringAsync(new Uri( string.Format("http://api.twitter.com/1/users/show/{0}.json", Author)));
        }

        void DownloadPicCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //TODO: If this fails, Twitter is probably over capacity so we should just use a default image.
            if (e.Error != null)
            {
                //MessageBox.Show(e.Error.Message + Environment.NewLine + e.Error.StackTrace,
                //    "Error", MessageBoxButton.OK);
                return;
            }
            JObject o = JObject.Parse(e.Result);
            string url = (string)o.SelectToken("profile_image_url");
            PicUrl = new Uri(url);
        }

        public Uri PicUrl
        {
            get { return picUrl; }
            set
            {
                if (value != picUrl)
                {
                    picUrl = value;
                    onPropertyChanged(this, "PicUrl");
                }
            }
        }



        #region  INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged(object sender, string propertyName)
        {

            if (this.PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
