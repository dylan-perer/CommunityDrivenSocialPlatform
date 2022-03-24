using System;
using System.ComponentModel.DataAnnotations;

namespace CommunityDrivenSocialPlatform_APi.Model.Request
{
    public class CreateCommentRequest
    {
        [Required(ErrorMessage = "Sorry, comment body cannot be empty.")]
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
