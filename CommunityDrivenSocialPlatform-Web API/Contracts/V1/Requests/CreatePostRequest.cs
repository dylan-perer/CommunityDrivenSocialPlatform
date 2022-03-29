using CDSP_API.Models;
using CDSP_API.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace CDSP_API.Contracts.V1.Requests
{
    public class CreatePostRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime CreatedAt = DateTime.UtcNow;

        public Post MapToModel()
        {
            return new Post
            {
                Title = Title,
                Body = Body,   
                CreatedAt = CreatedAt,  
            };
        }

        public override string ToString()
        {
            return $"Title: {Title}, Body: {Body}, CreatedAt: {CreatedAt}";
        }
    }


}
