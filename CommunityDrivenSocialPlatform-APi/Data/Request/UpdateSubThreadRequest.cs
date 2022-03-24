using System.ComponentModel.DataAnnotations;

namespace CommunityDrivenSocialPlatform_APi.Data.Request
{
    public class UpdateSubThreadRequest
    {
        [Required(ErrorMessage = "Sorry, description is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Sorry, welcome message is required.")]
        public string WelcomeMessage { get; set; }
    }
}
