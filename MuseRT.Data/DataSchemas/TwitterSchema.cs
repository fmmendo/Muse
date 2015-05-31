using System;
using System.Globalization;

namespace MuseRT.Data
{
    public class TwitterSchema : BindableSchemaBase, IEquatable<TwitterSchema>, IComparable<TwitterSchema>
    {
        private string _text;
        private DateTime _creationDateTime;
        private string _userId;
        private string _userName;
        private string _userScreenName;
        private string _userProfileImageUrl;
        private string _url;

        public string Id { get; set; }

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        public DateTime CreationDateTime
        {
            get { return _creationDateTime; }
            set { SetProperty(ref _creationDateTime, value); }
        }

        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public string UserScreenName
        {
            get { return _userScreenName; }
            set { SetProperty(ref _userScreenName, value); }
        }

        public string UserProfileImageUrl
        {
            get { return _userProfileImageUrl; }
            set { SetProperty(ref _userProfileImageUrl, value); }
        }

        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        public override string DefaultTitle
        {
            get { return UserName; }
        }

        public override string DefaultSummary
        {
            get { return Text; }
        }

        public override string DefaultImageUrl
        {
            get { return UserProfileImageUrl; }
        }

        public override string DefaultContent
        {
            get { return Text; }
        }

        public TwitterSchema()
        {
        }

        public TwitterSchema(TwitterTimeLineItem item)
        {
            Id = item.id_str;
            Text = item.text;
            CreationDateTime = TryParse(item.created_at);
            if (item.user == null)
            {
                UserId = string.Empty;
                UserName = string.Empty;
                UserScreenName = string.Empty;
                UserProfileImageUrl = string.Empty;
                Url = string.Empty;
            }
            else
            {
                UserId = item.user.id_str;
                UserName = item.user.name;
                UserScreenName = string.Concat("@", item.user.screen_name);
                UserProfileImageUrl = item.user.profile_image_url;
                Url = string.Format("https://twitter.com/{0}/status/{1}", item.user.screen_name, item.id_str);
                if (!string.IsNullOrEmpty(UserProfileImageUrl))
                {
                    UserProfileImageUrl = UserProfileImageUrl.Replace("_normal", string.Empty);
                }
            }
        }

        public override string GetValue(string propertyName)
        {
            if (!String.IsNullOrEmpty(propertyName))
            {
                switch (propertyName.ToLower())
                {
                    case "id":
                        return String.Format("{0}", Id);
                    case "text":
                        return String.Format("{0}", Text);
                    case "creationDateTime":
                        return String.Format("{0}", CreationDateTime);
                    case "userId":
                        return String.Format("{0}", UserId);
                    case "userName":
                        return String.Format("{0}", UserName);
                    case "userScreenName":
                        return String.Format("{0}", UserScreenName);
                    case "userProfileImageUrl":
                        return String.Format("{0}", UserProfileImageUrl);
                    case "link":
                        return String.Format("{0}", Url);
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

        public bool Equals(TwitterSchema other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            return this.Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TwitterSchema);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        private static DateTime TryParse(string dateTime)
        {
            DateTime dt;
            if (!DateTime.TryParseExact(dateTime, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                dt = DateTime.Today;
            }

            return dt;
        }

        public int CompareTo(TwitterSchema other)
        {
            return -1 * this.CreationDateTime.CompareTo(other.CreationDateTime);
        }
    }
}
