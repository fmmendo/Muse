using System;
using System.Collections;

namespace MuseRT.Data
{
    public class InstagramSchema : BindableSchemaBase, IEquatable<InstagramSchema>, IComparable<InstagramSchema>
    {
        private string _id;
        private string _title;
        private string _imageUrl;
        private string _thumbnailUrl;
        private string _sourceUrl;
        private string _author;
        private DateTime _publishDate;

        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set { SetProperty(ref _imageUrl, value); }
        }

        public string ThumbnailUrl
        {
            get { return _thumbnailUrl; }
            set { SetProperty(ref _thumbnailUrl, value); }
        }

        public string SourceUrl
        {
            get { return _sourceUrl; }
            set { SetProperty(ref _sourceUrl, value); }
        }

        public DateTime Published
        {
            get { return _publishDate; }
            set { SetProperty(ref _publishDate, value); }
        }

        public string Author
        {
            get { return _author; }
            set { SetProperty(ref _author, value); }
        }

        public override string DefaultTitle
        {
            get { return Title; }
        }

        public override string DefaultSummary
        {
            get { return Author; }
        }

        public override string DefaultImageUrl
        {
            get { return ImageUrl; }
        }

        public override string DefaultContent
        {
            get { return Author; }
        }

        public override string GetValue(string propertyName)
        {
            if (!String.IsNullOrEmpty(propertyName))
            {
                switch (propertyName.ToLower())
                {
                    case "title":
                        return String.Format("{0}", Title);
                    case "imageurl":
                        return String.Format("{0}", ImageUrl);
                    case "thumbnailUrl":
                        return String.Format("{0}", ThumbnailUrl);
                    case "sourceUrl":
                        return String.Format("{0}", SourceUrl);
                    case "published":
                        return String.Format("{0}", Published);
                    case "link":
                        return String.Format("{0}", SourceUrl);
                    case "author":
                        return String.Format("{0}", Author);
                    case "defaulttitle":
                        return String.Format("{0}", DefaultTitle);
                    case "defaultsummary":
                        return String.Format("{0}", DefaultSummary);
                    case "defaultimageurl":
                        return String.Format("{0}", DefaultImageUrl);
                    default:
                        break;
                }
            }
            return String.Empty;
        }

        public bool Equals(InstagramSchema other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            return this.SourceUrl == other.SourceUrl;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as InstagramSchema);
        }

        public override int GetHashCode()
        {
            return this.SourceUrl == null ? 0 : this.SourceUrl.GetHashCode();
        }

        public int CompareTo(InstagramSchema other)
        {
            return -1 * this.Published.CompareTo(other.Published);
        }
    }
}
