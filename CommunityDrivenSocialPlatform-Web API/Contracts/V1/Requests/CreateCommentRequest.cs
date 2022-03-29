using CDSP_API.Models;
using CDSP_API.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace CDSP_API.Contracts.V1.Requests
{
    public class CreateCommentRequest
    {
        [Required]
        public string Body { get; set; }

        public DateTime CreatedAt = DateTime.UtcNow;

        public Comment MapToModel()
        {
            return new Comment
            {
                Body = Body,
                CreatedAt = CreatedAt,
            };
        }

        public override string ToString()
        {
            return $"Body: {Body}, CreatedAt: {CreatedAt}";
        }
    }


}
