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


namespace Muse
{
    public class MainViewModel
    {
        #region Fields


        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> Items { get { return Muse.App.MuseService.Items; } }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> TourDates { get { return Muse.App.MuseService.TourDates; } }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> Photos { get { return Muse.App.MuseService.Photos; } }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {

            if (!Muse.App.MuseService.LoadData())
            {
                MessageBox.Show("No internet connection is available. Try again later.");
            }
        }
        
    }
}