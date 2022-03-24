using CommunityDrivenSocialPlatform_APi.Validaton;
using System.ComponentModel.DataAnnotations;

namespace CommunityDrivenSocialPlatform_APi.Model.Request
{
    public class SubThreadDetailRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string WelcomeMessage { get; set; }
    }
}
