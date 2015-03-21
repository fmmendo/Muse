using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace MuseRT.Data
{
    /// <summary>
    /// Type of Rss.
    /// </summary>
    public enum RssType
    {
        Atom,
        Rss,
        Unknown
    }

    internal abstract class BaseRssReader
    {
        // fmmd
        //private static readonly XNamespace NsMedia = "http://search.yahoo.com/mrss/";
        //private static readonly XNamespace NsItunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";

        /// <summary>
        /// Get the feed type: Rss, Atom or Unknown
        /// </summary>
        /// <param name="rss"></param>
        /// <returns></returns>
        public static RssType GetFeedType(XDocument doc)
        {
            if (doc.Root == null)
            {
                Logger.WriteError("AtomReader.LoadFeed", "Not supported type");
                return RssType.Unknown;
            }
            XNamespace defaultNamespace = doc.Root.GetDefaultNamespace();
            return defaultNamespace.NamespaceName.EndsWith("Atom") ? RssType.Atom : RssType.Rss;
        }

        /// <summary>
        /// Abstract method to be override by specific implementations of the reader.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public abstract ObservableCollection<RssSchema> LoadFeed(XDocument doc);
    }
}
