using microCommerce.Common;
using microCommerce.Ioc;
using microCommerce.Localization;
using System;
using System.ComponentModel.DataAnnotations;

namespace microCommerce.Mvc.Helpers
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum value)
        {
            if (value == null)
                return string.Empty;

            if (!value.GetType().IsEnum && value.GetType().GetMember(value.ToString()).Length == 0)
                throw new ArgumentException(string.Format("Type '{0}' is not Enum", value.GetType()));

            object[] customAttributes = value.GetType().GetMember(value.ToString())[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            if (customAttributes.Length == 0)
                throw new ArgumentException(string.Format("'{0}.{1}' doesn't have DisplayAttribute", value.GetType().Name, value));

            var displayAttribute = customAttributes[0] as DisplayAttribute;

            return displayAttribute?.GetName();
        }
        
        public static string GetLocalizedName(this Enum value)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return value.GetLocalizedName(workContext.CurrentLanguage.LanguageCulture);
        }

        public static string GetLocalizedName(this Enum value, string cultureCode)
        {
            var localization = EngineContext.Current.Resolve<ILocalizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            string displayName = value.GetDisplayName();

            return localization.GetResourceValue(displayName, workContext.CurrentLanguage.LanguageCulture, displayName).Result;
        }
    }
}