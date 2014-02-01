using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Uncas.Fraudo
{
    public class Sample<T>
    {
        private readonly Vector<double> _dimensions;
        private readonly object _identifier;
        private readonly bool _match;

        public Sample(
            T item,
            object identifier,
            bool match,
            IEnumerable<Feature<T>> features,
            bool useSecondOrder)
        {
            _identifier = identifier;
            _match = match;
            double[] featureValues = features.Select(x => x.Value(item)).ToArray();
            var dimensions = new List<double> {1d};
            dimensions.AddRange(featureValues);
            if (useSecondOrder)
            {
                int numberOfFeatures = featureValues.Length;
                for (int featureIndex1 = 0;
                    featureIndex1 < numberOfFeatures;
                    featureIndex1++)
                {
                    for (int featureIndex2 = featureIndex1;
                        featureIndex2 < numberOfFeatures;
                        featureIndex2++)
                        dimensions.Add(
                            featureValues[featureIndex1]*featureValues[featureIndex2]);
                }
            }

            _dimensions = new DenseVector(dimensions.ToArray());
        }

        public object Identifier
        {
            get { return _identifier; }
        }

        public bool Match
        {
            get { return _match; }
        }

        /// <summary>
        ///     The dimensions that we're going to optimize for.
        /// </summary>
        public Vector<double> Dimensions
        {
            get { return _dimensions; }
        }

        public double Probability { get; set; }

        public double Actual
        {
            get { return Match ? 1d : 0d; }
        }

        public double Deviation
        {
            get { return Probability - Actual; }
        }

        public double GetProbability(Vector<double> theta)
        {
            double thetaX = Dimensions.DotProduct(theta);
            return SpecialFunctions.Logistic(thetaX);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Match, Probability);
        }
    }
}