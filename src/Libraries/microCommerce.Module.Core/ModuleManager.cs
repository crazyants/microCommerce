using microCommerce.Common;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace microCommerce.Module.Core
{
    public class ModuleManager
    {
        #region Fields
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

        /// <summary>
        /// Returns a collection of all loaded modules that have been shadow copied
        /// </summary>
        public static IList<ModuleInfo> LoadedModules { get; set; }
        #endregion

        #region Utilities
        private static Assembly DeployModule(ApplicationPartManager applicationPartManager, FileInfo moduleFile)
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.LoadFrom(moduleFile.FullName);
            }
            catch (FileLoadException) { throw; }

            if (assembly != null)
            {
                var assemblyPart = new AssemblyPart(assembly);
                if (!applicationPartManager.ApplicationParts.Contains(assemblyPart))
                    applicationPartManager.ApplicationParts.Add(assemblyPart);
            }

            return assembly;
        }

        private static void SaveInstalledToFile(IList<string> moduleSystemNames, string filePath)
        {
            var text = JsonConvert.SerializeObject(moduleSystemNames, Formatting.Indented);
            File.WriteAllText(filePath, text);
        }

        private static IList<string> GetInstalledFile(string filePath)
        {
            FileEnsureCreated(filePath);

            var text = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<IList<string>>(text) ?? new List<string>();
        }

        private static void FileEnsureCreated(string filePath)
        {
            if (!File.Exists(filePath))
                using (File.Create(filePath)) { }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize the module manager
        /// </summary>
        /// <param name="applicationPartManager"></param>
        public static void Initialize(ApplicationPartManager applicationPartManager)
        {
            if (applicationPartManager == null)
                throw new ArgumentNullException(nameof(applicationPartManager));

            using (new WriteLockDisposable(Locker))
            {
                var loadedModules = new List<ModuleInfo>();

                //gets the module folder info
                var moduleFolder = new DirectoryInfo(CommonHelper.MapRootPath(ModulesPath));

                //gets the module.json files
                var moduleInfoFiles = moduleFolder.GetFiles(ModuleInfoFileName, SearchOption.AllDirectories).ToList();

                //gets the installed module system names
                var installedModules = GetInstalledFile(CommonHelper.MapRootPath(InstalledModuleFilePath));

                foreach (var moduleInfoFile in moduleInfoFiles)
                {
                    //deserialize module information file to ModuleInfo object
                    var moduleInfo = JsonConvert.DeserializeObject<ModuleInfo>(File.ReadAllText(moduleInfoFile.FullName));

                    //gets the module dll files
                    var moduleBinaryFiles = moduleInfoFile.Directory.GetFiles("*.dll", SearchOption.AllDirectories);

                    //gets the main binary file
                    var mainModuleFile = moduleBinaryFiles.FirstOrDefault(x => x.Name.Equals(moduleInfo.AssemblyFileName, StringComparison.InvariantCultureIgnoreCase));

                    //set the installed
                    moduleInfo.Installed = installedModules.Any(x => x.Equals(moduleInfo.SystemName, StringComparison.InvariantCultureIgnoreCase));

                    //set the module assembly
                    moduleInfo.Assembly = DeployModule(applicationPartManager, mainModuleFile);
                    moduleInfo.AssemblyFileInfo = mainModuleFile;

                    //exclude the main module in module binary files
                    var readyToDeployModules = moduleBinaryFiles
                        .Where(m => !m.Name.Equals(mainModuleFile.Name, StringComparison.InvariantCultureIgnoreCase));

                    //deploy the module files
                    foreach (var moduleFileInfo in readyToDeployModules)
                        DeployModule(applicationPartManager, moduleFileInfo);

                    var type = moduleInfo.Assembly.GetTypes().FirstOrDefault(t => typeof(IModule).IsAssignableFrom(t));
                    if (!type.IsInterface && !type.IsAbstract && type.IsClass)
                        moduleInfo.ModuleType = type;

                    loadedModules.Add(moduleInfo);
                }

                LoadedModules = loadedModules;
            }
        }

        public static void MarkAsInstalled(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
                throw new ArgumentNullException(nameof(systemName));

            string filePath = CommonHelper.MapRootPath(InstalledModuleFilePath);
            FileEnsureCreated(filePath);

            //gets the installed module system names
            var installedModules = GetInstalledFile(filePath);

            var alreadyInstalled = installedModules.Any(m => m.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
            if (!alreadyInstalled)
                installedModules.Add(systemName);

            SaveInstalledToFile(installedModules, filePath);
        }

        public static void MarkAsUninstalled(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
                throw new ArgumentNullException(nameof(systemName));

            string filePath = CommonHelper.MapRootPath(InstalledModuleFilePath);
            FileEnsureCreated(filePath);

            //gets the installed module system names
            var installedModules = GetInstalledFile(filePath);

            var alreadyInstalled = installedModules.Any(m => m.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
            if (alreadyInstalled)
                installedModules.Remove(systemName);

            SaveInstalledToFile(installedModules, filePath);
        }

        public static void SaveModuleInfoFile(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
                throw new ArgumentException(nameof(moduleInfo));

            //get the description file path
            if (moduleInfo.AssemblyFileInfo == null)
                throw new Exception($"Cannot load assembly path for {moduleInfo.SystemName} module.");

            var filePath = Path.Combine(moduleInfo.AssemblyFileInfo.Directory.FullName, ModuleInfoFileName);
            if (!File.Exists(filePath))
                throw new Exception($"ModuleInfo file for {moduleInfo.SystemName} module does not exist. {filePath}");

            //save the file
            var text = JsonConvert.SerializeObject(moduleInfo, Formatting.Indented);
            File.WriteAllText(filePath, text);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the modules directory path
        /// </summary>
        public const string ModulesPath = "~/Modules";

        /// <summary>
        /// Gets the installed module file path
        /// </summary>
        public const string InstalledModuleFilePath = "~/Modules/InstalledModules.json";

        /// <summary>
        /// Gets the module manifest file
        /// </summary>
        public const string ModuleInfoFileName = "module.json";

        #endregion
    }
}