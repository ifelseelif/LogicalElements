using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Server.Exceptions;

namespace Server.Models.domain
{
    public class LogicalElement : Element
    {
        private List<Element> InputElements { get; }

        public ElemType ElemType { get; }

        public LogicalElement(int id, ElemType elemType)
        {
            Id = id;
            InputElements = new List<Element>();
            ElemType = elemType;
        }

        public override void AddInput(Element element)
        {
            if (InputElements.Count >= 2) return;
            if (ElemType == ElemType.not && InputElements.Count >= 1) return;
            InputElements.Add(element);
        }

        public override bool Result()
        {
            if (ElemType == ElemType.not) return !InputElements[0].Result();
            if (InputElements.Count != 2) throw new CalculationError();
            return ElemType switch
            {
                ElemType.or => InputElements[0].Result() | InputElements[1].Result(),
                ElemType.and => InputElements[0].Result() & InputElements[1].Result(),
                ElemType.xor => InputElements[0].Result() ^ InputElements[1].Result(),
                _ => throw new CalculationError()
            };
        }

        public override string Show()
        {
            var connectedElements = new StringBuilder("(");
            foreach (var inputElement in InputElements)
            {
                connectedElements.Append(inputElement);
            }

            connectedElements.Append(')');
            return ToString() + connectedElements;
        }

        public override string ToString()
        {
            return $"{Id}:{ElemType.ToString()}";
        }
    }
}