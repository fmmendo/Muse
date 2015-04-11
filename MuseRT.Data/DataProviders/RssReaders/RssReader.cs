using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Web.Http;

namespace MuseRT.Data
{
    /// <summary>
    /// Rss reader implementation to parse Rss content.
    /// </summary>
    internal class RssReader : BaseRssReader
    {
        private readonly XNamespace NsRdfNamespaceUri = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
        private readonly XNamespace NsRdfElementsNamespaceUri = "http://purl.org/dc/elements/1.1/";

        /// <summary>
        /// This override load and parses the document and return a list of RssSchema values.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public override ObservableCollection<RssSchema> LoadFeed(XDocument doc)
        {
            bool isRDF = false;
            var feed = new ObservableCollection<RssSchema>();
            XNamespace defaultNamespace = string.Empty;

            if (doc.Root != null)
            {
                isRDF = doc.Root.Name == (NsRdfNamespaceUri + "RDF");
                defaultNamespace = doc.Root.GetDefaultNamespace();
            }

            foreach (var item in doc.Descendants(defaultNamespace + "item"))
            {
                var rssItem = isRDF ? ParseRDFItem(item) : ParseRssItem(item);
                feed.Add(rssItem);
            }
            return feed;
        }

        /// <summary>
        /// RSS all versions
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static RssSchema ParseItem(XElement item)
        {
            var rssItem = new RssSchema();
            rssItem.Title = item.GetSafeElementString("title").Trim();
            rssItem.FeedUrl = item.GetSafeElementString("link");

            string description = item.GetSafeElementString("description");
            if (string.IsNullOrEmpty(description))
                description = item.GetSafeElementString("content");

            rssItem.Summary = System.Net.WebUtility.HtmlDecode(description).Trim().Truncate(500, true);
            rssItem.Summary = RssHelper.SanitizeString(rssItem.Summary);
            rssItem.Content = RssHelper.SanitizeString(description);

            string id = item.GetSafeElementString("guid").Trim();
            if (string.IsNullOrEmpty(id))
            {
                id = item.GetSafeElementString("id").Trim();
                if (string.IsNullOrEmpty(id))
                {
                    id = rssItem.FeedUrl;
                }
            }
            rssItem.Id = id;

            return rssItem;
        }

        /// <summary>
        /// RSS version 1.0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private RssSchema ParseRDFItem(XElement item)
        {
            XNamespace ns = "http://search.yahoo.com/mrss/";
            var rssItem = ParseItem(item);

            rssItem.PublishDate = item.GetSafeElementDate("date", NsRdfElementsNamespaceUri);

            string image = item.GetSafeElementString("image");
            if (string.IsNullOrEmpty(image))
            {
                image = item.GetImage();
            }
            if (string.IsNullOrEmpty(image) && item.Elements(ns + "thumbnail").LastOrDefault() != null)
            {

                var element = item.Elements(ns + "thumbnail").Last();
                image = element.Attribute("url").Value;
            }
            if (string.IsNullOrEmpty(image) && item.ToString().Contains("thumbnail"))
            {
                image = item.GetSafeElementString("thumbnail");
            }

            rssItem.ImageUrl = image;

            return rssItem;
        }

        /// <summary>
        /// RSS version 2.0
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static RssSchema ParseRssItem(XElement item)
        {
            XNamespace ns = "http://search.yahoo.com/mrss/";
            var rssItem = ParseItem(item);

            rssItem.PublishDate = item.GetSafeElementDate("pubDate");

            string image = item.GetSafeElementString("image");
            if (string.IsNullOrEmpty(image))
            {
                image = item.GetImageFromEnclosure();
            }
            if (string.IsNullOrEmpty(image))
            {
                image = item.GetImage();
            }
            if (string.IsNullOrEmpty(image) && item.Elements(ns + "thumbnail").LastOrDefault() != null)
            {
                var element = item.Elements(ns + "thumbnail").Last();
                image = element.Attribute("url").Value;
            }
            if (string.IsNullOrEmpty(image) && item.ToString().Contains("thumbnail"))
            {
                image = item.GetSafeElementString("thumbnail");
            }

            rssItem.ImageUrl = image;

            rssItem = GetImageAndNewsBody(rssItem).Result;

            return rssItem;
        }

        private static async Task<RssSchema> GetImageAndNewsBody(RssSchema item)
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
                    return item;
                }

                int index = -1;
                start = -1;
                end = -1;

                if (string.IsNullOrEmpty(html)) return item;

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
                    //NewsBody = GetDescription(_newsBody);

                    index = end = start = -1;
                }
                html = null;
            }

            return item;
        }
    }
}
