using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models.domain;
using Element = Server.Db.models.Element;

namespace Server.Db.Repository.Interface
{
    public interface IElementRepository
    {
        Task<List<Element>> GetAllElementsByUserId(Guid userId);
        Task<Element> AddElement(LogicalElement elem, Guid userId);
        Task<Element> AddElement(ValueElement element, Guid userId);
        Task<string> SetValueForElement(string name, bool value, Guid userId);
        Task<int> GetMaxId();
    }
}