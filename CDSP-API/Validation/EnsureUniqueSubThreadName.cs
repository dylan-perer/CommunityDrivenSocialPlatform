using CDSP_API.Data;
using CDSP_API.misc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CDSP_API.Validation
{
    public class EnsureUniqueSubThreadName : BaseValidation
    {
        public override ValidationResult _Validate(object value, ValidationContext validationContext, DataContext dataContext)
        {
            if (dataContext.SubThread.SingleOrDefault(r => r.Name == value as string) == null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ApiConstant.SubThread.SubThreadNameAreadyInUse);
        }
    }
}
