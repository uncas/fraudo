using System.Collections.Generic;

namespace Uncas.Sandbox.Fraud
{
    public class CommentRepository
    {
        public IList<Comment> GetComments()
        {
            var comments = new List<Comment>
                               {
                                   new Comment
                                       {
                                           IsFraud = false,
                                           UserReputation = 1,
                                           Text = "Hi a dollar for your thoughts"
                                       },
                                   new Comment
                                       {
                                           IsFraud = false,
                                           UserReputation = 32,
                                           Text = "Hi a dollar for your thoughts"
                                       },
                                   new Comment
                                       {
                                           IsFraud = false,
                                           UserReputation = 12,
                                           Text = "Hi again I saw a rich man"
                                       },
                                   new Comment
                                       {
                                           IsFraud = false,
                                           UserReputation = 92,
                                           Text = "Hi how are you doing"
                                       },
                                   new Comment
                                       {
                                           IsFraud = false,
                                           UserReputation = 32,
                                           Text = "Hi a dollar for your thoughts"
                                       },
                                   new Comment
                                       {
                                           IsFraud = false,
                                           UserReputation = 12,
                                           Text = "Hi again I saw a rich man"
                                       },
                                   new Comment
                                       {
                                           IsFraud = false,
                                           UserReputation = 92,
                                           Text = "Hi how are you doing"
                                       },
                                   new Comment
                                       {
                                           IsFraud = false,
                                           UserReputation = 132,
                                           Text = "Hi how are you doing"
                                       },
                                   new Comment
                                       {
                                           IsFraud = false,
                                           UserReputation = 0,
                                           Text = "Hi how are you doing"
                                       },
                                   new Comment
                                       {
                                           IsFraud = false,
                                           UserReputation = 32,
                                           Text = "Hi how are you doing"
                                       },
                                   new Comment
                                       {
                                           IsFraud = false,
                                           UserReputation = 42,
                                           Text = "Hi how are you doing"
                                       },
                                   new Comment
                                       {
                                           IsFraud = false,
                                           UserReputation = 82,
                                           Text = "Hi how are you doing"
                                       },
                                   new Comment
                                       {
                                           IsFraud = false,
                                           UserReputation = 0,
                                           Text = "Hi again"
                                       },
                                   new Comment
                                       {
                                           IsFraud = true,
                                           UserReputation = 1,
                                           Text = "Win a million dollars"
                                       },
                                   new Comment
                                       {
                                           IsFraud = true,
                                           UserReputation = 23,
                                           Text = "Win a million dollars and get rich"
                                       },
                                   new Comment
                                       {
                                           IsFraud = true,
                                           UserReputation = 12,
                                           Text = "Win a million dollars and get famous"
                                       },
                                   new Comment
                                       {
                                           IsFraud = true,
                                           UserReputation = 0,
                                           Text = "Win a million dollars and get sex"
                                       },
                                   new Comment
                                       {
                                           IsFraud = true,
                                           UserReputation = 2,
                                           Text = "Win a million dollars"
                                       },
                                   new Comment
                                       {
                                           IsFraud = true,
                                           UserReputation = 2,
                                           Text = "One million dollars. Please make a bank transfer to this account."
                                       },
                                   new Comment
                                       {
                                           IsFraud = true,
                                           UserReputation = 0,
                                           Text = "One thousand dollars. Viagra makes you happy."
                                       }
                               };
            var result = new List<Comment>();
            result.AddRange(comments);
            return result;
        }
    }
}