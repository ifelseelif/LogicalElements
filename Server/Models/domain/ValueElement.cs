using Common;

namespace Server.Models.domain
{
    public class ValueElement : Element
    {
        public bool Value { get; }
        public string Name { get; }
        public bool IsInput { get; }

        private Element _inputElement;

        public ValueElement(string name, bool isInput)
        {
            ElemType = ElemType.value;
            IsInput = isInput;
            Name = name;
        }

        public ValueElement(bool value, string name, bool isInput) : this(name, isInput)
        {
            Value = value;
        }

        public override string Show()
        {
            return ToString();
        }

        public override void AddInput(Element element)
        {
            _inputElement = element;
        }

        public override string ToString()
        {
            var type = IsInput ? "input" : "output";
            return $" {Id}:{Name}:{Value}:" + type;
        }

        public override bool Result()
        {
            if (!IsInput && _inputElement == null) return false;
            return IsInput ? Value : _inputElement.Result();
        }
    }
}