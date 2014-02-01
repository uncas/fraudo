using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Uncas.Fraudo
{
    public class LogisticRegression<T>
    {
        private readonly bool _useSecondOrder;

        public LogisticRegression(bool useSecondOrder)
        {
            _useSecondOrder = useSecondOrder;
        }

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

            foreach (var dimension in dimensions)
                dimension.Theta = thetas.ElementAt(dimensions.IndexOf(dimension));
            ResultWriter.OutputBestFit(dimensions);
            ResultWriter.OutputDeviations(samples, targetDeviation);
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
                sample.Probability = sample.GetProbability(thetas);
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

        private IList<Dimension<T>> GetDimensions(IList<Feature<T>> features)
        {
            // ReSharper disable UseObjectOrCollectionInitializer
            var dimensions = new List<Dimension<T>>();
            // ReSharper restore UseObjectOrCollectionInitializer

            // Zeroth order:
            dimensions.Add(new Dimension<T>());

            // First order:
            dimensions.AddRange(
                features.Select(feature => new Dimension<T>(feature)));

            if (!_useSecondOrder)
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
    }
}