using System.Collections.Generic;
using System.Linq;

namespace Uncas.Sandbox.Fraud
{
    public class BadWordFeature
    {
        private readonly List<string> _words =
            new List<string>
                {
                    "win",
                    "million",
                    "dollar",
                    "rich",
                    "famous",
                    "sex"
                };

        public bool ContainsFeature(Comment comment)
        {
            return _words.Any(comment.Text.ToLower().Contains);
        }

        public int NumberOfBadWords(Comment comment)
        {
            return _words.Count(comment.Text.ToLower().Contains);
        }

        public IEnumerable<double> ContainsIndividualWords(Comment comment)
        {
            return _words.Select(word => comment.Text.ToLower().Contains(word) ? 1d : 0d);
        }
    }
}