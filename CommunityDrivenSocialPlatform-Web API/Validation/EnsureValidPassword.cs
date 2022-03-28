using CDSP_API.Data;
using CDSP_API.misc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CDSP_API.Validation
{
    public class EnsureValidPassword : BaseValidation
    {
        public override ValidationResult _Validate(object value, ValidationContext validationContext, DataContext dataContext)
        {
            if(value.ToString().Length>= 6 && value.ToString().Length <= 255)
            {
                return ValidationResult.Success;
            }
            
            return new ValidationResult(ApiConstant.Identity.InvalidPassword);
        }
    }
}
