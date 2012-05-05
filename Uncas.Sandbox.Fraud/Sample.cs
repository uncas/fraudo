using System.Collections.Generic;
using System.Linq;

namespace Uncas.Sandbox.Fraud
{
    public class Sample<T>
    {
        public Sample(T item, object identifier, bool match, IEnumerable<Feature<T>> features)
        {
            Item = item;
            Identifier = identifier;
            Match = match;
            Features = features.Select(x => x.Value(item)).ToArray();
        }

        public T Item { get; private set; }
        public object Identifier { get; private set; }
        public bool Match { get; private set; }
        public double[] Features { get; private set; }

        public double Probability { get; set; }

        public double Actual
        {
            get { return Match ? 1d : 0d; }
        }

        public double Deviation
        {
            get { return Actual - Probability; }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Match, Probability);
        }
    }
}