using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;
using Uncas.Fraudo;

namespace Uncas.Sandbox.Fraud
{
    public class LogisticRegression<T>
    {
        public IList<Dimension<T>> Iterate(
            IList<Sample<T>> samples,
            IList<Feature<T>> features,
            double targetDeviation = 0.001d,
            double stepSize = 0.5d,
            int maxIterations = 10000)
        {
            IList<Dimension<T>> dimensions = GetDimensions(features);
            Vector<double> thetas = GetInitialGuessAtTheta(dimensions);
            Console.WriteLine(
                "Iterations to achieve deviation {0:P}:",
                targetDeviation);
            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                thetas = GradientDescent(samples, thetas, stepSize);
                double deviation = GetDeviation(samples);
                bool breakIteration = deviation < targetDeviation;
                if (iteration == 0 || (iteration + 1)%10 == 0 || breakIteration)
                    Console.WriteLine(
                        "  {0}: standard deviation={1:P3}",
                        iteration + 1,
                        deviation);
                if (breakIteration)
                    break;
            }

            OutputBestFit(dimensions, thetas);
            OutputDeviations(samples, targetDeviation);
            foreach (var dimension in dimensions)
                dimension.Theta = thetas.ElementAt(dimensions.IndexOf(dimension));

            return dimensions;
        }

        private static Vector<double> GradientDescent(
            IEnumerable<Sample<T>> samples,
            Vector<double> thetas,
            double stepSize)
        {
            Vector<double> sums = new DenseVector(thetas.Count, 0d);
            foreach (var sample in samples)
            {
                sample.Probability = GetProbability(sample, thetas);
                sums += sample.Deviation*sample.Dimensions;
            }

            thetas += -stepSize*sums;
            return thetas;
        }

        private static double GetDeviation(IList<Sample<T>> samples)
        {
            double deviationSquared =
                samples.Select(sample => Math.Pow(sample.Deviation, 2d)).Sum();
            return Math.Sqrt(deviationSquared/samples.Count);
        }

        private static Vector<double> GetInitialGuessAtTheta(
            IEnumerable<Dimension<T>> dimensions)
        {
            return new DenseVector(
                dimensions.Select(d => d.GetInitialGuess()).ToArray());
        }

        private static void OutputDeviations(
            IEnumerable<Sample<T>> samples,
            double targetDeviation)
        {
            double deviationThreshold = targetDeviation/2d;
            IEnumerable<Sample<T>> deviatingSamples =
                samples.Where(
                x => Math.Abs(x.Deviation) > deviationThreshold).ToList();

            if (!deviatingSamples.Any())
                return;

            Console.WriteLine("Deviations above {0:P}:", deviationThreshold);
            foreach (var sample in deviatingSamples.OrderByDescending(
                x => Math.Abs(x.Deviation)))
                Console.WriteLine(
                    "  {0}, {1:P2}, {2:P2}, {3}",
                    sample.Match,
                    sample.Probability,
                    sample.Deviation,
                    sample.Identifier);
        }

        private static void OutputBestFit(
            IList<Dimension<T>> dimensions,
            Vector<double> thetas)
        {
            Console.WriteLine("Dimensions and best fit:");
            for (int dimensionIndex = 0; 
                dimensionIndex < dimensions.Count;
                dimensionIndex++)
            {
                double theta = thetas[dimensionIndex];
                Dimension<T> dimension = dimensions[dimensionIndex];
                Console.WriteLine(
                    "  Theta={1:N3}, Feature: {0}", 
                    dimension.Description,
                    theta);
            }
        }

        private static IList<Dimension<T>> GetDimensions(IList<Feature<T>> features)
        {
// ReSharper disable UseObjectOrCollectionInitializer
            var dimensions = new List<Dimension<T>>();
// ReSharper restore UseObjectOrCollectionInitializer

            // Zeroth order:
            dimensions.Add(new Dimension<T>());

            // First order:
            dimensions.AddRange(
                features.Select(feature => new Dimension<T>(feature)));

            if (!Program.UseSecondOrder)
                return dimensions;

            // Second order:
            int numberOfFeatures = features.Count;
            for (int featureIndex1 = 0;
                featureIndex1 < numberOfFeatures; 
                featureIndex1++)
            {
                for (int featureIndex2 = featureIndex1;
                    featureIndex2 < numberOfFeatures;
                    featureIndex2++)
                    dimensions.Add(
                        new Dimension<T>(features[featureIndex1], features[featureIndex2]));
            }

            return dimensions;
        }

        private static double GetProbability(
            Sample<T> sample,
            Vector<double> theta)
        {
            double thetaX = sample.Dimensions.DotProduct(theta);
            return SpecialFunctions.Logistic(thetaX);
        }
    }
}