using System;
using System.Collections.Generic;
using System.Linq;

namespace Uncas.Sandbox.Fraud
{
    public class Program
    {
        private static void Main()
        {
            const double alpha = 0.5d;
            const int iterations = 2000;

            IList<Comment> comments = new CommentRepository().GetComments();
            var badWordFeature = new BadWordFeature();
            IEnumerable<Sample<Comment>> samples =
                comments.Select(
                    c =>
                    ConvertToSample(c, badWordFeature)).
                    ToList();

            int numberOfFeatures = samples.First().Features.Length;

            // Guess at theta
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

                List<double> delta = sums.Select(s => -alpha*s).ToList();
                thetas = thetas.Select((x, j) => x + delta[j]).ToList();
            }

            foreach (double theta in thetas)
                Console.WriteLine("Theta: {0:N3}", theta);
            foreach (var sample in samples)
                Console.WriteLine("{0}, {1:N6}", sample.Match, sample.Probability);
            Console.ReadKey();
        }

        private static Sample<Comment> ConvertToSample(
            Comment comment,
            BadWordFeature badWordFeature)
        {
            const double nullFeature = 1d;
            double containsBadWord = badWordFeature.ContainsFeature(comment) ? 1d : 0d;
            return new Sample<Comment>(
                comment,
                comment.IsFraud,
                nullFeature,
                badWordFeature.NumberOfBadWords(comment)); //,
            //containsBadWord);
        }

        private static double Probability(Sample<Comment> s, IEnumerable<double> theta)
        {
            double thetaX = s.Features.Select((t, i) => theta.ElementAt(i)*t).Sum();
            return 1d/(1d + Math.Exp(-thetaX));
        }
    }
}