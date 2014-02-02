using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;
using Uncas.Fraudo;

namespace Uncas.Sandbox.Fraud
{
    public class Program
    {
        private const bool UseSecondOrder = false;

        private static void Main()
        {
            var features = new List<Feature<Comment>>();
            var badWordFeature = new BadWordFeature();
            features.Add(new Feature<Comment>("Bad word",
                comment => badWordFeature.NumberOfBadWords(comment), 1d));
            features.Add(new Feature<Comment>("Reputation",
                comment => Math.Log(1d + comment.UserReputation), -1d));
            features.AddRange(BadWordFeature.ContainsIndividualWords());

            IList<Comment> comments = new CommentRepository().GetComments();
            IList<Sample<Comment>> samples =
                comments.Select(
                    comment =>
                        ConvertToSample(comment, features)).
                    ToList();
            var logisticRegression = new LogisticRegression<Comment>(UseSecondOrder);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            IList<Dimension<Comment>> dimensions = logisticRegression.Iterate(samples,
                features, 0.01d);
            stopwatch.Stop();
            Console.WriteLine("Duration: {0:N4} seconds", stopwatch.Elapsed.TotalSeconds);

            // Testing some different reputations:
            for (int userReputation = 0; userReputation < 10; userReputation++)
            {
                var test = new Sample<Comment>(
                    new Comment
                    {
                        Text = "Hej med dig. Sex?",
                        UserReputation = userReputation
                    },
                    "X", false, features, UseSecondOrder);
                Vector<double> denseVector =
                    new DenseVector(dimensions.Select(x => x.Theta).ToArray());
                double probability = test.GetProbability(denseVector);
                Console.WriteLine("{0}, {1:P2}", userReputation, probability);
            }

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
                features,
                UseSecondOrder);
        }
    }
}