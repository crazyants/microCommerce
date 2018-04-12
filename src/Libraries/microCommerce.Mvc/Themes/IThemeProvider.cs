using microCommerce.Common;
using System.Collections.Generic;

namespace microCommerce.Mvc.Themes
{
    public interface IThemeProvider
    {
        /// <summary>
        /// Load all themes
        /// </summary>
        IList<ThemeInfo> LoadThemes();

        /// <summary>
        /// Check whether the theme with specified name exists
        /// </summary>
        /// <param name="name"></param>
        bool ThemeExists(string name);
    }
}