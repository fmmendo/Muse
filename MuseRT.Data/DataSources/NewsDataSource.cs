﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseRT.Data.DataSources
{
    public class NewsDataSource : DataSourceBase<RssSchema>
    {
        private const string _url = @"http://feeds2.feedburner.com/MarksDailyApple/";

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
                return await rssDataProvider.Load();
            }
            catch (Exception ex)
            {
                Logger.WriteError("NewsDataSourceDataSource.LoadData", ex.ToString());
                return new RssSchema[0];
            }
        }
    }
}