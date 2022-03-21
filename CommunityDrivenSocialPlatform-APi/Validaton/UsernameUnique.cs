using CommunityDrivenSocialPlatform_APi.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CommunityDrivenSocialPlatform_APi.Validaton
{
    public class UsernameUnique
    {
        public ValidationResult validate(CDSPdB dbContext, string username)
        {
            if (dbContext.User.FirstOrDefault(r => r.Username == username) == null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"Sorry, {username} is already in use. Please try again.");
        }
    }
}
