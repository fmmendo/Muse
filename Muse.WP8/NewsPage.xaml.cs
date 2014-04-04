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
    public partial class NewsPage : PhoneApplicationPage
    {
        public NewsPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;//urrentItem;
        }
        
        private void Back_Click(object sender, EventArgs e)
        {
            App.MuseService.CurrentItemIndex -= 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.News);
            radSlideView.MoveToPreviousItem(true);
            //NavigationService.Navigate(new Uri("/NewsPage.xaml", UriKind.Relative));
            //DataContext = App.MuseService.CurrentItem;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            App.MuseService.CurrentItemIndex += 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.News);
            radSlideView.MoveToNextItem(true);
            //NavigationService.Navigate(new Uri("/NewsPage.xaml", UriKind.Relative));
            //DataContext = App.MuseService.CurrentItem;
        }
    }
}