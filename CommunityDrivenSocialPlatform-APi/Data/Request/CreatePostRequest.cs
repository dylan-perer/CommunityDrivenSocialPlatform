using System;
using System.ComponentModel.DataAnnotations;

namespace CommunityDrivenSocialPlatform_APi.Model.Request
{
    public class CreatePostRequest
    {
        [Required(ErrorMessage = "Sorry, post title is required.")]
        [MinLength(3, ErrorMessage = "Sorry, post title must be atleast 3 characters long.")]
        [MaxLength(255, ErrorMessage = "Sorry, post title must not be longer than 255 characters long.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Sorry, post body is required.")]
        [MinLength(3, ErrorMessage = "Sorry, post body must be atleast 3 characters long.")]
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Id { get; set; }
    }
}
