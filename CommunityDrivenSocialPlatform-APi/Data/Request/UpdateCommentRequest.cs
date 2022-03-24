using System.ComponentModel.DataAnnotations;

namespace CommunityDrivenSocialPlatform_APi.Model.Request
{
    public class UpdateCommentRequest
    {
        [Required(ErrorMessage = "Sorry, comment id is required.")]
        [RegularExpression("[1-9]+", ErrorMessage = "Sorry id must be a number and cannot be 0.")]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Sorry, comment body cannot be empty.")]
        public string Body { get; set; }
    }
}
