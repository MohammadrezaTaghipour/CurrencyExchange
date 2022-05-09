using CurrencyExchange.Persistence.Graph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyExchange.Domain
{
    public class CurrencyConverter : ICurrencyConverter
    {
        IGraphRepository _graphRepository;

        public CurrencyConverter(IGraphRepository graphRepository)
        {
            _graphRepository = graphRepository;
        }

        public void ClearConfiguration()
        {
            _graphRepository.DeleteAll();
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            var startConrrency = Vertex.Create(fromCurrency);
            var endCorrency = Vertex.Create(toCurrency);
            var shortestPath = _graphRepository.GetShortestPath(
                startConrrency, endCorrency).ToArray();

            double result = amount;

            for (int i = 0; i < shortestPath.Length - 1; i++)
            {
                if (shortestPath.Length < i + 1)
                    return result;

                var currencyVertex1 = _graphRepository.GetVertexById(shortestPath[i].Id);
                var currencyVertex2 = _graphRepository.GetVertexById(shortestPath[i + 1].Id);

                result = ConvertCurrency(currencyVertex1, currencyVertex2, result);
            }

            return result;
        }

        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            foreach (var conversionRate in conversionRates.ToList())
            {
                _graphRepository.Add(Tuple.Create(
                    Vertex.Create(conversionRate.Item1),
                    Edge.Create(conversionRate.Item3),
                    Vertex.Create(conversionRate.Item2)));
            }
        }

        double ConvertCurrency(Tuple<Vertex, HashSet<AdjacenctVertex>> fromCurrencyVertex,
            Tuple<Vertex, HashSet<AdjacenctVertex>> toCurrencyVertex, double amount)
        {
            var toCurrency = fromCurrencyVertex.Item2.First(a => a.Vertex.Id == toCurrencyVertex.Item1.Id);
            return toCurrency.Edge.PrimaryDirection
                ? amount * toCurrency.Edge.Value
                : amount / toCurrency.Edge.Value;
        }
    }
}
