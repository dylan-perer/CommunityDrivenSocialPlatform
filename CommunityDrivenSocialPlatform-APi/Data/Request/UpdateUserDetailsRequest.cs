using CommunityDrivenSocialPlatform_APi.Validaton;
using System.ComponentModel.DataAnnotations;

namespace CommunityDrivenSocialPlatform_APi.Model.Request
{
    public class UpdateUserDetailsRequest
    {
        public string Description { get; set; }
        public string ProfilePictureUrl { get; set; }
        [UserSignupEnsureUniqueEmail]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
