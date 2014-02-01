using System;
using System.Collections.Generic;
using System.Linq;

namespace Uncas.Fraudo
{
    public class ResultWriter
    {
        public static void OutputBestFit<T>(IEnumerable<Dimension<T>> dimensions)
        {
            Console.WriteLine("Dimensions and best fit:");
            foreach (var dimension in dimensions)
            {
                Console.WriteLine(
                    "  Theta={1:N3}, Feature: {0}",
                    dimension.Description,
                    dimension.Theta);
            }
        }

        public static void OutputDeviations<T>(
            IEnumerable<Sample<T>> samples,
            double targetDeviation)
        {
            double deviationThreshold = targetDeviation;
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
    }
}