using System;

namespace Utility.Cache
{
    internal interface ICacheService
    {
        T Get<T>(string cacheID, Func<T> getItemCallback) where T : class;
    }
}