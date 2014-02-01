using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Uncas.Fraudo
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

        public double Theta { get; set; }

        public static Vector<double> GetInitialGuessAtTheta(
            IEnumerable<Dimension<T>> dimensions)
        {
            return new DenseVector(
                dimensions.Select(d => d.GetInitialGuess()).ToArray());
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