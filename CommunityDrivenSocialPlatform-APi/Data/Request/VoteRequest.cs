using System.ComponentModel.DataAnnotations;

namespace CommunityDrivenSocialPlatform_APi.Model.Request
{
    public class VoteRequest
    {
        [Required(ErrorMessage = "Sorry, post id is required.")]
        [RegularExpression("[1-9]+", ErrorMessage = "Sorry id must be a number and cannot be 0.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Sorry, post vote type is required.")]
        [RegularExpression("(1|2)", ErrorMessage = "Sorry, vote value must be '1' or '2'.")]
        public byte VoteType { get; set; }
    }
}
