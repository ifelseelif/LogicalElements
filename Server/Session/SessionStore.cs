using System.Collections.Generic;
using Server.Models;

namespace Server.Session
{
    public class SessionStore : ISessionStore
    {
        private readonly Dictionary<string, List<Element>> _cache = new();

        public List<Element> GetCachedList(string id)
        {
            return !_cache.ContainsKey(id) ? new List<Element>() : _cache[id];
        }

        public void SetCachedList(string id, List<Element> list)
        {
            _cache[id] = list;
        }
    }
}