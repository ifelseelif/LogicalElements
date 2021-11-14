using System.Collections.Generic;
using Server.Models;

namespace Server.Session
{
    public interface ISessionStore
    {
        List<Element> GetCachedList(string id);
        void SetCachedList(string id, List<Element> list);
    }
}