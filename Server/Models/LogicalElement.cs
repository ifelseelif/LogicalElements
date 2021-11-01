using System.Collections.Generic;
using System.Text;
using Common;

namespace Server.Models
{
    public class LogicalElement : Element
    {
        private List<Element> InputElements { get; }

        private readonly ElemType _elemType;

        public LogicalElement(int id, ElemType elemType) : base(id)
        {
            InputElements = new List<Element>();
            _elemType = elemType;
        }

        public override void AddInput(Element element)
        {
            if (InputElements.Count >= 2) return;
            if (_elemType == ElemType.not && InputElements.Count >= 1) return;
            InputElements.Add(element);
        }

        public override bool Result()
        {
            if (_elemType == ElemType.not) return !InputElements[0].Result();
            if (InputElements.Count != 2) throw new CalculationError();
            return _elemType switch
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
            return $"{Id}:{_elemType.ToString()}";
        }
    }
}