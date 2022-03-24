using System.ComponentModel.DataAnnotations;

namespace CommunityDrivenSocialPlatform_APi.Model.Request
{
    public class UpdatePostRequest
    {
        [Required(ErrorMessage = "Sorry, post id is required.")]
        [RegularExpression("[1-9]+", ErrorMessage = "Sorry id must be a number and cannot be 0.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Sorry, post title is required.")]
        [MinLength(3, ErrorMessage = "Sorry, post title must be atleast 3 characters long.")]
        [MaxLength(255, ErrorMessage = "Sorry, post title must not be longer than 255 characters long.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Sorry, post body is required.")]
        [MinLength(3, ErrorMessage = "Sorry, post body must be atleast 3 characters long.")]
        public string Body { get; set; }
    }

}
