using Common;

namespace Server.Models.domain
{
    public abstract class Element
    {
        public int Id { get; set; }
        public ElemType ElemType { get; set; }
        public abstract string Show();
        public abstract void AddInput(Element firstElement);
        public abstract bool Result();
    }
}