using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Xml.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;


namespace Muse
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields
        private const string NEWSRSS = "http://muse.mu/rss/news/";
        private const string TOURRSS = "http://muse.mu/rss/gigs/";
        private const string GALLERYRSS = "http://muse.mu/rss/photos/";

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> TourDates { get; private set; }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Photos { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDataLoaded { get; private set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            IsInternetAvailable();

            IsDataLoaded = false;

            this.Items = new ObservableCollection<ItemViewModel>();
            this.TourDates = new ObservableCollection<ItemViewModel>();
            this.Photos = new ObservableCollection<ItemViewModel>();
        }

        #region Load Data
        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            if (!IsDataLoaded)
            {
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
            }
        }

        /// <summary>
        /// Loads news data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void wc_DownloadStringCompleted_news(object sender, DownloadStringCompletedEventArgs e)
        {
            //If an error occured during download exit 
            if (e.Error != null)
            {
                return;
            }

            XElement xmlItems = XElement.Parse(e.Result);

            foreach (var item in xmlItems.Descendants("item"))
            {
                ItemViewModel rssItem = new ItemViewModel(item.Element("title").Value,
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
                ItemViewModel rssItem = new ItemViewModel("No News", "", "No news have been published.", "");
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
            //If an error occured during download exit 
            if (e.Error != null)
            {
                return;
            }

            XElement xmlItems = XElement.Parse(e.Result);

            foreach (var item in xmlItems.Descendants("item"))
            {
                ItemViewModel rssItem = new ItemViewModel(item.Element("title").Value,
                                                          item.Element("link").Value,
                                                          item.Element("description").Value,
                                                          item.Element("pubDate").Value );
                if (!TourDates.Contains(rssItem))
                {
                    DateTime dt = DateTime.Parse(rssItem.Day + " " + rssItem.MonthYear);
                    if (DateTime.Compare(DateTime.Today, dt) > 0)
                    {
                        continue;
                    }
                    int index = -1;
                    foreach(var tourDate in TourDates)
                    {
                        DateTime dt2 = DateTime.Parse(tourDate.Day + " " + tourDate.MonthYear);
                        if (DateTime.Compare(dt, dt2) > 0)
                        {
                            index = TourDates.IndexOf(tourDate);
                            continue;
                        }
                    }
                    //TourDates.Add(rssItem);
                    TourDates.Insert(index+1, rssItem);
                }
            }

            if (TourDates.Count <= 0)
            {
                ItemViewModel rssItem = new ItemViewModel("No Tour Dates", "", "No future dates have been announced.", "");
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
            //If an error occured during download exit 
            if (e.Error != null)
            {
                return;
            }

            XElement xmlItems = XElement.Parse(e.Result);

            foreach (var item in xmlItems.Descendants("item"))
            {
                ItemViewModel rssItem = new ItemViewModel(item.Element("title").Value,
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
                ItemViewModel rssItem = new ItemViewModel("No photos", "", "No photos in gallery", "");
                Photos.Add(rssItem);
            }
        }
        #endregion

        private bool IsInternetAvailable()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("No internet connection is available. Try again later.");
                return false;
            }
            return true;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}