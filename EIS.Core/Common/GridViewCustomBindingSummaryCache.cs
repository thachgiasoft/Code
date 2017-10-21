
using System.Collections.Generic;
using System.Web;

namespace EIS.Core.Common
{
   public class GridViewCustomBindingSummaryCache
    {
        const string CacheKey = "B08E5DF5-4D10-45C7-B4F1-C95EB2FE69C8";
        static HttpContext Context { get { return HttpContext.Current; } }
        static Dictionary<string, int> Cache
        {
            get
            {
                if (Context.Items[CacheKey] == null)
                    Context.Items[CacheKey] = new Dictionary<string, int>();
                return (Dictionary<string, int>)Context.Items[CacheKey];
            }
        }
        public static bool TryGetCount(string key, out int count)
        {
            count = 0;
            if (!Cache.ContainsKey(key))
                return false;
            count = Cache[key];
            return true;
        }
        public static void SaveCount(string key, int count)
        {
            Cache[key] = count;
        }
    }
}
