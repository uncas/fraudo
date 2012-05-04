using System;
using System.Collections.Generic;
using System.Linq;

namespace Uncas.Sandbox.Fraud
{
    public class Program
    {
        private static void Main(string[] args)
        {
            const double alpha = 0.5d;

            // Guess at theta
            double theta0 = 0.1d;
            double theta1 = 1d;

            IList<Comment> comments = new CommentRepository().GetComments();
            var badWordFeature = new BadWordFeature();
            IEnumerable<Sample<Comment>> samples =
                comments.Select(c => new Sample<Comment>(c, c.IsFraud, 1d, badWordFeature.NumberOfBadWords(c))).
                    ToList();

            for (int i = 0; i < 100; i++)
            {
                foreach (var sample in samples)
                    sample.Probability = Probability(sample, theta0, theta1);
                double sum0 = 0d;
                double sum1 = 0d;
                foreach (var sample in samples)
                {
                    sum0 += (sample.Probability - (sample.Match ? 1 : 0))*sample.Features[0];
                    sum1 += (sample.Probability - (sample.Match ? 1 : 0))*sample.Features[1];
                }

                double delta0 = -alpha*sum0;
                double delta1 = -alpha*sum1;

                theta0 += delta0;
                theta1 += delta1;
            }

            Console.WriteLine("{0:N3}, {1:N3}", theta0, theta1);
            foreach (var sample in samples)
                Console.WriteLine("{0}, {1:N6}", sample.Match, sample.Probability);
            Console.ReadKey();
        }

        private static double Probability(Sample<Comment> s, params double[] theta)
        {
            double thetaX = s.Features.Select((t, i) => theta[i]*t).Sum();
            return 1d/(1d + Math.Exp(-thetaX));
        }
    }
}