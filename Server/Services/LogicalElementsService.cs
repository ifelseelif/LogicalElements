using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Models;

namespace Server.Services
{
    public class LogicalElementsService : ILogicalElementsService
    {
        private List<Element> _elements;

        public LogicalElementsService()
        {
            _elements = new List<Element>();
        }

        public string AddElement(ElemType elemType)
        {
            var id = _elements.Count;
            var elem = new LogicalElement(id, elemType);
            _elements.Add(elem);
            return "created " + elem;
        }

        public string SetValueForElement(string name, bool value)
        {
            foreach (var element in _elements)
            {
                if (element is ValueElement valueElement && valueElement.Name == name)
                {
                    valueElement.Value = value;
                    return "ok";
                }
            }

            return "not found";
        }

        public string AddIO(bool isInput, string name)
        {
            var element = new ValueElement(_elements.Count, name, isInput);
            _elements.Add(element);
            return "created" + element;
        }

        public string AddConnection(int idOfInput, int idOfOutput)
        {
            var firstElement = _elements[idOfInput];
            var secondElement = _elements[idOfOutput];
            secondElement.AddInput(firstElement);
            return "ok";
        }

        public string Show(int id)
        {
            return id < _elements.Count ? _elements[id].Show() : "not found";
        }

        public string Print()
        {
            var result = new StringBuilder();
            foreach (var element in _elements)
            {
                if (element is ValueElement { IsInput: false } valueElement)
                {
                    try
                    {
                        result.Append($"{valueElement.Name} - {valueElement.Result()}\n");
                    }
                    catch (Exception)
                    {
                        return "Can't calculate";
                    }
                }
            }

            return result.ToString();
        }
    }
}