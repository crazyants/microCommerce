using System.Collections.Generic;
using System.Linq;

namespace microCommerce.Module.Core
{
    public class ModuleProvider : IModuleProvider
    {
        #region Fields
        private IList<ModuleInfo> _modules;
        private bool _modulesLoaded;
        #endregion

        #region Utilities
        protected virtual void EnsureModulesLoaded()
        {
            if (!_modulesLoaded)
            {
                var loadedModules = ModuleManager.LoadedModules.ToList();
                loadedModules.Sort();
                _modules = loadedModules.ToList();

                _modulesLoaded = true;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads the modules
        /// </summary>
        /// <param name="loadOnlyInstalledModules"></param>
        /// <returns></returns>
        public virtual IList<T> LoadModules<T>(bool loadOnlyInstalledModules = true, string category = null) where T : class, IModule
        {
            var instanceType = typeof(T);

            return LoadModules(loadOnlyInstalledModules)
                .Where(m => typeof(T).IsAssignableFrom(m.ModuleType))
                .Select(m => m.Instance<T>()).ToList();
        }

        /// <summary>
        /// Load module infos
        /// </summary>
        /// <param name="loadOnlyInstalledModules"></param>
        /// <returns></returns>
        public virtual IList<ModuleInfo> LoadModules(bool loadOnlyInstalledModules = true, string category = null)
        {
            EnsureModulesLoaded();

            if (loadOnlyInstalledModules)
                return _modules.Where(m => m.Installed).ToList();

            return _modules.ToList();
        }

        /// <summary>
        /// Reload the modules to assembly side
        /// </summary>
        public virtual void ReloadModules()
        {

        }
        #endregion
    }
}