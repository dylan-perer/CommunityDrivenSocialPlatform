using CDSP_API.Models;
using CDSP_API.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace CDSP_API.Contracts.V1.Requests
{
    public class CreateSubThreadRequest
    {
        [Required]
        [EnsureUniqueSubThreadName]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string WelcomeMessage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public SubThread MapToModel()
        {
            return new SubThread
            {
                Name = Name,
                Description = Description,
                WelcomeMessage = WelcomeMessage,
                CreatedAt = CreatedAt,
            };
        }

    }
}
