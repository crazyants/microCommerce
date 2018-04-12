using System.Collections.Generic;

namespace microCommerce.Module.Core
{
    public interface IModuleLoader
    {
        /// <summary>
        /// Loads the modules
        /// </summary>
        /// <param name="loadOnlyInstalledModules"></param>
        /// <returns></returns>
        IList<T> LoadModules<T>(bool loadOnlyInstalledModules = true, string category = null) where T : class, IModule;

        /// <summary>
        /// Load module infos
        /// </summary>
        /// <param name="loadOnlyInstalledModules"></param>
        /// <returns></returns>
        IList<ModuleInfo> LoadModules(bool loadOnlyInstalledModules = true, string category = null);

        /// <summary>
        /// Reload the modules to assembly side
        /// </summary>
        void ReloadModules();
    }
}