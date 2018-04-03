using System;

namespace microCommerce.Domain.Settings
{
    public interface ISettingService
    {
        /// <summary>
        /// Load settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T LoadSetting<T>() where T : ISettings, new();

        /// <summary>
        /// Load settings by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ISettings LoadSetting(Type type);

        /// <summary>
        /// Get settings by key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        T GetSettingByKey<T>(string key, T defaultValue = default(T));
    }
}