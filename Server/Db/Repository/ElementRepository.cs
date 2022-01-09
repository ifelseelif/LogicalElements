using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.EntityFrameworkCore;
using Server.Db.Repository.Interface;
using Server.Models.domain;
using Element = Server.Db.models.Element;

namespace Server.Db.Repository
{
    public class ElementRepository : IElementRepository
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        public ElementRepository(IDbContextFactory<ApplicationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public Task<List<Element>> GetAllElementsByUserId(Guid userId)
        {
            var applicationContext = _contextFactory.CreateDbContext();
            return applicationContext.Elements
                .Where(elem => elem.UserId == userId)
                .ToListAsync();
        }

        public async Task<Element> AddElement(LogicalElement elem, Guid userId)
        {
            var element = new Element
            {
                ElemType = elem.ElemType,
                UserId = userId,
            };

            await AddElement(element);
            return element;
        }

        public async Task<Element> AddElement(ValueElement elem, Guid userId)
        {
            var element = new Element
            {
                ElemType = ElemType.value,
                UserId = userId,
                IsInput = elem.IsInput,
                Value = elem.Value,
                Name = elem.Name,

            };

            await AddElement(element);
            return element;
        }

        public async Task<string> SetValueForElement(string name, bool value, Guid userId)
        {
            var applicationContext = await _contextFactory.CreateDbContextAsync();
            var elem = await applicationContext.Elements.FirstOrDefaultAsync(elem => elem.Name == name);
            if (elem == null) return "not found";
            applicationContext.Elements.Attach(elem);
            elem.Value = value;
            applicationContext.Update(elem);

            await applicationContext.SaveChangesAsync();
            return "updated";
        }

        public Task<int> GetMaxId()
        {
            var applicationContext = _contextFactory.CreateDbContext();
            return applicationContext.Elements.CountAsync();
        }

        private async Task AddElement(Element element)
        {
            var applicationContext = await _contextFactory.CreateDbContextAsync();
            await applicationContext.Elements.AddAsync(element);
            await applicationContext.SaveChangesAsync();
        }
    }
}