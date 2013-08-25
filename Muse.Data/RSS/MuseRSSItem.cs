using System;
using System.ComponentModel;

namespace Muse.Data.RSS
{
    public sealed class MuseRSSItem : RSSItem
    {
        #region Properties
        private string _itemID;
        /// <summary>
        /// Number that identifies an item.
        /// </summary>
        /// <returns></returns>
        public string ItemID
        {
            get
            {
                return _itemID;
            }
            private set
            {
                if (value != _itemID)
                {
                    _itemID = value;
                    NotifyPropertyChanged("ItemID");
                }
            }
        }


        public string _newsBody;
        public string NewsBody
        {
            get { return _newsBody; }
            private set
            {
                if (value != _newsBody)
                {
                    _newsBody = value;
                    NotifyPropertyChanged("NewsBody");
                }
            }
        }

        private string _imageURL;
        /// <summary>
        /// Image, url to the image that accompanies the news item.
        /// </summary>
        /// <returns></returns>
        public string ImageURL
        {
            get
            {
                return _imageURL;
            }
            set
            {
                if (value != _imageURL)
                {
                    _imageURL = value;
                    NotifyPropertyChanged("ImageURL");
                }
            }
        }

        private string _imageThumb;
        /// <summary>
        /// Image, url to the image that accompanies the news item.
        /// </summary>
        /// <returns></returns>
        public string ImageThumb
        {
            get
            {
                return _imageThumb;
            }
            set
            {
                if (value != _imageThumb)
                {
                    _imageThumb = value;
                    NotifyPropertyChanged("ImageThumb");
                }
            }
        }

        /// <summary>
        /// MonthYear, A string that has the month and year of a gig
        /// </summary>
        /// <returns></returns>
        public string MonthYear
        {
            get
            {
                return TourDate.ToString("MMM yyyy");
            }
        }

        /// <summary>
        /// Day of the gig
        /// </summary>
        /// <returns></returns>
        public int Day
        {
            get
            {
                return TourDate.Day;
            }
        }

        private DateTime _tourDate;
        /// <summary>
        /// TourDate, holds the date for a gig.
        /// </summary>
        /// <returns></returns>
        public DateTime TourDate
        {
            get
            {
                return _tourDate;
            }
            set
            {
                if (value != _tourDate)
                {
                    _tourDate = value;

                    NotifyPropertyChanged("TourDate");
                    NotifyPropertyChanged("Day");
                    NotifyPropertyChanged("MonthYear");
                }
            }
        }
        public string TourDateString
        {
            get { return _tourDate.ToString("dd MMM yyyy"); }
        }
        #endregion

        /// <summary>
        /// Constructor.
        /// Creates the RSS item and then processes the data to populate Muse related fields.
        /// </summary>
        /// <param name="title">Title of the item</param>
        /// <param name="link">Link to the full story</param>
        /// <param name="description">Content</param>
        /// <param name="pubdate">Publication date</param>
        public MuseRSSItem(string title, string link, string description, string pubdate)
        {
            ClearAll();

            /* Deal with the RSS bit */
            Title = title;
            Link = link;
            Description = description;
            //Description = GetDescription(description);
            PubDate = pubdate;
            GetTourDate();
            /* Prepare some muse specific data */

            //GetDescription();
            Description = GetDescription(description);
            LoadHeavyDutyData();
        }

        /// <summary>
        /// Does the rest of the necessary loading, pulling images, scraping pages, etc..
        /// </summary>
        internal void LoadHeavyDutyData()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// Clear all data
        /// </summary>
        private void ClearAll()
        {
            Title = "";
            Link = "";
            Description = "";
            PubDate = "";

            ItemID = "";
            ImageURL = "";
            TourDate = new DateTime();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            GetImageAndNewsBody();
        }

