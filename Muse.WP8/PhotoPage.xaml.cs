using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Muse.WP8
{
    public partial class PhotoPage : PhoneApplicationPage
    {
        public PhotoPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            App.MuseService.CurrentItemIndex -= 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.Photo);
            radSlideView.MoveToPreviousItem();
            //DataContext = App.MuseService.CurrentItem;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            App.MuseService.CurrentItemIndex += 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.Photo);
            //DataContext = App.MuseService.CurrentItem;
            radSlideView.MoveToNextItem();
        }
    }
}