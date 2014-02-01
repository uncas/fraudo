using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Uncas.Fraudo
{
    public class Sample<T>
    {
        public Sample(
            T item,
            object identifier,
            bool match,
            IEnumerable<Feature<T>> features,
            bool useSecondOrder)
        {
            Identifier = identifier;
            Match = match;
            Features = features.Select(x => x.Value(item)).ToArray();
            var dimensions = new List<double> {1d};
            dimensions.AddRange(Features);
            if (useSecondOrder)
            {
                int numberOfFeatures = Features.Length;
                for (int featureIndex1 = 0;
                    featureIndex1 < numberOfFeatures;
                    featureIndex1++)
                {
                    for (int featureIndex2 = featureIndex1;
                        featureIndex2 < numberOfFeatures;
                        featureIndex2++)
                        dimensions.Add(
                            Features[featureIndex1]*Features[featureIndex2]);
                }
            }

            Dimensions = new DenseVector(dimensions.ToArray());
        }

        public object Identifier { get; private set; }
        public bool Match { get; private set; }

        /// <summary>
        ///     The features of the sample.
        /// </summary>
        public double[] Features { get; private set; }

        /// <summary>
        ///     The dimensions that we're going to optimize for.
        /// </summary>
        public Vector<double> Dimensions { get; private set; }

        public double Probability { get; set; }

        public double Actual
        {
            get { return Match ? 1d : 0d; }
        }

        public double Deviation
        {
            get { return Probability - Actual; }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Match, Probability);
        }
    }
}