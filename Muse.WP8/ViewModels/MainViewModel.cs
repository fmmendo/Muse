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
using Muse.Data.RSS;
using Microsoft.Phone.Tasks;

namespace Muse
{
    public class MainViewModel
    {
        #region Fields


        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> Items { get { return Muse.WP8.App.MuseService.Items; } }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> TourDates { get { return Muse.WP8.App.MuseService.TourDates; } }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> Photos { get { return Muse.WP8.App.MuseService.Photos; } }

        public int CurrentIndex { get { return Muse.WP8.App.MuseService.CurrentItemIndex; } }
        public MuseRSSItem CurrentItem { get { return Muse.WP8.App.MuseService.CurrentItem; } }
        #endregion

        public ButtonCommand NavigateTo {get;set;}

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            if (!Muse.WP8.App.MuseService.LoadData())
            {
                MessageBox.Show("No internet connection is available. Try again later.");
            }

            NavigateTo = new ButtonCommand(OnExecuteNavigateTo, CanExecuteNavigateTo);
        }

        private bool CanExecuteNavigateTo(object parameter)
        {
            return true;
        }

        private void OnExecuteNavigateTo(object parameter)
        {
            WebBrowserTask webBrowserTask = new WebBrowserTask();
            webBrowserTask.Uri = new Uri(parameter as string);
            webBrowserTask.Show();
        }

    }
}