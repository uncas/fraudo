using System.Linq;

namespace Uncas.Fraudo
{
    public class Dimension<T>
    {
        public Dimension(params Feature<T>[] features)
        {
            Features = features;
        }

        public Feature<T>[] Features { get; private set; }
        public double Theta { get; set; }

        public string Description
        {
            get
            {
                if (!HasFeatures())
                    return "Empty";
                return Features.Select(f => f.Name).Aggregate((i, j) => i + " +++ " + j);
            }
        }

        public double GetInitialGuess()
        {
            if (!HasFeatures())
                return 0d;
            return Features.Sum(f => f.InitialGuess);
        }

        private bool HasFeatures()
        {
            return Features != null && Features.Length > 0;
        }
    }
}