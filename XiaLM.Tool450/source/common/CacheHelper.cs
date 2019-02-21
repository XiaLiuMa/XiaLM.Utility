using System;
using System.Runtime.Caching;

namespace XiaLM.Tool450.source.common
{
    /// <summary>
    /// 缓存助手
    /// </summary>
    public class CacheHelper
    {
        public ObjectCache cache;
        public CacheItemPolicy policy;

        /// <summary>
        /// 缓存助手
        /// </summary>
        /// <param name="name">缓存名称</param>
        /// <param name="isAbsolute">是否是绝对时间(否则是相对时间)</param>
        /// <param name="time">缓存时间</param>
        public CacheHelper(string name, bool isAbsolute, int time)
        {
            cache = new MemoryCache(name);
            if (isAbsolute)
            {
                policy = new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(time) };   //(绝对时间)3逐出
            }
            else
            {
                policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromSeconds(time) };   //(相对时间)逐出
            }
            policy.RemovedCallback += CacheEntryRemovedCallback;
        }

        /// <summary>
        /// 移除后处理函数
        /// </summary>
        /// <param name="arguments"></param>
        private void CacheEntryRemovedCallback(CacheEntryRemovedArguments arguments)
        {

        }

        public void Test()
        {
            string cheKey = "";
            var che = cache.Get(cheKey);
            if (che == null) cache.Add(cheKey, Guid.NewGuid(), policy);
        }

    }
}
