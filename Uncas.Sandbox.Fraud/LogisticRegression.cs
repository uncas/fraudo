using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Uncas.Sandbox.Fraud
{
    public class LogisticRegression
    {
        public static readonly bool UseSecondOrder = true;

        public Vector<double> Iterate<T>(
            IList<Sample<T>> samples,
            IList<Feature<T>> features,
            double stepSize,
            int iterations)
        {
            var dimensions = new List<Dimension<T>> {new Dimension<T>()};
            dimensions.AddRange(features.Select(feature => new Dimension<T>(feature)));
            if (UseSecondOrder)
            {
                int numberOfFeatures = features.Count;
                for (int featureIndex1 = 0; featureIndex1 < numberOfFeatures; featureIndex1++)
                {
                    for (int featureIndex2 = featureIndex1; featureIndex2 < numberOfFeatures; featureIndex2++)
                        dimensions.Add(new Dimension<T>(features[featureIndex1], features[featureIndex2]));
                }
            }

            int numberOfDimensions = dimensions.Count;

            // Initial guess at theta:
            Vector<double> thetas = new DenseVector(dimensions.Select(d => d.GetInitialGuess()).ToArray());

            Console.WriteLine("Iterations:");
            for (int iteration = 0; iteration < iterations; iteration++)
            {
                foreach (var sample in samples)
                    sample.Probability = Probability(sample, thetas);
                var sums = new DenseVector(numberOfDimensions, 0d);
                foreach (var sample in samples)
                {
                    for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
                    {
                        sums[dimensionIndex] +=
                            (sample.Probability - (sample.Match ? 1 : 0))*
                            sample.Dimensions[dimensionIndex];
                    }
                }

                Vector<double> delta = -stepSize*sums;
                thetas += delta;

                if (iteration == 0 || (iteration + 1)%10 == 0)
                {
                    double deviationSquared = samples.Select(s => Math.Pow(s.Deviation, 2d)).Sum();
                    double deviation = Math.Sqrt(deviationSquared/samples.Count);
                    Console.WriteLine("  {0}: standard deviation={1:P3}", iteration + 1, deviation);
                }
            }

            Console.WriteLine("Dimensions and best fit:");
            for (int dimensionIndex = 0; dimensionIndex < numberOfDimensions; dimensionIndex++)
            {
                double theta = thetas[dimensionIndex];
                Dimension<T> dimension = dimensions[dimensionIndex];
                Console.WriteLine("  Theta={1:N3}, Feature: {0}", dimension.Description, theta);
            }

            const double deviationThreshold = 0.001d;
            IEnumerable<Sample<T>> deviatingSamples =
                samples.Where(x => Math.Abs(x.Deviation) > deviationThreshold).ToList();
            if (deviatingSamples.Any())
            {
                Console.WriteLine("Deviations above {0:P1}:", deviationThreshold);
                foreach (var sample in deviatingSamples.OrderByDescending(x => Math.Abs(x.Deviation)))
                    Console.WriteLine(
                        "  {0}, {1:P2}, {2:P2}, {3}",
                        sample.Match,
                        sample.Probability,
                        sample.Deviation,
                        sample.Identifier);
            }

            return thetas;
        }

        private static double Probability<T>(
            Sample<T> sample,
            IEnumerable<double> theta)
        {
            double thetaX =
                sample.Dimensions.Select((dimension, dimensionIndex) => theta.ElementAt(dimensionIndex)*dimension).Sum();
            return SpecialFunctions.Logistic(thetaX);
        }
    }
}