using System;
using System.Text;

namespace MuseRT.Data
{
    public abstract class BindableSchemaBase : BindableBase
    {
        protected BindableSchemaBase();

        public abstract string DefaultContent { get; }
        public abstract string DefaultImageUrl { get; }
        public abstract string DefaultSummary { get; }
        public abstract string DefaultTitle { get; }

        public abstract string GetValue(string propertyName);
        public virtual string GetValues(params string[] propertyNames)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var propertyName in propertyNames)
            {
                object value = GetValue(propertyName) ?? String.Empty;
                sb.AppendLine(value.ToString());
            }
            return sb.ToString();
        }

    }
}
