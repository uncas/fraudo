using System.Collections.Generic;
using System.Linq;

namespace Uncas.Sandbox.Fraud
{
    public class Sample<T>
    {
        public Sample(T item, bool match, IEnumerable<Feature<T>> features)
        {
            Item = item;
            Match = match;
            Features = features.Select(x => x.Value(item)).ToArray();
        }

        public T Item { get; private set; }
        public bool Match { get; private set; }
        public double[] Features { get; private set; }

        public double Probability { get; set; }
    }
}