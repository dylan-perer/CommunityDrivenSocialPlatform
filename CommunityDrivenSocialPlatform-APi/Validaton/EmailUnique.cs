using CommunityDrivenSocialPlatform_APi.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CommunityDrivenSocialPlatform_APi.Validaton
{
    public class EmailUnique
    {
        public ValidationResult validate(CDSPdB dbContext, string emailAddress)
        {
            if (dbContext.User.FirstOrDefault(r => r.EmailAddress == emailAddress) == null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"Sorry, {emailAddress} is already in use. Please try again.");
        }
    }
}
