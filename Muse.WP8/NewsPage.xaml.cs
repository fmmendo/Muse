using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;



using System.Diagnostics;
using System.Text;
using System.Windows.Documents;
using System.Windows.Input;
using HtmlAgilityPack;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace Muse.WP8
{
    public partial class NewsPage : PhoneApplicationPage
    {
        public NewsPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            App.MuseService.CurrentItemIndex -= 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.News);
            radSlideView.MoveToPreviousItem(true);
        }

        private void Next_Click(object sender, EventArgs e)
        {
            App.MuseService.CurrentItemIndex += 1;
            App.MuseService.LoadItem(Data.MuseService.MuseDataType.News);
            radSlideView.MoveToNextItem(true);
        }
    }
}
