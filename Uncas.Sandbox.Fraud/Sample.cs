namespace Uncas.Sandbox.Fraud
{
    public class Sample<T>
    {
        public Sample(T item, bool match, params double[] features)
        {
            Item = item;
            Match = match;
            Features = features;
        }

        public T Item { get; private set; }
        public bool Match { get; private set; }
        public double[] Features { get; private set; }

        public double Probability { get; set; }
    }
}