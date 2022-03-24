using CommunityDrivenSocialPlatform_APi.Data;
using CommunityDrivenSocialPlatform_APi.Model.Request;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static CommunityDrivenSocialPlatform_APi.Controllers.SubthreadController;

namespace CommunityDrivenSocialPlatform_APi.Validaton.Subthread
{
    public class EnsureUniqueSubthreadName : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            CreateSubThreadRequest subThreadRequest = validationContext.ObjectInstance as CreateSubThreadRequest;
            CDSPdB dbContext = (CDSPdB)validationContext.GetService(typeof(CDSPdB));

            if (dbContext.SubThread.FirstOrDefault(r => r.Name == subThreadRequest.Name) == null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"Sorry, a subthread with a name {subThreadRequest.Name} is already exists. Please try a different name.");
        }
    }
}
