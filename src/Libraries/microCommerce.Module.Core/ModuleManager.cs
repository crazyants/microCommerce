using microCommerce.Common;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace microCommerce.Module.Core
{
    public class ModuleManager
    {
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();

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

        /// <summary>
        /// Returns a collection of all loaded modules that have been shadow copied
        /// </summary>
        public static IList<ModuleInfo> LoadedModules { get; set; }

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

                    loadedModules.Add(moduleInfo);
                }

                LoadedModules = loadedModules;
            }
        }

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
            if (!File.Exists(filePath))
                File.WriteAllLines(filePath, new[] { "" }, Encoding.UTF8);

            var text = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<IList<string>>(text) ?? new List<string>();
        }
    }
}