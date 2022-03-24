using CommunityDrivenSocialPlatform_APi.Data;
using CommunityDrivenSocialPlatform_APi.Model.Request;
using System.ComponentModel.DataAnnotations;

namespace CommunityDrivenSocialPlatform_APi.Validaton
{
    public class UserSignupEnsureUniqueEmail : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            UpdateUserDetailsRequest user = validationContext.ObjectInstance as UpdateUserDetailsRequest;
            CDSPdB dbContext = (CDSPdB)validationContext.GetService(typeof(CDSPdB));

            return new EmailUnique().validate(dbContext, user.EmailAddress);
        }
    }
}
