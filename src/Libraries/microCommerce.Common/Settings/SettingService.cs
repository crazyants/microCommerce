using microCommerce.Caching;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace microCommerce.Domain.Settings
{
    public class SettingService : ISettingService
    {
        #region Constants
        /// <summary>
        /// Key for caching
        /// </summary>
        private const string SETTINGS_ALL_KEY = "Setting.all";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string SETTINGS_PATTERN_KEY = "Setting.";
        #endregion

        #region Fields
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Ctor
        public SettingService(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        protected virtual T To<T>(object value)
        {
            return (T)To(value, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        protected virtual object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                var destinationConverter = TypeDescriptor.GetConverter(destinationType);
                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                    return destinationConverter.ConvertFrom(null, culture, value);

                var sourceConverter = TypeDescriptor.GetConverter(sourceType);
                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                    return sourceConverter.ConvertTo(null, culture, value, destinationType);

                if (destinationType.IsEnum && value is int)
                    return Enum.ToObject(destinationType, (int)value);

                if (!destinationType.IsInstanceOfType(value))
                    return Convert.ChangeType(value, destinationType, culture);
            }

            return value;
        }

        protected virtual IDictionary<string, IList<SettingForCaching>> GetAllSettingsCached()
        {
            var key = string.Format(SETTINGS_ALL_KEY);
            return _cacheManager.Get(key, () =>
            {
                var dictionary = new Dictionary<string, IList<SettingForCaching>>();
                foreach (var s in settings)
                {
                    var resourceName = s.Name.ToLowerInvariant();
                    var settingForCaching = new SettingForCaching
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Value = s.Value,
                        StoreId = s.StoreId
                    };
                    if (!dictionary.ContainsKey(resourceName))
                    {
                        //first setting
                        dictionary.Add(resourceName, new List<SettingForCaching>
                        {
                            settingForCaching
                        });
                    }
                    else
                    {
                        //already added
                        //most probably it's the setting with the same name but for some certain store (storeId > 0)
                        dictionary[resourceName].Add(settingForCaching);
                    }
                }
                return dictionary;
            });
        }
        #endregion

        #region Nested classes
        /// <summary>
        /// Setting (for caching)
        /// </summary>
        [Serializable]
        public class SettingForCaching
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public int StoreId { get; set; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Load settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T LoadSetting<T>() where T : ISettings, new()
        {
            return (T)LoadSetting(typeof(T));
        }

        /// <summary>
        /// Load settings by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual ISettings LoadSetting(Type type)
        {
            var settings = Activator.CreateInstance(type);

            foreach (var prop in type.GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = type.Name + "." + prop.Name;
                var setting = GetSettingByKey<string>(key);
                if (setting == null)
                    continue;
                
                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
                    continue;

                var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);

                //set property
                prop.SetValue(settings, value, null);
            }

            return settings as ISettings;
        }

        /// <summary>
        /// Get settings by key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual T GetSettingByKey<T>(string key, T defaultValue = default(T))
        {
            if (string.IsNullOrEmpty(key))
                return defaultValue;

            var settings = GetAllSettingsCached();
            key = key.Trim().ToLowerInvariant();
            if (settings.ContainsKey(key))
            {
                var setting = settings[key];
                if (setting != null)
                    return To<T>(setting.Value);
            }

            return defaultValue;
        }
        #endregion
    }
}
