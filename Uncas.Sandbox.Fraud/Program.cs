using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Uncas.Fraudo;

namespace Uncas.Sandbox.Fraud
{
    public class Program
    {
        private const bool UseSecondOrder = true;

        private static void Main()
        {
            var features = new List<Feature<Comment>>();
            var badWordFeature = new BadWordFeature();
            features.Add(new Feature<Comment>("Bad word", comment => badWordFeature.NumberOfBadWords(comment), 1d));
            features.Add(new Feature<Comment>("Reputation", comment => Math.Log(1d + comment.UserReputation), -1d));
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
            logisticRegression.Iterate(samples, features);
            stopwatch.Stop();
            Console.WriteLine("Duration: {0:N4} seconds", stopwatch.Elapsed.TotalSeconds);
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