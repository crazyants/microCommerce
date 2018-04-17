using microCommerce.Common;
using System.Collections.Generic;

namespace microCommerce.Mvc.Themes
{
    public interface IThemeProvider
    {
        /// <summary>
        /// Load theme info by unique theme name
        /// </summary>
        /// <param name="themeName">unique theme name</param>
        /// <returns></returns>
        ThemeInfo LoadThemeInfo(string themeName);

        /// <summary>
        /// Load all themes
        /// </summary>
        IList<ThemeInfo> LoadThemes();

        /// <summary>
        /// Check whether the theme with specified name exists
        /// </summary>
        /// <param name="themeName">unique theme name</param>
        bool ThemeExists(string themeName);
    }
}