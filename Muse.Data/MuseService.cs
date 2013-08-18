using Muse.Data.RSS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml.Linq;

namespace Muse.Data
{
    public class MuseService : INotifyPropertyChanged
    {
        private const string NEWSRSS = "http://muse.mu/rss/news/";
        private const string TOURRSS = "http://muse.mu/rss/gigs/";
        private const string GALLERYRSS = "http://muse.mu/rss/photos/";

        public bool IsDataLoaded { get; private set; }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> Items { get; private set; }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> TourDates { get; private set; }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<MuseRSSItem> Photos { get; private set; }

        public MuseRSSItem CurrentItem { get; private set; }
        public int CurrentItemIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public MuseService()
        {
            IsDataLoaded = false;
            CurrentItemIndex = -1;

            Items = new ObservableCollection<MuseRSSItem>();
            Photos = new ObservableCollection<MuseRSSItem>();
            TourDates = new ObservableCollection<MuseRSSItem>();
        }

        public bool LoadData()
        {
            if (!NetworkInterface.GetIsNetworkAvailable()) return false;
            if (IsDataLoaded) return true;

            WebClient wc_news = new WebClient();
            wc_news.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted_news);
            wc_news.DownloadStringAsync(new Uri(NEWSRSS));

            WebClient wc_tour = new WebClient();
            wc_tour.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted_tour);
            wc_tour.DownloadStringAsync(new Uri(TOURRSS));

            WebClient wc_photos = new WebClient();
            wc_photos.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted_photos);
            wc_photos.DownloadStringAsync(new Uri(GALLERYRSS));

            IsDataLoaded = true;
            return IsDataLoaded;
        }

        public void LoadItem(MuseDataType type)
        {
            switch (type)
            {
                case MuseDataType.News:
                    if (CurrentItemIndex >= 0 && Items.Count > CurrentItemIndex)
                        CurrentItem = Items[CurrentItemIndex];
                    break;
                case MuseDataType.Tour:
                    if (CurrentItemIndex >= 0 && TourDates.Count > CurrentItemIndex)
                        CurrentItem = TourDates[CurrentItemIndex];
                    break;
                case MuseDataType.Photo:
                    if (CurrentItemIndex >= 0 && Photos.Count > CurrentItemIndex)
                        CurrentItem = Photos[CurrentItemIndex];
                    break;
            }
            NotifyPropertyChanged("CurrentItem");
            CurrentItem.NotifyAll();
        }

        public enum MuseDataType
        {
            News,
            Tour,
            Photo
        }

        /// <summary>
        /// Loads news data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void wc_DownloadStringCompleted_news(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null) return;

            var xmlItems = System.Xml.Linq.XElement.Parse(e.Result);

            foreach (var item in xmlItems.Descendants("item"))
            {
                var rssItem = new MuseRSSItem(item.Element("title").Value,
                                              item.Element("link").Value,
                                              item.Element("description").Value,
                                              item.Element("pubDate").Value);
                if (!Items.Contains(rssItem))
                {
                    Items.Add(rssItem);
                }
            }

            if (Items.Count <= 0)
            {
                var rssItem = new MuseRSSItem("No News", "", "No news have been published.", "");
                Items.Add(rssItem);
            }
        }

        /// <summary>
        /// Loads tour data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void wc_DownloadStringCompleted_tour(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null) return;

            var xmlItems = System.Xml.Linq.XElement.Parse(e.Result);

            foreach (var item in xmlItems.Descendants("item"))
            {
                var rssItem = new MuseRSSItem(item.Element("title").Value,
                                              item.Element("link").Value,
                                              item.Element("description").Value,
                                              item.Element("pubDate").Value);
                if (!TourDates.Contains(rssItem))
                {
                    //DateTime dt = DateTime.Parse(rssItem.Day + " " + rssItem.MonthYear);
                    if (DateTime.Compare(DateTime.Today, rssItem.TourDate) > 0)
                    {
                        continue;
                    }
                    int index = -1;
                    foreach (var tourDate in TourDates)
                    {
                        if (DateTime.Compare(rssItem.TourDate, tourDate.TourDate) > 0)
                        {
                            index = TourDates.IndexOf(tourDate);
                            continue;
                        }
                    }
                    TourDates.Insert(index + 1, rssItem);
                }
            }

            if (TourDates.Count <= 0)
            {
                var rssItem = new MuseRSSItem("No Tour Dates", "", "No future dates have been announced.", "");
                TourDates.Add(rssItem);
            }
        }

        /// <summary>
        /// Loads Gallery data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void wc_DownloadStringCompleted_photos(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null) return;

            var xmlItems = System.Xml.Linq.XElement.Parse(e.Result);

            foreach (var item in xmlItems.Descendants("item"))
            {
                var rssItem = new MuseRSSItem(item.Element("title").Value,
                                              item.Element("link").Value,
                                              item.Element("description").Value,
                                              item.Element("pubDate").Value);
                if (!Photos.Contains(rssItem))
                {
                    Photos.Add(rssItem);
                }
            }

            if (Photos.Count <= 0)
            {
                var rssItem = new MuseRSSItem("No photos", "", "No photos in gallery", "");
                Photos.Add(rssItem);
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
