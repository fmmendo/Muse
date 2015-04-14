using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MuseRT.Data.DataSources
{
    public class GalleryDataSource : DataSourceBase<RssSchema>
    {
        private const string _url = @"http://muse.mu/rss/photos/";

        protected override string CacheKey
        {
            get { return "GalleryDataSource"; }
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
                return await rssDataProvider.Load();
            }
            catch (Exception ex)
            {
                Logger.WriteError("GalleryDataSourceDataSource.LoadData", ex.ToString());
                return new RssSchema[0];
            }
        }
    }
}
