using CommunityDrivenSocialPlatform_APi.Validaton;
using CommunityDrivenSocialPlatform_APi.Validaton.Subthread;
using System.ComponentModel.DataAnnotations;

namespace CommunityDrivenSocialPlatform_APi.Model.Request
{
    public class CreateSubThreadRequest
    {
        [EnsureUniqueSubthreadName]
        [StringLength(150, ErrorMessage = "Sorry, subthread name must be less than 150 characters. Please try again.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Sorry, description is required. Please add a description and try again.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Sorry, welcome message is required. Please add a welcome message and try again.")]
        public string WelcomeMessage { get; set; }
    }
}
