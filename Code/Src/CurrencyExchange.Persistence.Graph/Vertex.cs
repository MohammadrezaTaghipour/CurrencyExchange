namespace CurrencyExchange.Persistence.Graph
{
    public class Vertex
    {
        public string Id { get; protected set; }

        protected Vertex(string id)
        {
            Id = id;
        }

        public static Vertex Create(string id)
        {
            return new Vertex(id);
        }

        public override bool Equals(object obj)
        {
            var node = obj as Vertex;
            if (node == null)
                return false;

            return Id.Equals(node.Id);
        }

        public override string ToString()
        {
            return Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}