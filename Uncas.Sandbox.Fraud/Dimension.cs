using System.Linq;

namespace Uncas.Sandbox.Fraud
{
    public class Dimension<T>
    {
        public Dimension(params Feature<T>[] features)
        {
            Features = features;
        }

        public Feature<T>[] Features { get; private set; }

        public string Description
        {
            get
            {
                if (Features == null || Features.Length == 0)
                    return "Empty";
                return Features.Select(f => f.Name).Aggregate((i, j) => i + " +++ " + j);
            }
        }
    }
}