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
            var badWordFeature = new BadWordFeature();
            features.Add(new Feature<Comment>("Bad word", comment => badWordFeature.NumberOfBadWords(comment)));
            features.Add(new Feature<Comment>("Reputation", comment => Math.Log(1d + comment.UserReputation)));
            features.AddRange(BadWordFeature.ContainsIndividualWords());

            IList<Comment> comments = new CommentRepository().GetComments();
            IList<Sample<Comment>> samples =
                comments.Select(
                    comment =>
                    ConvertToSample(comment, features)).
                    ToList();

            var logisticRegression = new LogisticRegression();
            logisticRegression.Iterate(samples, features, stepSize, iterations);
            Console.ReadKey();
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