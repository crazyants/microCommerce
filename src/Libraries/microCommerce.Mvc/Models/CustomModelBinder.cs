using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;

namespace microCommerce.Mvc.Models
{
    public class CustomModelBinder : ComplexTypeModelBinder
    {
        public CustomModelBinder(IDictionary<ModelMetadata, IModelBinder> propertyBinders)
            : base(propertyBinders)
        {
        }

        /// <summary>
        /// Create model for given binding context
        /// </summary>
        /// <param name="bindingContext">Model binding context</param>
        /// <returns>Model</returns>
        protected override object CreateModel(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            //create base model
            var model = base.CreateModel(bindingContext);

            //add custom model binding
            if (model is BaseModel)
                (model as BaseModel).BindModel(bindingContext);

            return model;
        }

        /// <summary>
        ///  Updates a property in the current model
        /// </summary>
        /// <param name="bindingContext">Model binding context</param>
        /// <param name="modelName">The model name</param>
        /// <param name="propertyMetadata">The model metadata for the property to set</param>
        /// <param name="bindingResult">The binding result for the property's new value</param>
        protected override void SetProperty(ModelBindingContext bindingContext, string modelName,
            ModelMetadata propertyMetadata, ModelBindingResult bindingResult)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            //trim property string values for nop models
            string valueAsString = bindingResult.Model as string;
            if (bindingContext.Model is BaseModel && !string.IsNullOrEmpty(valueAsString))
                bindingResult = ModelBindingResult.Success(valueAsString.Trim());

            base.SetProperty(bindingContext, modelName, propertyMetadata, bindingResult);
        }
    }
}