using CommunityDrivenSocialPlatform_APi.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static CommunityDrivenSocialPlatform_APi.Controllers.AuthenticateController;

namespace CommunityDrivenSocialPlatform_APi.Validaton
{
    public class SignupEnsureUniqueUsername : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            signupRequest user = validationContext.ObjectInstance as signupRequest;
            CDSPdB dbContext = (CDSPdB)validationContext.GetService(typeof(CDSPdB));

            return new UsernameUnique().validate(dbContext, user.Username);
        }
    }
}
