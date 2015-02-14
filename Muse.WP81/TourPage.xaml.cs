using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Muse.WP81
{
    public partial class TourPage : PhoneApplicationPage
    {
        public TourPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            App.MuseService.CurrentItemIndex -= 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.Tour);
            radSlideView.MoveToPreviousItem(true);
        }

        private void Next_Click(object sender, EventArgs e)
        {
            App.MuseService.CurrentItemIndex += 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.Tour);
            radSlideView.MoveToNextItem(true);
        }

        private async void AddToCalendar_Click(object sender, EventArgs e)
        {
            if (await App.MuseService.AddToCalendar())
                MessageBox.Show("This tour date was added to your calendar.");
            else
                MessageBox.Show("Couldn't add date to calendar.");
        }
    }
}