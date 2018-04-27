using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.MongoDb;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace microCommerce.Setting
{
    public class SettingService : ISettingService
    {
        #region Contants
        private const string SETTINGS_ALL_KEY = "SETTINGS_ALL_KEY";
        private const string SETTINGS_PATTERN_KEY = "SETTINGS_";
        #endregion

        #region Fields
        private readonly IMongoRepository<Setting> _settingRepository;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Ctor
        public SettingService(IMongoRepository<Setting> settingRepository,
            ICacheManager cacheManager)
        {
            _settingRepository = settingRepository;
			_cacheManager = cacheManager;
        }
        #endregion

        #region Nested classes
        [Serializable]
        public class SettingForCaching
        {
            public ObjectId Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }
        #endregion

        #region Utilities
        protected virtual async Task<IDictionary<string, IList<SettingForCaching>>> GetAllSettingsCached()
        {
            //cache
            var settings = _cacheManager.Get(SETTINGS_ALL_KEY, () =>
             {
                 var allSettings = _settingRepository.Table.OrderBy(s => s.Name).ToArray();

                 var dictionary = new Dictionary<string, IList<SettingForCaching>>();
                 foreach (var s in allSettings)
                 {
                     var resourceName = s.Name.ToLowerInvariant();
                     var settingForCaching = new SettingForCaching
                     {
                         Id = s.Id,
                         Name = s.Name,
                         Value = s.Value
                     };

                     if (!dictionary.ContainsKey(resourceName))
                     {
                         //first setting
                         dictionary.Add(resourceName, new List<SettingForCaching>
                         {
                            settingForCaching
                         });
                     }
                     else//already added
                         dictionary[resourceName].Add(settingForCaching);
                 }

                 return dictionary;
             });

            return await Task.FromResult(settings);
        }

        protected virtual async Task InsertSetting(Setting setting, bool clearCache)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            await _settingRepository.InsertAsync(setting);

            //cache
            if (clearCache)
                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
        }

        protected virtual async Task UpdateSetting(Setting setting, bool clearCache)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            await _settingRepository.UpdateAsync(setting);

            //cache
            if (clearCache)
                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
        }

        protected virtual async Task DeleteSetting(Setting setting, bool clearCache)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            await _settingRepository.DeleteAsync(setting);

            //cache
            if (clearCache)
                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
        }

        protected virtual async Task DeleteSettings(IList<Setting> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            await _settingRepository.DeleteAsync(settings);

            //cache
                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
        }
        #endregion

        #region Methods
        public virtual async Task<Setting> GetSettingById(object Id)
        {
            if (Id ==null)
                return null;

            return await _settingRepository.FindAsync(Id);
        }

        public virtual async Task<Setting> GetSetting(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            return await _settingRepository.FindAsync(s => s.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        }

        public virtual async Task<T> GetSettingByKey<T>(string key, T defaultValue = default(T))
        {
            if (string.IsNullOrEmpty(key))
                return defaultValue;

            var settings = await GetAllSettingsCached();
            key = key.Trim().ToLowerInvariant();
            if (settings.ContainsKey(key))
            {
                var setting = settings[key].FirstOrDefault();
                if (setting != null)
                    return CommonHelper.To<T>(setting.Value);
            }

            return defaultValue;
        }

        public virtual async Task<T> GetSetting<T>() where T : ISettings, new()
        {
            var setting = await GetSetting(typeof(T));

            return (T)setting;
        }

        public virtual async Task<ISettings> GetSetting(Type type)
        {
            var settings = Activator.CreateInstance(type);

            foreach (var prop in type.GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = type.Name + "." + prop.Name;
                //load by store
                var setting = await GetSettingByKey<string>(key);
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

        public virtual async Task<IList<Setting>> GetAllSettings()
        {
            var allSettings = _settingRepository.Table.OrderBy(s => s.Name).ToList();
            return await Task.FromResult(allSettings);
        }

        public virtual async Task SaveSetting<T>(string key, T value, bool clearCache = true)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            key = key.Trim().ToLowerInvariant();
            var valueStr = TypeDescriptor.GetConverter(typeof(T)).ConvertToInvariantString(value);

            var allSettings = await GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ? allSettings[key].FirstOrDefault() : null;
            if (settingForCaching != null)
            {
                //update
                var setting = await GetSettingById(settingForCaching.Id);
                setting.Value = valueStr;
                await UpdateSetting(setting, clearCache);
            }
            else
            {
                //insert
                await InsertSetting(new Setting
                {
                    Name = key,
                    Value = valueStr
                }, clearCache);
            }
        }

        public virtual async Task SaveSetting<T>(T setting) where T : ISettings, new()
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                var key = typeof(T).Name + "." + prop.Name;
                //Duck typing is not supported in C#. That's why we're using dynamic type
                dynamic value = prop.GetValue(setting, null);
                if (value != null)
                    await SaveSetting(key, value, false);
                else
                    await SaveSetting(key, "", false);
            }
        }

        public virtual async Task SaveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, bool clearCache = true) where T : ISettings, new()
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            var key = settings.GetSettingKey(keySelector);
            //Duck typing is not supported in C#. That's why we're using dynamic type
            dynamic value = propInfo.GetValue(settings, null);
            if (value != null)
                await SaveSetting(key, value, clearCache);
            else
                await SaveSetting(key, "", clearCache);
        }

        public virtual async Task RemoveSetting<T>(T setting) where T : ISettings, new()
        {
            var settingsToDelete = new List<Setting>();
            var allSettings = await GetAllSettings();
            foreach (var prop in typeof(T).GetProperties())
            {
                var key = typeof(T).Name + "." + prop.Name;
                settingsToDelete.AddRange(allSettings.Where(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
            }

            await DeleteSettings(settingsToDelete);
        }

        public virtual async Task RemoveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettings, new()
        {
            var key = settings.GetSettingKey(keySelector);
            key = key.Trim().ToLowerInvariant();

            var allSettings = await GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ? allSettings[key].FirstOrDefault() : null;
            if (settingForCaching != null)
            {
                //delete
                var setting = await GetSettingById(settingForCaching.Id);
                await DeleteSetting(setting, true);
            }
        }
        #endregion
    }
}