using System.Collections.Generic;

namespace Uncas.Sandbox.Fraud
{
    public class CommentRepository
    {
        public IList<Comment> GetComments()
        {
            var comments = new List<Comment>
                               {
                                   new Comment {IsFraud = false, Text = "Hi a dollar for your thoughts"},
                                   new Comment {IsFraud = false, Text = "Hi again I saw a rich man"},
                                   new Comment {IsFraud = false, Text = "Hi how are you doing"},
                                   new Comment {IsFraud = false, Text = "Hi how are you doing"},
                                   new Comment {IsFraud = false, Text = "Hi how are you doing"},
                                   new Comment {IsFraud = false, Text = "Hi how are you doing"},
                                   new Comment {IsFraud = false, Text = "Hi how are you doing"},
                                   new Comment {IsFraud = false, Text = "Hi how are you doing"},
                                   new Comment {IsFraud = false, Text = "Hi again"},
                                   new Comment {IsFraud = true, Text = "Win a million dollars"},
                                   new Comment {IsFraud = true, Text = "Win a million dollars and get rich"},
                                   new Comment {IsFraud = true, Text = "Win a million dollars and get famous"},
                                   new Comment {IsFraud = true, Text = "Win a million dollars and get sex"},
                                   new Comment {IsFraud = true, Text = "Win a million dollars"},
                                   new Comment {IsFraud = true, Text = "One million dollars"}
                               };
            return comments;
        }
    }
}