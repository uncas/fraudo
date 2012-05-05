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
                if (!HasFeatures())
                    return "Empty";
                return Features.Select(f => f.Name).Aggregate((i, j) => i + " +++ " + j);
            }
        }

        private bool HasFeatures()
        {
            return Features != null && Features.Length > 0;
        }

        public double GetInitialGuess()
        {
            if (!HasFeatures())
                return 0d;
            return Features.Sum(f => f.InitialGuess);
        }
    }
}