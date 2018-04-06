using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace microCommerce.Mvc.Models
{
    public abstract class BaseModel
    {
        public virtual void BindModel(ModelBindingContext bindingContext)
        {
        }
    }
}