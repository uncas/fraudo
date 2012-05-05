using System;
using System.Collections.Generic;
using System.Linq;

namespace Uncas.Sandbox.Fraud
{
    public class LogisticRegression
    {
        public IList<double> Iterate<T>(
            IList<Sample<T>> samples,
            IList<Feature<T>> features,
            double stepSize,
            int iterations)
        {
            int numberOfFeatures = samples.First().Features.Length;

            // Initial guess at theta:
            IList<double> thetas = new List<double>();
            for (int j = 0; j < numberOfFeatures; j++)
                thetas.Add(1d);

            for (int i = 0; i < iterations; i++)
            {
                foreach (var sample in samples)
                    sample.Probability = Probability(sample, thetas);
                List<double> sums = thetas.Select(t => 0d).ToList();
                foreach (var sample in samples)
                {
                    for (int featureIndex = 0; featureIndex < numberOfFeatures; featureIndex++)
                    {
                        sums[featureIndex] +=
                            (sample.Probability - (sample.Match ? 1 : 0))*
                            sample.Features[featureIndex];
                    }
                }

                List<double> delta = sums.Select(sum => -stepSize*sum).ToList();
                thetas = thetas.Select((x, j) => x + delta[j]).ToList();
            }

            for (int featureIndex = 0; featureIndex < numberOfFeatures; featureIndex++)
            {
                double theta = thetas[featureIndex];
                Feature<T> feature = features[featureIndex];
                Console.WriteLine("Theta={1:N3}, Feature: {0}", feature.Name, theta);
            }

            foreach (var sample in samples)
                Console.WriteLine("{0}, {1:P2}", sample.Match, sample.Probability);

            return thetas;
        }

        private static double Probability<T>(
            Sample<T> sample,
            IEnumerable<double> theta)
        {
            double thetaX =
                sample.Features.Select((feature, featureIndex) => theta.ElementAt(featureIndex)*feature).Sum();
            return 1d/(1d + Math.Exp(-thetaX));
        }
    }
}