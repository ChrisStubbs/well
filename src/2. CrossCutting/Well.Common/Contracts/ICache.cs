namespace PH.Well.Common.Contracts
{
    public interface ICache
    {
        T Get<T>(string key, string username) where T : class;

        void AddItem(string key, string username, object objectToCache, int minutesToCacheFor, bool removeIfExists = false);

        void RemoveItem(string key, string username);
    }
}
