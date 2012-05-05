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

            Console.WriteLine("Iterations:");
            for (int iteration = 0; iteration < iterations; iteration++)
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

                if (iteration == 0 || (iteration + 1)%10 == 0)
                {
                    double deviationSquared = samples.Select(s => Math.Pow(s.Deviation, 2d)).Sum();
                    double deviation = Math.Sqrt(deviationSquared/samples.Count);
                    Console.WriteLine("  {0}: standard deviation={1:P3}", iteration + 1, deviation);
                }
            }

            Console.WriteLine("Features and best fit:");
            for (int featureIndex = 0; featureIndex < numberOfFeatures; featureIndex++)
            {
                double theta = thetas[featureIndex];
                Feature<T> feature = features[featureIndex];
                Console.WriteLine("  Theta={1:N3}, Feature: {0}", feature.Name, theta);
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
                sample.Features.Select((feature, featureIndex) => theta.ElementAt(featureIndex)*feature).Sum();
            return 1d/(1d + Math.Exp(-thetaX));
        }

        private static double Cost<T>(
            IList<Sample<T>> samples,
            IList<double> theta)
        {
            double cost = 0d;
            foreach (var sample in samples)
            {
                double y = sample.Match ? 1d : 0d;
                double h = Probability(sample, theta);
                cost += -y*Math.Log(h) - (1 - y)*Math.Log(1 - h);
            }

            return cost/samples.Count;
        }
    }
}