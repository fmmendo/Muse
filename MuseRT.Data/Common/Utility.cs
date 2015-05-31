using System;

namespace MuseRT.Data
{
    static public class Utility
    {
        public static bool EqualNoCase(this string value, string content)
        {
            if (value != null)
            {
                return value.Equals(content, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return false;
            }
        }

        public static string Truncate(this String str, int length, bool ellipsis = false)
        {
            if (!String.IsNullOrEmpty(str))
            {
                str = str.Trim();
                if (str.Length > length)
                {
                    if (ellipsis)
                    {
                        return str.Substring(0, length) + "...";
                    }
                    else
                    {
                        return str.Substring(0, length);
                    }
                }
            }
            return str ?? String.Empty;
        }
    }
}
