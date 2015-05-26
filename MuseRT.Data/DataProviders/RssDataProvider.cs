using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MuseRT.Data
{
    /// <summary>
    /// Rss data provider class
    /// </summary>
    public class RssDataProvider
    {
        private Uri _uri;
        private string _userAgent;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="url"></param>
        public RssDataProvider(string url, string userAgent = null)
        {
            _uri = new Uri(url);
            _userAgent = userAgent;
        }

        /// <summary>
        /// Starts loading the feed and initializing the reader for the feed type.
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<RssSchema>> Load()
        {
            string xmlContent = await DownloadAsync();

            var doc = XDocument.Parse(xmlContent);
            var type = BaseRssReader.GetFeedType(doc);

            BaseRssReader rssReader;
            if (type == RssType.Rss)
                rssReader = new RssReader();
            else
                rssReader = new AtomReader();

            return rssReader.LoadFeed(doc);
        }

        private async Task<string> DownloadAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, _uri);

            if (!string.IsNullOrEmpty(_userAgent))
            {
                request.Headers.UserAgent.ParseAdd(_userAgent);
            }

            var responseMessage = await client.SendAsync(request);

            using (var stream = await responseMessage.Content.ReadAsStreamAsync())
            {
                using (var memStream = new MemoryStream())
                {
                    // Note: Some RSS feeds return gzipped data even when they are not requested to.
                    // This code checks if data is gzipped and unzip data if needed.
                    await stream.CopyToAsync(memStream);
                    byte[] buffer = memStream.ToArray();
                    memStream.Position = 0;

                    if (buffer[0] == 31 && buffer[1] == 139 && buffer[2] == 8)
                    {
                        using (var gzipStream = new GZipStream(memStream, CompressionMode.Decompress))
                        {
                            return ReadStream(gzipStream);
                        }
                    }
                    else
                    {
                        return ReadStream(memStream);
                    }
                }
            }
        }

        private string ReadStream(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public async Task<ObservableCollection<RssSchema>> LoadAdditionalMuseData(ObservableCollection<RssSchema> items)
        {
            foreach (var item in items)
            {
                int start = -1, end = -1;
                start = item.FeedUrl.IndexOf("_") + 1;
                end = item.FeedUrl.IndexOf(".htm");
                string html = string.Empty;
                if (start > 0 && end > 0 && end > start)
                {
                    var id = item.FeedUrl.Substring(start, end - (start));
                }

                if (!String.IsNullOrEmpty(item.FeedUrl) && !item.FeedUrl.StartsWith("http://muse.mu/tour-dates") && !item.FeedUrl.StartsWith("http://muse.mu/images"))
                {
                    var req = (HttpWebRequest)WebRequest.Create(item.FeedUrl);
                    var res = await req.GetResponseAsync();

                    using (var sr = new StreamReader(res.GetResponseStream()))
                    {
                        html = sr.ReadToEnd();
                    }

                    if (String.IsNullOrEmpty(html))
                    {
                        continue;
                    }

                    int index = -1;
                    start = -1;
                    end = -1;

                    if (string.IsNullOrEmpty(html)) continue;

                    index = html.IndexOf("og:image");
                    if (index > 0)
                    {
                        start = html.Substring(index).IndexOf("content=") + 8;
                        end = html.Substring(index).IndexOf("\" />");
                        string url = html.Substring(index + start + 1, end - (start + 1));
                        if (url.Contains("thumb"))
                            url = url.Replace("thumb", "original");
                        else if (url.Contains("square"))
                            url = url.Replace("square", "original");

                        item.ImageUrl = url;
                        item.ExtraImageUrl = item.ImageUrl.Replace("original", "thumb");

                        index = end = start = -1;
                    }

                    index = html.IndexOf("<div class=\"newsBody\">");
                    if (index > 0)
                    {
                        start = index + 22;
                        end = html.Substring(index + 22).IndexOf("</div>");
                        string desc = html.Substring(start, end);

                        item.Content = RssHelper.SanitizeString(desc);

                        index = end = start = -1;
                    }
                    html = null;
                }
            }

            return items;
        }

        public async Task<ObservableCollection<RssSchema>> GetTourDate(ObservableCollection<RssSchema> items)
        {
            foreach (var item in items)
            {
                int start = -1;
                int end = -1;

                start = item.Content.IndexOf("<b>On:</b>");
                end = item.Content.IndexOf("<br />");

                if (start >= 0 && end > 0 && end > start)
                {
                    string strDate = item.Content.Substring(start + 10, end - (start + 10)).Trim();
                    item.TourDate = DateTime.Parse(strDate);
                }
            }

            return items;
        }

        public async Task<ObservableCollection<RssSchema>> GetDescription(ObservableCollection<RssSchema> items)
        {

            foreach (var item in items)
            {

                var desc = item.Content;

                // remove the ImageURL from the description
                if (String.IsNullOrEmpty(item.ImageUrl))
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
                                var url = desc.Substring(startImage, endImage - startImage);
                                if (url.Contains("thumb"))
                                {
                                    item.ExtraImageUrl = url;
                                    item.ImageUrl = url.Replace("thumb", "original");
                                }
                                else if (url.Contains("square"))
                                {
                                    item.ExtraImageUrl = url.Replace("square", "thumb");
                                    item.ImageUrl = url.Replace("square", "original");
                                }
                                else
                                {
                                    item.ExtraImageUrl = url.Replace("original", "thumb");
                                    item.ImageUrl = url;
                                }
                                //_imageURL = desc.Substring(startImage, endImage - startImage);
                                //ImageThumb = _imageURL;
                                //NotifyPropertyChanged("ImageThumb");
                                //NotifyPropertyChanged("ImageURL");
                            }
                        }
                    }
                }
                else
                {
                    if (item.ImageUrl.Contains("thumb"))
                    {
                        item.ExtraImageUrl = item.ImageUrl;
                        item.ImageUrl = item.ImageUrl.Replace("thumb", "original");
                    }
                    else if (item.ImageUrl.Contains("square"))
                    {
                        item.ExtraImageUrl = item.ImageUrl.Replace("square", "thumb");
                        item.ImageUrl = item.ImageUrl.Replace("square", "original");
                    }
                }

                item.Content = desc;
            }

            return items;
        }
    }
}
