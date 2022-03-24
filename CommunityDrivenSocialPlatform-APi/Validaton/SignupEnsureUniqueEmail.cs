using CommunityDrivenSocialPlatform_APi.Data;
using CommunityDrivenSocialPlatform_APi.Model.Requests;
using System.ComponentModel.DataAnnotations;

namespace CommunityDrivenSocialPlatform_APi.Validaton
{
    public class SignupEnsureUniqueEmail : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            SignupRequest user = validationContext.ObjectInstance as SignupRequest;
            CDSPdB dbContext = (CDSPdB)validationContext.GetService(typeof(CDSPdB));

            return new EmailUnique().validate(dbContext, user.EmailAddress);
        }
    }
}
