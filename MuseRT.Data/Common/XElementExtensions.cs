using System;
using System.Globalization;
using System.Xml.Linq;

namespace MuseRT.Data
{
    public static class XElementExtensions
    {
        public static DateTime GetSafeDateValue(this XElement parent, XName elementName)
        {
            string value = parent.GetSafeValue(elementName);
            if (!string.IsNullOrEmpty(value))
            {
                DateTime dateTime = DateTime.MinValue;
                if (DateTime.TryParse(value, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowWhiteSpaces, out dateTime))
                {
                    return dateTime;
                }
            }
            return DateTime.MinValue;
        }

        public static string GetSafeValue(this XElement parent, XName elementName)
        {
            var targetChild = GetElement(parent, elementName);

            if (targetChild != null)
            {
                return targetChild.Value == null ? string.Empty : targetChild.Value.Trim();
            }
            return string.Empty;
        }

        public static string GetSafeAttribute(this XElement parent, XName elementName, string attrName)
        {
            var targetChild = GetElement(parent, elementName);

            if (targetChild != null)
            {
                var attr = targetChild.Attribute(attrName);
                if (attr != null)
                {
                    return attr.Value.Trim();
                }
            }
            return string.Empty;
        }

        private static XElement GetElement(XElement parent, XName elementName)
        {
            if (parent != null)
            {
                return parent.Element(elementName);
            }
            return null;
        }
    }
}
