using CDSP_API.Models;
using CDSP_API.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace CDSP_API.Contracts.V1.Requests
{
    public class UpdateCommentRequest
    {
        public string Body { get; set; }

        public Comment MapToModel()
        {
            return new Comment
            {
                Body = Body,
            };
        }

        public override string ToString()
        {
            return $"Body: {Body}";
        }
    }


}
