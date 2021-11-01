namespace Server.Models
{
    public abstract class Element
    {
        protected readonly int Id;

        protected Element(int id)
        {
            Id = id;
        }

        public abstract string Show();
        public abstract void AddInput(Element firstElement);
        public abstract bool Result();
    }
}