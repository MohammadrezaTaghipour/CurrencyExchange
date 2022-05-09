using System.Collections.Concurrent;

namespace CurrencyExchange.Persistence.Graph
{
    public class InMemoryGraphRepository : IGraphRepository
    {
        public InMemoryGraphRepository()
        {
        }

        static ConcurrentDictionary<Vertex, HashSet<AdjacenctVertex>> _vertexes =
            new ConcurrentDictionary<Vertex, HashSet<AdjacenctVertex>>();

        public Tuple<Vertex, HashSet<AdjacenctVertex>> GetVertexById(string id)
        {
            var vertex = _vertexes.Keys.FirstOrDefault(a => a.Id == id);
            return vertex != null
                 ? Tuple.Create(vertex, _vertexes[vertex])
                 : null;
        }

        public IReadOnlyDictionary<Vertex, HashSet<AdjacenctVertex>> GetAllVertexes()
        {
            return _vertexes;
        }

        public void Add(Tuple<Vertex, Edge, Vertex> edge)
        {
            if (!_vertexes.ContainsKey(edge.Item1))
                _vertexes.TryAdd(edge.Item1, new HashSet<AdjacenctVertex>());

            if (!_vertexes.ContainsKey(edge.Item3))
                _vertexes.TryAdd(edge.Item3, new HashSet<AdjacenctVertex>());


            _vertexes[edge.Item1].Add(AdjacenctVertex.Create(Edge.Create(edge.Item2.Value, true), edge.Item3));
            _vertexes[edge.Item3].Add(AdjacenctVertex.Create(Edge.Create(edge.Item2.Value, false), edge.Item1));
        }

        public void DeleteAll()
        {
            _vertexes.Clear();
        }

        public IEnumerable<Vertex> GetShortestPath(Vertex start, Vertex end)
        {
            var previous = new Dictionary<Vertex, Vertex>();

            var queue = new Queue<Vertex>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in _vertexes[vertex])
                {
                    if (previous.ContainsKey(neighbor.Vertex))
                        continue;

                    previous[neighbor.Vertex] = _vertexes.Keys.First(a => a.Id == vertex.Id);
                    queue.Enqueue(neighbor.Vertex);
                }
            }


            return ShortestPath(previous, start, end);
        }

        IEnumerable<Vertex> ShortestPath(
            Dictionary<Vertex, Vertex> previous, Vertex start, Vertex end)
        {
            var path = new List<Vertex> { };

            var current = end;
            while (!current.Equals(start))
            {
                path.Add(current);
                current = previous[current];
            }

            path.Add(start);
            path.Reverse();

            return path;
        }
    }
}
