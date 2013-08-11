using System;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;

namespace Muse
{
    //public class ItemViewModel : INotifyPropertyChanged
    //{
    //    #region RSS fields
    //    private string _title;
    //    /// <summary>
    //    /// Title, holds the title for the RSS feed item
    //    /// </summary>
    //    /// <returns></returns>
    //    public string Title
    //    {
    //        get
    //        {
    //            return _title;
    //        }
    //        set
    //        {
    //            if (value != _title)
    //            {
    //                _title = value;

    //                // Should this be a Tour Date item, the Date will precede the title
    //                // so we must remove the date part.
    //                DateTime date;
    //                if (_title.Length > 10 && DateTime.TryParse(_title.Substring(0, 10), out date))
    //                {
    //                    TourDate = date;
    //                    _title = _title.Remove(0, 13);
    //                }

    //                NotifyPropertyChanged("Title");
    //            }
    //        }
    //    }

    //    private string _description;
    //    /// <summary>
    //    /// Description, holds the content of the RSS feed item
    //    /// </summary>
    //    /// <returns></returns>
    //    public string Description
    //    {
    //        get
    //        {
    //            return _description;
    //        }
    //        set
    //        {
    //            if (value != _description)
    //            {
    //                _description = GetDescription(value);

    //                NotifyPropertyChanged("Description");
    //            }
    //        }
    //    }

    //    private string GetDescription(string value)
    //    {
    //        var desc = value;

    //        if (String.IsNullOrEmpty(MonthYear))
    //        {
    //            int start = -1;
    //            int end = -1;

    //            start = desc.IndexOf("<b>On:</b>");
    //            end = desc.IndexOf("<br />");

    //            if (start >= 0 && end > 0 && end > start)
    //            {
    //                string date = desc.Substring(start + 10, end - (start + 10)).Trim();
    //                string[] split = date.Split(new char[] { ' ' });
    //                TourDate = DateTime.Parse(String.Format("{0} {1} {2}", split[0], split[1], split[2]));
    //            }
    //        }

    //        // remove the ImageURL from the description
    //        if (String.IsNullOrEmpty(ImageURL))
    //        {
    //            int startImage = -1;
    //            int endImage = -1;
    //            int endtag = -1;

    //            startImage = desc.IndexOf("<img src=\"", 0);
    //            if (startImage >= 0)
    //            {
    //                startImage += 10;
    //                endImage = desc.IndexOf("\" ", startImage);
    //                if (endImage >= startImage)
    //                {
    //                    endtag = desc.IndexOf(">", endImage);
    //                    if (endtag >= endImage)
    //                    {
    //                        ImageThumb = desc.Substring(startImage, endImage - startImage);
    //                        ImageURL = ImageThumb.Replace("thumb", "original");
    //                        desc = desc.Substring(endtag + 1);
    //                    }
    //                }
    //            }
    //        }

    //        while (desc.Contains("<a href"))
    //        {
    //            int start = -1;
    //            int end = -1;

    //            start = desc.IndexOf("<a href", 0);
    //            end = desc.IndexOf(">", start);
    //            var str = desc.Substring(start, end - start + 1);
    //            desc = desc.Replace(str, "");
    //        }
    //        while (desc.Contains("<p style"))
    //        {
    //            int start = -1;
    //            int end = -1;

    //            start = desc.IndexOf("<p style", 0);
    //            end = desc.IndexOf(">", start + 1);
    //            var str = desc.Substring(start, end - start + 1);
    //            desc = desc.Replace(str, "");
    //        }
    //        while (desc.Contains("<span style"))
    //        {
    //            int start = -1;
    //            int end = -1;

    //            start = desc.IndexOf("<span style", 0);
    //            end = desc.IndexOf(">", start + 1);
    //            var str = desc.Substring(start, end - start + 1);
    //            desc = desc.Replace(str, "");
    //        }

    //        //_description = "<html><body>" + _description + "</body></html>";
    //        // remove html tags (won't be necessary once we use a webview
    //        desc = desc.Replace("<b>", "<bold>").Replace("</b>", "</bold>")
    //                   .Replace("<p>", "\n").Replace("</p>", "")
    //                   .Replace("&nbsp;", " ").Replace("&#39;", "'").Replace("&pound;", "£")
    //                   .Replace("<div>", "").Replace("</div>", "")
    //                   .Replace("<span>", "").Replace("</span>", "")
    //                   .Replace("<br />", "\n").Replace("</br>", "\n");

    //        return desc;
    //    }

    //    private string _link;
    //    /// <summary>
    //    /// Link, URL to the RSS item on the muse.mu website
    //    /// </summary>
    //    /// <returns></returns>
    //    public string Link
    //    {
    //        get
    //        {
    //            return _link;
    //        }
    //        set
    //        {
    //            if (value != _link)
    //            {
    //                _link = value;
    //                NotifyPropertyChanged("Link");
    //            }
    //        }
    //    }

