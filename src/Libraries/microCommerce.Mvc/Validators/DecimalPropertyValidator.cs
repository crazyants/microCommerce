using FluentValidation.Validators;

namespace microCommerce.Mvc.Validators
{
    public class DecimalPropertyValidator : PropertyValidator
    {
        private readonly decimal _maxValue;
        
        public DecimalPropertyValidator(decimal maxValue) :
            base("Decimal value is out of range")
        {
            _maxValue = maxValue;
        }
        
        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (decimal.TryParse(context.PropertyValue.ToString(), out decimal value))
            {
                return value < _maxValue;
            }

            return false;
        }
    }
}