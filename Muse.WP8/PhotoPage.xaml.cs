using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Muse.WP8
{
    public partial class PhotoPage : PhoneApplicationPage
    {
        WebClient photoDownloader;
        public PhotoPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;

            photoDownloader = new WebClient();
            photoDownloader.OpenReadCompleted += WebClient_OpenReadCompleted;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            photoDownloader.OpenReadAsync(new Uri(App.MuseService.CurrentItem.ImageURL));
        }

        private void Back_Click(object sender, EventArgs e)
        {
            App.MuseService.CurrentItemIndex -= 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.Photo);
            radSlideView.MoveToPreviousItem();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            App.MuseService.CurrentItemIndex += 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.Photo);
            radSlideView.MoveToNextItem();
        }

        private void WebClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            using (var mediaLibrary = new MediaLibrary())
            {
                mediaLibrary.SavePicture(Path.GetFileName(App.MuseService.CurrentItem.ImageURL), e.Result);
            }

            MessageBox.Show("Photo saved");
        }
    }
}