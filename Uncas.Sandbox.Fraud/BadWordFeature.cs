using System.Collections.Generic;
using System.Linq;

namespace Uncas.Sandbox.Fraud
{
    public class BadWordFeature
    {
        private static readonly List<string> Words =
            new List<string>
                {
                    "win",
                    "million",
                    "dollar",
                    "rich",
                    "famous",
                    "sex",
                    "viagra",
                    "bank",
                    "account",
                    "bank transfer"
                };

        public bool ContainsFeature(Comment comment)
        {
            return Words.Any(comment.Text.ToLower().Contains);
        }

        public int NumberOfBadWords(Comment comment)
        {
            return Words.Count(comment.Text.ToLower().Contains);
        }

        public IEnumerable<double> ContainsIndividualWords(Comment comment)
        {
            return Words.Select(word => comment.Text.ToLower().Contains(word) ? 1d : 0d);
        }

        public static IEnumerable<Feature<Comment>> ContainsIndividualWords()
        {
            return Words.Select(GetFunc);
        }

        private static Feature<Comment> GetFunc(string word)
        {
            return new Feature<Comment>("Word: " + word, comment => comment.Text.ToLower().Contains(word) ? 1d : 0d, 1d);
        }
    }
}