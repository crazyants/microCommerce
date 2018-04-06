using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace microCommerce.Mvc
{
    public class CustomModelBinderProvider:IModelBinderProvider
    {
        /// <summary>
        /// Creates a custom model binder based on passed context
        /// </summary>
        /// <param name="context">Model binder provider context</param>
        /// <returns>Model binder</returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            var modelType = context.Metadata.ModelType;
            if (!typeof(BaseModel).IsAssignableFrom(modelType))
                return null;

            //use CustomModelBinder as a ComplexTypeModelBinder for BaseModel
            if (context.Metadata.IsComplexType && !context.Metadata.IsCollectionType)
            {
                //create binders for all model properties
                var propertyBinders = context.Metadata.Properties
                    .ToDictionary(modelProperty => modelProperty, modelProperty => context.CreateBinder(modelProperty));

                return new CustomModelBinder(propertyBinders);
            }

            //or return null to further search for a suitable binder
            return null;
        }
    }
}