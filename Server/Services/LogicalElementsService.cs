using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Server.Db.models;
using Server.Db.Repository.Interface;
using Server.Models.domain;
using Server.Services.Interfaces;
using Element = Server.Db.models.Element;
using DomainElement = Server.Models.domain.Element;

namespace Server.Services
{
    public class LogicalElementsService : ILogicalElementsService
    {
        private readonly IElementRepository _elementRepository;
        private readonly IConnectionRepository _connectionRepository;

        public LogicalElementsService(IElementRepository elementRepository, IConnectionRepository connectionRepository)
        {
            _elementRepository = elementRepository;
            _connectionRepository = connectionRepository;
        }

        public async Task<string> AddElement(ElemType elemType, Guid userId)
        {
            var elem = new LogicalElement(elemType);
            var cratedElem = await _elementRepository.AddElement(elem, userId);
            return "created " + ConvertElem(cratedElem);
        }

        public async Task<string> SetValueForElement(string name, bool value, Guid userId)
        {
            await _elementRepository.SetValueForElement(name, value, userId);
            return await _elementRepository.SetValueForElement(name, value, userId);
        }

        public async Task<string> AddIO(bool isInput, string name, Guid userId)
        {
            var element = new ValueElement(name, isInput);
            var cratedElem = await _elementRepository.AddElement(element, userId);
            return "created" + ConvertElem(cratedElem);
        }

        public async Task<string> AddConnection(int idOfInput, int idOfOutput, Guid userId)
        {
            var connection = new Connection
            {
                ElementIdIn = idOfInput,
                ElementIdOut = idOfOutput,
                UserId = userId
            };
            await _connectionRepository.AddConnection(connection);
            return "ok";
        }

        public async Task<string> Show(int elementId, Guid userId)
        {
            var elementGraph = await GetElementGraph(userId);
            var element = elementGraph.FirstOrDefault(element => element.Id == elementId);
            return element == null ? "not found" : element.Show();
        }

        public async Task<string> Print(Guid userId)
        {
            var elementGraph = await GetElementGraph(userId);
            var outputElements = elementGraph
                .Where(elem => elem.ElemType == ElemType.value)
                .Select(elem => elem as ValueElement)
                .Where(elem => elem.IsInput == false)
                .Select(elem => elem.Name + " " + elem.Result())
                .ToList();

            return string.Join(" ", outputElements);
        }

        private async Task<List<DomainElement>> GetElementGraph(Guid userId)
        {
            var elements = (await _elementRepository.GetAllElementsByUserId(userId)).Select(ConvertElem).ToList();
            var connections = await _connectionRepository.GetAllConnectionsByUserId(userId);
            connections.ForEach(connection => Connect(connection, elements));

            return elements;
        }

        private DomainElement ConvertElem(Element elem)
        {
            if (elem.ElemType == ElemType.value)
                return new ValueElement
                (
                    elem.Value,
                    elem.Name,
                    elem.IsInput
                )
                {
                    Id = elem.Id
                };
            return new LogicalElement(elem.ElemType)
            {
                Id = elem.Id
            };
        }

        private void Connect(Connection connection, IList<DomainElement> elements)
        {
            var inElement = elements.FirstOrDefault(elem => elem.Id == connection.ElementIdIn);
            var outElement = elements.FirstOrDefault(elem => elem.Id == connection.ElementIdOut);
            if (inElement == null || outElement == null) return;

            outElement.AddInput(inElement);
        }
    }
}