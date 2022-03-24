using CDSP_API.Data;
using CDSP_API.misc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CDSP_API.Validation
{
    public class EnsureUniqueEmailAddress : BaseValidation
    {
        public override ValidationResult _Validate(object value, ValidationContext validationContext, DataContext dataContext)
        {
            if (dataContext.User.SingleOrDefault(r => r.EmailAddress == value as string) == null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ApiConstant.Identity.EmailAddressAreadyInUse);
        }
    }
}
