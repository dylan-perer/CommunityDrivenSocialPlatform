using CDSP_API.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CDSP_API.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public abstract class BaseValidation :ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return null;

            DataContext DataContext = (DataContext)validationContext.GetService(typeof(DataContext));

            return _Validate(value, validationContext, DataContext);
        }

        public abstract ValidationResult _Validate(object value, ValidationContext validationContext, DataContext dataContext);
    }
}
