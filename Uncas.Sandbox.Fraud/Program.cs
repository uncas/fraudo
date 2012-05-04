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
                double sum0 = 0d;
                double sum1 = 0d;
                foreach (var sample in samples)
                {
                    sum0 += (sample.Probability - (sample.Match ? 1 : 0))*sample.Features[0];
                    sum1 += (sample.Probability - (sample.Match ? 1 : 0))*sample.Features[1];
                }

                double delta0 = -alpha*sum0;
                double delta1 = -alpha*sum1;
                var delta = new[] {delta0, delta1};
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