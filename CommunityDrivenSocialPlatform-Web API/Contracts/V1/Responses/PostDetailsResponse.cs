using CDSP_API.Models;
using System;
using System.Collections.Generic;

namespace CDSP_API.Contracts.V1.Responses
{
    public class PostDetailsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int AuthorId { get; set; }
        public int SubThreadId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int VoteCount { get; set; } = 0;
        public List<CommentDetailsResponse> Comments { get; set; } = new List<CommentDetailsResponse>();

        public PostDetailsResponse MapToReponse(Post post)
        {
            foreach(var comment in post.Comment)
            {
                CommentDetailsResponse commentDetailsResponse = new CommentDetailsResponse().MapToReponse(comment);
                Comments.Add(commentDetailsResponse);
            }
            
            return new PostDetailsResponse
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                AuthorId = post.AuthorId,
                SubThreadId = post.SubThreadId,
                CreatedAt = post.CreatedAt,
                Comments = Comments,
            };
        }

        public List<PostDetailsResponse> MapToReponse(List<Post> posts)
        {
            List<PostDetailsResponse> postDetailsResponses= new List<PostDetailsResponse>();

            foreach (var post in posts)
            {
                postDetailsResponses.Add(new PostDetailsResponse().MapToReponse(post));
            }
            return postDetailsResponses;
        }
    }
}
