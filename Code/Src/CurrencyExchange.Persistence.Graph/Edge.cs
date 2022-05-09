namespace CurrencyExchange.Persistence.Graph
{
    public class Edge
    {
        Edge(double value, bool primaryDirection)
        {
            Value = value;
            PrimaryDirection = primaryDirection;
        }

        public static Edge Create(double value, bool primaryDirection = true)
        {
            return new Edge(value, primaryDirection);
        }

        public double Value { get; }
        public bool PrimaryDirection { get; }

    }
}