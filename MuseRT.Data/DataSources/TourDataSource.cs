using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseRT.Data.DataSources
{
    public class TourDataSource : DataSourceBase<RssSchema>
    {
        private const string _url = @"http://muse.mu/rss/gigs";

        protected override string CacheKey
        {
            get { return "TourDataSource"; }
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
                var touritems = await rssDataProvider.GetTourDate(items);
                //var orderedTourItems = touritems.OrderBy(i => i.TourDate);
                return touritems;//.OrderBy(i => i.TourDate);
            }
            catch (Exception ex)
            {
                Logger.WriteError("TourDataSourceDataSource.LoadData", ex.ToString());
                return new RssSchema[0];
            }
        }
    }
}
