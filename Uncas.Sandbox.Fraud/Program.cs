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

            // Guess at theta
            IEnumerable<double> theta = new[] {0.1d, 1d}.ToList();

            IList<Comment> comments = new CommentRepository().GetComments();
            var badWordFeature = new BadWordFeature();
            IEnumerable<Sample<Comment>> samples =
                comments.Select(c => new Sample<Comment>(c, c.IsFraud, 1d, badWordFeature.NumberOfBadWords(c))).
                    ToList();

            for (int i = 0; i < 100; i++)
            {
                foreach (var sample in samples)
                    sample.Probability = Probability(sample, theta);
                List<double> sums = theta.Select(t => 0d).ToList();
                foreach (var sample in samples)
                {
                    for (int featureIndex = 0; featureIndex < sample.Features.Count(); featureIndex++)
                    {
                        sums[featureIndex] +=
                            (sample.Probability - (sample.Match ? 1 : 0))*
                            sample.Features[featureIndex];
                    }
                }

                List<double> delta = sums.Select(s => -alpha*s).ToList();
                theta = theta.Select((x, j) => x + delta[j]).ToList();
            }

            Console.WriteLine("{0:N3}, {1:N3}", theta.ElementAt(0), theta.ElementAt(1));
            foreach (var sample in samples)
                Console.WriteLine("{0}, {1:N6}", sample.Match, sample.Probability);
            Console.ReadKey();
        }

        private static double Probability(Sample<Comment> s, IEnumerable<double> theta)
        {
            double thetaX = s.Features.Select((t, i) => theta.ElementAt(i)*t).Sum();
            return 1d/(1d + Math.Exp(-thetaX));
        }
    }
}