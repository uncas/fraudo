using System.Collections.Generic;

namespace Uncas.Sandbox.Fraud
{
    public class CommentRepository
    {
        public IList<Comment> GetComments()
        {
            var comments = new List<Comment>
                               {
                                   new Comment {IsFraud = false, Text = "Hi"},
                                   new Comment {IsFraud = false, Text = "Hi again"},
                                   new Comment {IsFraud = true, Text = "Win a million"},
                                   new Comment {IsFraud = true, Text = "One million dollars"}
                               };
            return comments;
        }
    }
}