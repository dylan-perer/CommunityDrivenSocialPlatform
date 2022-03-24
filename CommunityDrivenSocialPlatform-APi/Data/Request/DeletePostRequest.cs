using System.ComponentModel.DataAnnotations;

namespace CommunityDrivenSocialPlatform_APi.Model.Request
{
    public class DeletePostRequest
    {
        [Required(ErrorMessage = "Sorry, post id is required.")]
        public int? Id { get; set; }
    }
}