    //    private string _pubDate;
    //    /// <summary>
    //    /// PubDate, Date in which the item was published
    //    /// </summary>
    //    /// <returns></returns>
    //    public string PubDate
    //    {
    //        get
    //        {
    //            return _pubDate;
    //        }
    //        set
    //        {
    //            if (value != _pubDate)
    //            {
    //                _pubDate = value;
    //                NotifyPropertyChanged("PubDate");
    //            }
    //        }
    //    }
    //    #endregion

    //    #region Data Related to Muse
    //    private string _itemID;
    //    /// <summary>
    //    /// Number that identifies an item.
    //    /// </summary>
    //    /// <returns></returns>
    //    public string ItemID
    //    {
    //        get
    //        {
    //            return _itemID;
    //        }
    //        set
    //        {
    //            if (value != _itemID)
    //            {
    //                _itemID = value;
    //                NotifyPropertyChanged("ItemID");
    //            }
    //        }
    //    }

    //    private string _imageURL;
    //    /// <summary>
    //    /// Image, url to the image that accompanies the news item.
    //    /// </summary>
    //    /// <returns></returns>
    //    public string ImageURL
    //    {
    //        get
    //        {
    //            return _imageURL;
    //        }
    //        set
    //        {
    //            if (value != _imageURL)
    //            {
    //                _imageURL = value;
    //                NotifyPropertyChanged("ImageURL");
    //            }
    //        }
    //    }

    //    private string _imageThumb;
    //    /// <summary>
    //    /// Image, url to the image that accompanies the news item.
    //    /// </summary>
    //    /// <returns></returns>
    //    public string ImageThumb
    //    {
    //        get
    //        {
    //            return _imageThumb;
    //        }
    //        set
    //        {
    //            if (value != _imageThumb)
    //            {
    //                _imageThumb = value;
    //                NotifyPropertyChanged("ImageThumb");
    //            }
    //        }
    //    }

    //    private string _monthYear;
    //    /// <summary>
    //    /// MonthYear, A string that has the month and year of a gig
    //    /// </summary>
    //    /// <returns></returns>
    //    public string MonthYear
    //    {
    //        get
    //        {
    //            return _monthYear;
    //        }
    //        set
    //        {
    //            if (value != _monthYear)
    //            {
    //                _monthYear = value;
    //                NotifyPropertyChanged("MonthYear");
    //            }
    //        }
    //    }

    //    private int _day;
    //    /// <summary>
    //    /// Day of the gig
    //    /// </summary>
    //    /// <returns></returns>
    //    public int Day
    //    {
    //        get
    //        {
    //            return _day;
    //        }
    //        set
    //        {
    //            if (value != _day)
    //            {
    //                _day = value;
    //                NotifyPropertyChanged("Day");
    //            }
    //        }
    //    }

    //    private DateTime _tourDate;
    //    /// <summary>
    //    /// TourDate, holds the date for a gig.
    //    /// </summary>
    //    /// <returns></returns>
    //    public DateTime TourDate
    //    {
    //        get
    //        {
    //            return _tourDate;
    //        }
    //        set
    //        {
    //            if (value != _tourDate)
    //            {
    //                _tourDate = value;
    //                NotifyPropertyChanged("TourDate");

    //                Day = _tourDate.Day;
    //                MonthYear = _tourDate.ToString("MMM yyyy");
    //            }
    //        }
    //    }
    //    #endregion

    //    //#region Save to Isolated Storage
    //    ///// <summary>
    //    ///// Isolated storage filename for this Item
    //    ///// </summary>
    //    //public String Filename;

    //    ///// <summary>
    //    ///// Save an entry
    //    ///// </summary>
    //    //public void SaveContent()
    //    //{
    //    //    using (IsolatedStorageFile userStore = IsolatedStorageFile.GetUserStoreForApplication())
    //    //    {
    //    //        using (IsolatedStorageFileStream stream = userStore.CreateFile(Filename))
    //    //        {
    //    //            using (StreamWriter writer = new StreamWriter(stream))
    //    //            {
    //    //                writer.WriteLine(Title);
    //    //                writer.WriteLine(Description);
    //    //                writer.WriteLine(Link);
    //    //                writer.WriteLine(PubDate);

    //    //                writer.WriteLine(ImageURL);
    //    //                writer.WriteLine(MonthYear);
    //    //                writer.WriteLine(Day);
    //    //                writer.WriteLine(TourDate);
    //    //            }
    //    //        }
    //    //    }
    //    //}

