using System;
using System.Collections.Generic;

namespace MuseRT.Data
{
    public class DataSourceContent<T> where T : BindableSchemaBase
    {
        public DateTime TimeStamp { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
