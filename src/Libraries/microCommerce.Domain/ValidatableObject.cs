using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace microCommerce.Domain
{
    /// <summary>
    /// Entity object validation
    /// </summary>
    public abstract class ValidatableObject
    {
        /// <summary>
        /// Validate object
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid()
        {
            return ValidationResults().Count == 0;
        }

        /// <summary>
        /// Gets the validation results
        /// </summary>
        /// <returns></returns>
        public virtual IList<ValidationResult> ValidationResults()
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this, null, null), validationResults, true);
            return validationResults;
        }
    }
}