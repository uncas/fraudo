using System;
using System.Collections.Generic;
using System.Linq;

namespace Uncas.Sandbox.Fraud
{
    public class Program
    {
        private static void Main()
        {
            const double stepSize = 0.5d;
            const int iterations = 100;

            var features = new List<Feature<Comment>>();
            features.Add(Feature<Comment>.Null());
            var badWordFeature = new BadWordFeature();
            features.Add(new Feature<Comment>("Bad word", x => badWordFeature.NumberOfBadWords(x)));
            features.Add(new Feature<Comment>("Reputation", x => Math.Log(1d + x.UserReputation)));
            features.AddRange(BadWordFeature.ContainsIndividualWords());

            IList<Comment> comments = new CommentRepository().GetComments();
            IList<Sample<Comment>> samples =
                comments.Select(
                    c =>
                    ConvertToSample(c, features)).
                    ToList();

            var logisticRegression = new LogisticRegression();
            IList<double> thetas = logisticRegression.Iterate(samples, features, stepSize, iterations);
            //Console.ReadKey();
        }

        private static Sample<Comment> ConvertToSample(
            Comment comment,
            IEnumerable<Feature<Comment>> features)
        {
            return new Sample<Comment>(
                comment,
                comment.Text,
                comment.IsFraud,
                features);
        }
    }
}