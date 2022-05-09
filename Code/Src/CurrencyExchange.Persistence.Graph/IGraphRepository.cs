using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.Persistence.Graph
{
    public interface IGraphRepository
    {
        IReadOnlyDictionary<Vertex, HashSet<AdjacenctVertex>> GetAllVertexes();
        Tuple<Vertex, HashSet<AdjacenctVertex>> GetVertexById(string id);
        void Add(Tuple<Vertex, Edge, Vertex> edge);
        void DeleteAll();
        IEnumerable<Vertex> GetShortestPath(Vertex start, Vertex end);
    }
}
