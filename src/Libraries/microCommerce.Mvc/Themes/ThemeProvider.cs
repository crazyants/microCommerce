using microCommerce.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace microCommerce.Mvc.Themes
{
    public class ThemeProvider : IThemeProvider
    {
        #region Fields
        /// <summary>
        /// Returns a collection of all loaded themes
        /// </summary>
        private IList<ThemeInfo> _themes;

        /// <summary>
        /// Gets the path to themes folder
        /// </summary>
        private const string ThemesPath = "~/Themes";

        /// <summary>
        /// Gets the name of the theme info file
        /// </summary>
        private const string ThemeInfoFileName = "theme.json";

        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();
        #endregion

        #region Methods
        /// <summary>
        /// Load theme info by unique theme name
        /// </summary>
        /// <param name="themeName">unique theme name</param>
        /// <returns></returns>
        public virtual ThemeInfo LoadThemeInfo(string themeName)
        {
            if (string.IsNullOrEmpty(themeName))
                return null;

            return LoadThemes().FirstOrDefault(t => t.ThemeName.Equals(themeName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Load all themes
        /// </summary>
        /// <param name="loadOnlyInstalledModules"></param>
        /// <returns></returns>
        public virtual IList<ThemeInfo> LoadThemes()
        {
            if (_themes == null)
            {
                using (new WriteLockDisposable(Locker))
                {
                    //load all theme descriptors
                    var themeFolder = new DirectoryInfo(CommonHelper.MapRootPath(ThemesPath));
                    _themes = new List<ThemeInfo>();
                    foreach (var themeInfoFile in themeFolder.GetFiles(ThemeInfoFileName, SearchOption.AllDirectories))
                    {
                        var text = File.ReadAllText(themeInfoFile.FullName);
                        if (string.IsNullOrEmpty(text))
                            continue;

                        //deserialize module information file to ModuleInfo object
                        var themeInfo = JsonConvert.DeserializeObject<ThemeInfo>(File.ReadAllText(themeInfoFile.FullName));

                        //validation
                        if (string.IsNullOrEmpty(themeInfo?.ThemeName))
                            throw new Exception($"A theme info '{themeInfoFile.FullName}' has no system name");

                        _themes.Add(themeInfo);
                    }
                }
            }

            return _themes;
        }

        /// <summary>
        /// Check whether the theme with specified name exists
        /// </summary>
        /// <param name="themeName"></param>
        /// <returns></returns>
        public virtual bool ThemeExists(string themeName)
        {
            if (string.IsNullOrEmpty(themeName))
                return false;

            return LoadThemes().Any(t => t.ThemeName.Equals(themeName, StringComparison.InvariantCultureIgnoreCase));
        }
        #endregion
    }
}