        /// <summary>
        /// Gets the TourDate and removes it from the title.
        /// </summary>
        private void GetTourDate()
        {
            //DateTime date;
            //if (Title.Length > 10 && DateTime.TryParse(Title.Substring(0, 10), out date))
            //{
            //    TourDate = date;
            //    Title = Title.Remove(0, 13);
            //}

            int start = -1;
            int end = -1;

            start = Description.IndexOf("<b>On:</b>");
            end = Description.IndexOf("<br />");

            if (start >= 0 && end > 0 && end > start)
            {
                string strDate = Description.Substring(start + 10, end - (start + 10)).Trim();
                TourDate = DateTime.Parse(strDate);
            }
        }
        private string GetDescription(string src)
        {
            var desc = src;

            //if (String.IsNullOrEmpty(MonthYear))
            //{
            //    int start = -1;
            //    int end = -1;

            //    start = desc.IndexOf("<b>On:</b>");
            //    end = desc.IndexOf("<br />");

            //    if (start >= 0 && end > 0 && end > start)
            //    {
            //        string date = desc.Substring(start + 10, end - (start + 10)).Trim();
            //        string[] split = date.Split(new char[] { ' ' });
            //        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            //        {
            //            TourDate = DateTime.Parse(String.Format("{0} {1} {2}", split[0], split[1], split[2]));
            //        });
            //    }
            //}

            // remove the ImageURL from the description
            if (String.IsNullOrEmpty(ImageURL))
            {
                int startImage = -1;
                int endImage = -1;
                int endtag = -1;

                startImage = desc.IndexOf("<img src=\"", 0);
                if (startImage >= 0)
                {
                    startImage += 10;
                    endImage = desc.IndexOf("\" ", startImage);
                    if (endImage >= startImage)
                    {
                        endtag = desc.IndexOf(">", endImage);
                        if (endtag >= endImage)
                        {
                            //System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                            //{
                                _imageThumb = desc.Substring(startImage, endImage - startImage);
                                _imageURL = ImageThumb.Replace("thumb", "original");
                                desc = desc.Substring(endtag + 1);

                                NotifyPropertyChanged("ImageThumb");
                                NotifyPropertyChanged("ImageURL");
                            //});
                        }
                    }
                }
            }

            while (desc.Contains("<a href"))
            {
                int start = -1;
                int end = -1;

                start = desc.IndexOf("<a href", 0);
                end = desc.IndexOf(">", start);
                var str = desc.Substring(start, end - start + 1);
                desc = desc.Replace(str, "");
            }
            while (desc.Contains("<p style"))
            {
                int start = -1;
                int end = -1;

                start = desc.IndexOf("<p style", 0);
                end = desc.IndexOf(">", start + 1);
                var str = desc.Substring(start, end - start + 1);
                desc = desc.Replace(str, "");
            }
            while (desc.Contains("<span "))
            {
                int start = -1;
                int end = -1;

                start = desc.IndexOf("<span ", 0);
                end = desc.IndexOf(">", start + 1);
                var str = desc.Substring(start, end - start + 1);
                desc = desc.Replace(str, "");
            }
            while (desc.Contains("<iframe "))
            {
                int start = -1;
                int end = -1;

                start = desc.IndexOf("<iframe ", 0);
                end = desc.IndexOf(">", start + 1);
                var str = desc.Substring(start, end - start + 1);
                desc = desc.Replace(str, "");
            }
            while (desc.Contains("<img"))
            {
                int start = -1;
                int end = -1;

                start = desc.IndexOf("<img", 0);
                end = desc.IndexOf(">", start + 1);
                var str = desc.Substring(start, end - start + 1);
                desc = desc.Replace(str, "");
            }

            //_description = "<html><body>" + _description + "</body></html>";
            // remove html tags (won't be necessary once we use a webview
            desc = desc.Replace("<b>", "").Replace("</b>", "")
                       .Replace("<p>", "\n").Replace("</p>", "").Replace("</iframe>", "")
                       .Replace("&iexcl;", "¡")
                       .Replace("&Agrave;", "À").Replace("&Aacute;", "Á")
                       .Replace("&Egrave;", "È").Replace("&Eacute;", "É")
                       .Replace("&Igrave;", "Ì").Replace("&Iacute;", "Í")
                       .Replace("&Ograve;", "Ò").Replace("&Oacute;", "Ó")
                       .Replace("&Ugrave;", "Ù").Replace("&Uacute;", "Ú")
                       .Replace("&agrave;", "à").Replace("&aacute;", "á")
                       .Replace("&egrave;", "è").Replace("&eacute;", "é")
                       .Replace("&igrave;", "ì").Replace("&iacute;", "í")
                       .Replace("&ograve;", "ò").Replace("&oacute;", "ó")
                       .Replace("&ugrave;", "ù").Replace("&uacute;", "ú")
                       .Replace("&aring;", "å").Replace("&oslash;", "ø")
                       .Replace("&auml;", "ä").Replace("&ouml;", "ö")
                       .Replace("&nbsp;", " ").Replace("&#39;", "'")
                       .Replace("&pound;", "£").Replace("&quot;","\"")
                       .Replace("<div>", "").Replace("</div>", "")
                       .Replace("<img>", "").Replace("</img>", "")
                       .Replace("<span>", "").Replace("</span>", "")
                       .Replace("<a>", "").Replace("</a>", "")
                       .Replace("<br />", "\n").Replace("</br>", "\n");

            return desc;
        }
        private void GetImageAndNewsBody()
        {
            int start = -1, end = -1;
            start = Link.IndexOf("_") + 1;
            end = Link.IndexOf(".htm");
            if (start > 0 && end > 0 && end > start)
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ItemID = Link.Substring(start, end - (start));
                });
            }

            if (!String.IsNullOrEmpty(Link) && !Link.StartsWith("http://muse.mu/tour-dates") && !Link.StartsWith("http://muse.mu/images"))
            {
                System.Net.WebClient wc = new System.Net.WebClient();

                wc.DownloadStringCompleted += wc_DownloadStringCompleted;
                wc.DownloadStringAsync(new Uri(Link));
            }
        }

        void wc_DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            //If an error occured during download exit 
            if (e.Error != null || String.IsNullOrEmpty(e.Result))
            {
                return;
            }

            string html = e.Result;
            int index = -1;
            int start = -1;
            int end = -1;

            if (string.IsNullOrEmpty(html)) return;

            index = html.IndexOf("og:image");
            if (index > 0)
            {
                start = html.Substring(index).IndexOf("content=") + 8;
                end = html.Substring(index).IndexOf("\" />");
                string url = html.Substring(index + start + 1, end - (start + 1));
                if (url.Contains("thumb")) url = url.Replace("thumb", "square");
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ImageURL = url;
                });
                index = end = start = -1;
            }

            index = html.IndexOf("<div class=\"newsBody\">");
            if (index > 0)
            {
                start = index + 22;
                end = html.Substring(index + 22).IndexOf("</div>");
                string desc = html.Substring(start, end);
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    NewsBody = desc;
                    NewsBody = GetDescription(_newsBody);
                });
                index = end = start = -1;
            }
            html = null;
        }

        public void NotifyAll()
        {
            NotifyPropertyChanged("TourDate");
            NotifyPropertyChanged("Day");
            NotifyPropertyChanged("MonthYear");
            NotifyPropertyChanged("ImageThumb");
            NotifyPropertyChanged("ImageURL");
            NotifyPropertyChanged("NewsBody");
            NotifyPropertyChanged("ItemID");
            NotifyPropertyChanged("Title");
            NotifyPropertyChanged("Description");
            NotifyPropertyChanged("Link");
            NotifyPropertyChanged("PubDate");
        }
    }
}
