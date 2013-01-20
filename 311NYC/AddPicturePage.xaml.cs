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

using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media; 

namespace _311NYC
{
    public partial class AddPicturePage : PhoneApplicationPage
    {
        // Declare the CameraCaptureTask object with page scope.
        CameraCaptureTask cameraCaptureTask;
        PhotoChooserTask photoChooserTask;
        BitmapImage bmp;

        public AddPicturePage()
        {
            InitializeComponent();
            // Initialize the Chooser objects and assign the handlers for their "Completed" events
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(photoCaptureOrSelectionCompleted);
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoCaptureOrSelectionCompleted);
        }

        //picture handler
        void photoCaptureOrSelectionCompleted(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                bmp = new BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                myImage.Source = bmp;
                myImage.Stretch = Stretch.Uniform;
                // swap UI element states
                savePhotoButton.IsEnabled = true;
                statusText.Text = "";
            }
            else
            {
                savePhotoButton.IsEnabled = false;
                statusText.Text = "Task Result Error: " + e.TaskResult.ToString();
            }
        }

        // The camera Chooser is shown in response to a button click.
        private void takePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            cameraCaptureTask.Show();
        }

        // The photo Chooser shown in response to a button click.
        private void choosePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            photoChooserTask.Show();
        }

        private void savePhotoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create a filename for JPEG file in isolated storage.
                String tempJPEG = "TempJPEG";
                // Create virtual store and file stream. Check for duplicate tempJPEG files.
                var myStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (myStore.FileExists(tempJPEG))
                {
                    myStore.DeleteFile(tempJPEG);
                }
                IsolatedStorageFileStream myFileStream = myStore.CreateFile(tempJPEG);
                // Create a stream out of the sample JPEG file.
                // Instead of MyQuickApp in the URI, use the correct project name.
                // The use of TempJPEG was established earlier.
                Uri uri = new Uri("MyQuickApp;component/TempJPEG.jpg", UriKind.Relative);
                // Create a new WriteableBitmap object and set it to the JPEG stream.
                WriteableBitmap wb = new WriteableBitmap(bmp);
                // Encode WriteableBitmap object to a JPEG stream.
                // SaveJpeg(WriteableBitmap bitmap, Stream targetStream, int targetWidth,
                // int targetHeight, int orientation, int quality)
                Extensions.SaveJpeg(wb, myFileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
                myFileStream.Close();
                // Create a new stream from isolated storage, and save the JPEG file to
                // the media library on Windows Phone.
                myFileStream = myStore.OpenFile(tempJPEG, FileMode.Open, FileAccess.Read);
                MediaLibrary library = new MediaLibrary();
                Picture pic = library.SavePicture("SavedPicture.jpg", myFileStream);
                myFileStream.Close();
                savePhotoButton.IsEnabled = false;
                statusText.Text = "Saved!";
            }
            catch (Exception myError)
            {
                statusText.Text = myError.Message;
            }
        }

        // Navigation Bar
        private void NextButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("//ComplaintNotePage.xaml", UriKind.Relative));
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("//WherePage.xaml", UriKind.Relative));
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