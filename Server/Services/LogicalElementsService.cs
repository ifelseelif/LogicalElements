using System;
using System.Text;
using Common;
using Server.Models;
using Server.Session;

namespace Server.Services
{
    public class LogicalElementsService : ILogicalElementsService
    {
        private readonly ISessionStore _sessionStore;

        public LogicalElementsService(ISessionStore sessionStore)
        {
            _sessionStore = sessionStore;
        }

        public string AddElement(ElemType elemType, string connectionId)
        {
            var elements = _sessionStore.GetCachedList(connectionId);
            var id = elements.Count;
            var elem = new LogicalElement(id, elemType);
            elements.Add(elem);
            _sessionStore.SetCachedList(connectionId, elements);
            return "created " + elem;
        }

        public string SetValueForElement(string name, bool value, string connectionId)
        {
            var elements = _sessionStore.GetCachedList(connectionId);
            foreach (var element in elements)
            {
                if (element is not ValueElement valueElement || valueElement.Name != name) continue;
                valueElement.Value = value;
                return "ok";
            }

            return "not found";
        }

        public string AddIO(bool isInput, string name, string connectionId)
        {
            var elements = _sessionStore.GetCachedList(connectionId);
            var element = new ValueElement(elements.Count, name, isInput);
            elements.Add(element);
            _sessionStore.SetCachedList(connectionId, elements);
            return "created" + element;
        }

        public string AddConnection(int idOfInput, int idOfOutput, string connectionId)
        {
            var elements = _sessionStore.GetCachedList(connectionId);
            var firstElement = elements[idOfInput];
            var secondElement = elements[idOfOutput];
            secondElement.AddInput(firstElement);
            return "ok";
        }

        public string Show(int id, string connectionId)
        {
            var elements = _sessionStore.GetCachedList(connectionId);
            return id < elements.Count ? elements[id].Show() : "not found";
        }

        public string Print(string connectionId)
        {
            var elements = _sessionStore.GetCachedList(connectionId);
            var result = new StringBuilder();
            foreach (var element in elements)
            {
                if (element is not ValueElement { IsInput: false } valueElement) continue;
                try
                {
                    result.Append($"{valueElement.Name} - {valueElement.Result()}\n");
                }
                catch (Exception)
                {
                    return "Can't calculate";
                }
            }

            return result.ToString();
        }
    }
}