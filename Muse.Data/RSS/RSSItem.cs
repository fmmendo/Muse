using System;
using System.ComponentModel;

namespace Muse.Data.RSS
{
    public class RSSItem : INotifyPropertyChanged
    {
        #region Properties
        private string _title;
        /// <summary>
        /// Title, holds the title for the RSS feed item
        /// </summary>
        /// <returns></returns>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        private string _description;
        /// <summary>
        /// Description, holds the content of the RSS feed item
        /// </summary>
        /// <returns></returns>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        private string _link;
        /// <summary>
        /// Link, URL to the RSS item on the muse.mu website
        /// </summary>
        /// <returns></returns>
        public string Link
        {
            get
            {
                return _link;
            }
            set
            {
                if (value != _link)
                {
                    _link = value;
                    NotifyPropertyChanged("Link");
                }
            }
        }

        private string _pubDate;
        /// <summary>
        /// PubDate, Date in which the item was published
        /// </summary>
        /// <returns></returns>
        public string PubDate
        {
            get
            {
                return _pubDate;
            }
            set
            {
                if (value != _pubDate)
                {
                    _pubDate = value;
                    NotifyPropertyChanged("PubDate");
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// INotifyPropertyChanged implementation.
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        protected void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
