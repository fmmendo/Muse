using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseRT.Data.DataSources
{
    public class NewsDataSource : DataSourceBase<RssSchema>
    {
        private const string _url = @"http://muse.mu/rss/news";

        protected override string CacheKey
        {
            get { return "NewsDataSource"; }
        }

        public override bool HasStaticData
        {
            get { return false; }
        }

        public async override Task<IEnumerable<RssSchema>> LoadDataAsync()
        {
            try
            {
                var rssDataProvider = new RssDataProvider(_url);
                var items = await rssDataProvider.Load();
                return await rssDataProvider.LoadAdditionalMuseData(items);
            }
            catch (Exception ex)
            {
                Logger.WriteError("NewsDataSourceDataSource.LoadData", ex.ToString());
                return new RssSchema[0];
            }
        }
    }
}
