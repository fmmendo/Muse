using System;
using System.Collections;

namespace MuseRT.Data
{
    /// <summary>
    /// Implementation of the YouTubeSchema class.
    /// </summary>
    public class YouTubeSchema : BindableSchemaBase, IEquatable<YouTubeSchema>, IComparable<YouTubeSchema>
    {
        private const string YoutubeWatchBaseUrl = "http://www.youtube.com/watch?v=";
        private const string YoutubeEmbedHtmlFragment = @"http://www.youtube.com/embed/{0}?rel=0&fs=0";

        private string _title;
        private string _summary;
        private string _videoUrl;
        private string _imageUrl;
        private string _videoId;
        private DateTime _published;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Summary
        {
            get { return _summary; }
            set { SetProperty(ref _summary, value); }
        }

        public string VideoUrl
        {
            get { return _videoUrl; }
            set { SetProperty(ref _videoUrl, value); }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set { SetProperty(ref _imageUrl, value); }
        }

        public DateTime Published
        {
            get { return _published; }
            set { SetProperty(ref _published, value); }
        }

        public string VideoId
        {
            get
            {
                return _videoId;
            }
            set { SetProperty(ref _videoId, value); }
        }

        public string ExternalUrl
        {
            get { return YoutubeWatchBaseUrl + VideoId; }
        }

        public override string DefaultTitle
        {
            get { return Title; }
        }

        public override string DefaultSummary
        {
            get { return Summary; }
        }

        public override string DefaultImageUrl
        {
            get { return ImageUrl; }
        }

        public override string DefaultContent
        {
            get { return Summary; }
        }

        public string EmbedHtmlFragment
        {
            get { return string.Format(YoutubeEmbedHtmlFragment, _videoId); }
        }

        override public string GetValue(string fieldName)
        {
            if (!String.IsNullOrEmpty(fieldName))
            {
                switch (fieldName.ToLower())
                {
                    case "title":
                        return String.Format("{0}", Title);
                    case "summary":
                        return String.Format("{0}", Summary);
                    case "videourl":
                        return String.Format("{0}", VideoUrl);
                    case "imageurl":
                        return String.Format("{0}", ImageUrl);
                    case "videoid":
                        return String.Format("{0}", VideoId);
                    case "externalurl":
                        return String.Format("{0}", ExternalUrl);
                    case "defaulttitle":
                        return String.Format("{0}", DefaultTitle);
                    case "defaultsummary":
                        return String.Format("{0}", DefaultSummary);
                    case "defaultimageurl":
                        return String.Format("{0}", DefaultImageUrl);
                    case "embedhtmlfragment":
                        return String.Format("{0}", EmbedHtmlFragment);
                    default:
                        break;
                }
            }
            return String.Empty;
        }

        public bool Equals(YouTubeSchema other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            return this.VideoId == other.VideoId;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as YouTubeSchema);
        }

        public override int GetHashCode()
        {
            return this.VideoId.GetHashCode();
        }

        public int CompareTo(YouTubeSchema other)
        {
            return -1 * this.Published.CompareTo(other.Published);
        }
    }
}
