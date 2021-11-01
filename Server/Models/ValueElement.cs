namespace Server.Models
{
    public class ValueElement : Element
    {
        public bool Value { get; set; }
        public string Name { get; }
        public bool IsInput { get; }
        private Element _inputElement;

        public ValueElement(int id, string name, bool isInput) : base(id)
        {
            IsInput = isInput;
            Name = name;
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
            return $" {Id}:" + type;
        }

        public override bool Result()
        {
            return IsInput ? Value : _inputElement.Result();
        }
    }
}