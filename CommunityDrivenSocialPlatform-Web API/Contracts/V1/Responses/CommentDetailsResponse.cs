using CDSP_API.Models;
using System.Collections.Generic;

namespace CDSP_API.Contracts.V1.Responses
{
    public class CommentDetailsResponse
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; }
        public string Body { get; set; }

        public CommentDetailsResponse MapToReponse(Comment comment)
        {
            return new CommentDetailsResponse
            {
                Id = comment.Id,
                PostId = comment.PostId,
                AuthorId = comment.UserId,
                Body = comment.Body,
            };
        }

        public List<CommentDetailsResponse> MapToReponse(List<Comment> comments)
        {
            List<CommentDetailsResponse> commentDetailsResponses = new List<CommentDetailsResponse>();

            foreach (var comment in comments)
            {
                commentDetailsResponses.Add(new CommentDetailsResponse().MapToReponse(comment));
            }
            return commentDetailsResponses;
        }
    }


   
}
