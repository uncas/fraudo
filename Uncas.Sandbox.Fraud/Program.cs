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
            const int iterations = 1000;

            var features = new List<Feature<Comment>>();
            features.Add(Feature<Comment>.Null());
            var badWordFeature = new BadWordFeature();
            features.Add(new Feature<Comment>("Bad word", x => badWordFeature.NumberOfBadWords(x)));
            features.Add(new Feature<Comment>("Reputation", x => x.UserReputation/100d));
            features.AddRange(BadWordFeature.ContainsIndividualWords());

            IList<Comment> comments = new CommentRepository().GetComments();
            IEnumerable<Sample<Comment>> samples =
                comments.Select(
                    c =>
                    ConvertToSample(c, features)).
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

            for (int featureIndex = 0; featureIndex < numberOfFeatures; featureIndex++)
            {
                double theta = thetas[featureIndex];
                Feature<Comment> feature = features[featureIndex];
                Console.WriteLine("Theta={1:N3}, Feature: {0}", feature.Name, theta);
            }

            foreach (var sample in samples)
                Console.WriteLine("{0}, {1:P2}", sample.Match, sample.Probability);
            Console.ReadKey();
        }

        private static Sample<Comment> ConvertToSample(
            Comment comment,
            List<Feature<Comment>> features)
        {
            const double nullFeature = 1d;
            return new Sample<Comment>(
                comment,
                comment.IsFraud,
                features);
        }

        private static double Probability(Sample<Comment> s, IEnumerable<double> theta)
        {
            double thetaX = s.Features.Select((t, i) => theta.ElementAt(i)*t).Sum();
            return 1d/(1d + Math.Exp(-thetaX));
        }
    }
}