    //    ///// <summary>
    //    ///// Load an entry
    //    ///// </summary>
    //    //public void LoadContent()
    //    //{
    //    //    using (IsolatedStorageFile userStore = IsolatedStorageFile.GetUserStoreForApplication())
    //    //    {
    //    //        if (!userStore.FileExists(Filename)) return;

    //    //        using (IsolatedStorageFileStream stream = userStore.CreateFile(Filename))
    //    //        {
    //    //            using (StreamReader reader = new StreamReader(stream))
    //    //            {
    //    //                // Temporary variables
    //    //                string day;
    //    //                string date;
    //    //                int _day;
    //    //                DateTime _date;

    //    //                // Read data from storage
    //    //                Title = reader.ReadLine();
    //    //                Description = reader.ReadLine();
    //    //                Link = reader.ReadLine();
    //    //                PubDate = reader.ReadLine();
    //    //                ImageURL = reader.ReadLine();
    //    //                MonthYear = reader.ReadLine();
    //    //                day = reader.ReadLine();
    //    //                date = reader.ReadLine();

    //    //                // Parse the tour date
    //    //                Int32.TryParse(day, out _day);
    //    //                DateTime.TryParse(date, out _date);

    //    //                Day = _day;
    //    //                TourDate = _date;
    //    //            }
    //    //        }
    //    //    }
    //    //}

    //    ///// <summary>
    //    ///// Delete an entry
    //    ///// </summary>
    //    //public void DeleteContent()
    //    //{
    //    //    using (IsolatedStorageFile userStore = IsolatedStorageFile.GetUserStoreForApplication())
    //    //    {
    //    //        userStore.DeleteFile(Filename);
    //    //    }
    //    //}
    //    //#endregion

    //    public ItemViewModel(string title, string link, string description, string pubdate)
    //    {
    //        ClearAll();

    //        /* Deal with the RSS bit */
    //        Title = title;              //also extracts date from title, for a tour item
    //        Link = link;                // ...
    //        Description = description;  // for compatibility's sake, attempts to remove the ImageURL
    //        PubDate = pubdate;          // ...

    //        /* Prepare some muse specific data */
    //        bw = new BackgroundWorker();
    //        bw.DoWork += bw_DoWork;
    //        bw.RunWorkerAsync();
    //    }

    //    void bw_DoWork(object sender, DoWorkEventArgs e)
    //    {
    //        int start = -1, end = -1;
    //        start = Link.IndexOf("_") + 1;
    //        end = Link.IndexOf(".htm");
    //        if (start > 0 && end > 0 && end > start)
    //        {
    //            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
    //            {
    //                ItemID = Link.Substring(start, end - (start));
    //            });
    //        }

    //        if (!String.IsNullOrEmpty(Link) && !Link.StartsWith("http://muse.mu/tour-dates") && !Link.StartsWith("http://muse.mu/images"))
    //        {
    //            System.Net.WebClient wc = new System.Net.WebClient();

    //            wc.DownloadStringCompleted += wc_DownloadStringCompleted;
    //            wc.DownloadStringAsync(new Uri(Link));
    //        }
    //    }

    //    void wc_DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
    //    {
    //        //If an error occured during download exit 
    //        if (e.Error != null || String.IsNullOrEmpty(e.Result))
    //        {
    //            return;
    //        }

    //        string html = e.Result;
    //        int index = -1;
    //        int start = -1;
    //        int end = -1;

    //        if (string.IsNullOrEmpty(html)) return;

    //        index = html.IndexOf("og:image");
    //        if (index > 0)
    //        {
    //            start = html.Substring(index).IndexOf("content=") + 8;
    //            end = html.Substring(index).IndexOf("\" />");
    //            string url = html.Substring(index + start + 1, end - (start + 1));
    //            if (url.Contains("thumb")) url = url.Replace("thumb", "square");
    //            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
    //            {
    //                ImageURL = url;
    //            });
    //            index = end = start = -1;
    //        }

    //        index = html.IndexOf("<div class=\"newsBody\">");
    //        if (index > 0)
    //        {
    //            start = index + 22;
    //            end = html.Substring(index + 22).IndexOf("</div>");
    //            string desc = html.Substring(start, end);
    //            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
    //            {
    //                Description = desc;
    //            });
    //            index = end = start = -1;
    //        }
    //        html = null;
    //    }

    //    /// <summary>
    //    /// Clear all data
    //    /// </summary>
    //    private void ClearAll()
    //    {
    //        _title = "";
    //        _link = "";
    //        _description = "";
    //        _pubDate = "";

    //        _itemID = "";
    //        _imageURL = "";
    //        _monthYear = "";
    //        _day = 0;
    //        _tourDate = new DateTime();

    //    }



    //    public BackgroundWorker bw { get; set; }
    //}
}