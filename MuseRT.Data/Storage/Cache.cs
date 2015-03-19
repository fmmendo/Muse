using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseRT.Data
{
    public class Cache
    {
        private static Dictionary<string, string> _memoryCache = new Dictionary<string, string>();

        public static async Task<DataSourceContent<T>> GetItemsAsync<T>(string key) where T : BindableSchemaBase
        {
            string json = null;
            if (_memoryCache.ContainsKey(key))
            {
                json = _memoryCache[key];
            }
            else
            {
                json = await UserStorage.ReadTextFromFile(key);
                _memoryCache[key] = json;
            }
            if (!String.IsNullOrEmpty(json))
            {
                try
                {
                    DataSourceContent<T> records = JsonConvert.DeserializeObject<DataSourceContent<T>>(json);
                    return records;
                }
                catch (Exception ex)
                {
                    Logger.WriteError("AppCache.GetItems", ex);
                }
            }
            return null;
        }

        public static async Task AddItemsAsync<T>(string key, DataSourceContent<T> data) where T : BindableSchemaBase
        {
            try
            {
                string json = JsonConvert.SerializeObject(data);
                await UserStorage.WriteText(key, json);
                if (_memoryCache.ContainsKey(key))
                {
                    _memoryCache[key] = json;
                }
                else
                {
                    _memoryCache.Add(key, json);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError("AppCache.AddItems", ex);
            }
        }
    }
}
