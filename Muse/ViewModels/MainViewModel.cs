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

        private MuseService _muse = new Data.MuseService();
        public MuseService Muse { get { return _muse; } }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> Items { get { return Muse.Items; } }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> TourDates { get { return Muse.TourDates; } }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> Photos { get { return Muse.Photos; } }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            if (!Muse.LoadData())
            {
                MessageBox.Show("No internet connection is available. Try again later.");
            }
        }
        
    }
}