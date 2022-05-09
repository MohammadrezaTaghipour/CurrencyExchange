namespace CurrencyExchange.Persistence.Graph
{
    public class AdjacenctVertex
    {
        AdjacenctVertex(Vertex vertex, Edge edge)
        {
            Vertex = vertex;
            Edge = edge;
        }

        public static AdjacenctVertex Create(Edge edge, Vertex vertex)
        {
            return new AdjacenctVertex(vertex, edge);
        }

        public Vertex Vertex { get; }
        public Edge Edge { get; }
    }
}