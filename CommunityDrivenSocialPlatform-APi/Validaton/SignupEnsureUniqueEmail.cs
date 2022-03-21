using CommunityDrivenSocialPlatform_APi.Data;
using CommunityDrivenSocialPlatform_APi.Model;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static CommunityDrivenSocialPlatform_APi.Controllers.AuthenticateController;

namespace CommunityDrivenSocialPlatform_APi.Validaton
{
    public class SignupEnsureUniqueEmail : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            signupRequest user = validationContext.ObjectInstance as signupRequest;
            CDSPdB dbContext = (CDSPdB)validationContext.GetService(typeof(CDSPdB));

            return new EmailUnique().validate(dbContext, user.EmailAddress);
        }
    }
}
