using Common;

namespace Client
{
    public class Message
    {
        public CommandType CommandType { get; set; }
        public ElemType ElemType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsInput { get; set; }
        public int OutputId { get; set; }
        public int InputId { get; set; }
        public bool Value { get; set; }
    }
}