using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace microCommerce.Mvc.Extensions
{
    public static class SessionExtensions
    {
        /// <summary>
        /// Set value to Session
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Get value from session
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value == null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}