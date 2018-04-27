using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace microCommerce.Setting
{
    public interface ISettingService
    {
        Task<Setting> GetSettingById(object Id);
        Task<Setting> GetSetting(string key);
        Task<T> GetSettingByKey<T>(string key, T defaultValue = default(T));
        Task<T> GetSetting<T>() where T : ISettings, new();
        Task<IList<Setting>> GetAllSettings();

        Task SaveSetting<T>(T setting) where T : ISettings, new();
        Task SaveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, bool clearCache = true) where T : ISettings, new();

        Task RemoveSetting<T>(T setting) where T : ISettings, new();
        Task RemoveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettings, new();
    }
}