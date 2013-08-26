using Microsoft.Phone.Controls;
using Muse.Data;
using Muse.Data.RSS;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Navigation;
using System.Xml.Linq;


namespace MuseWP8
{
    public class MainViewModel
    {
        #region Fields


        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> Items { get { return MuseWP8.App.MuseService.Items; } }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> TourDates { get { return MuseWP8.App.MuseService.TourDates; } }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> Photos { get { return MuseWP8.App.MuseService.Photos; } }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {

            if (!MuseWP8.App.MuseService.LoadData())
            {
                MessageBox.Show("No internet connection is available. Try again later.");
            }
        }
        
    }
}