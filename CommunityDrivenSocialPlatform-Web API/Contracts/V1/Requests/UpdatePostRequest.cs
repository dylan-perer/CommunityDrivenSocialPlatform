using CDSP_API.Models;
using CDSP_API.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace CDSP_API.Contracts.V1.Requests
{
    public class UpdatePostRequest
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public Post MapToModel()
        {
            var post = new Post();
            post.Id = Id;
            if (Title != null)
                post.Title = Title;
            if (Body != null)
                post.Body = Body;
            return post;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Body: {Body}";
        }
    }
}
