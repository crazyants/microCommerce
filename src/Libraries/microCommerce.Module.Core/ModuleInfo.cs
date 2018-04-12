using microCommerce.Ioc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace microCommerce.Module.Core
{
    public class ModuleInfo : IComparable<ModuleInfo>
    {
        /// <summary>
        /// Gets or sets the friendly module name
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the module system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the module binary file name
        /// </summary>
        public string AssemblyFileName { get; set; }

        /// <summary>
        /// Gets or sets the module version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the module description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the module priority
        /// </summary>
        public int Priority { get; set; }

        [JsonIgnore]
        public bool Installed { get; set; }

        /// <summary>
        /// Gets or sets the module assembly file info
        /// </summary>
        [JsonIgnore]
        public FileInfo AssemblyFileInfo { get; set; }

        /// <summary>
        /// Gets or sets the module assembly
        /// </summary>
        [JsonIgnore]
        public Assembly Assembly { get; set; }

        /// <summary>
        /// Gets or sets the module type
        /// </summary>
        [JsonIgnore]
        public Type ModuleType { get; set; }

        public virtual T Instance<T>() where T : class, IModule
        {
            object instance = null;
            try
            {
                instance = EngineContext.Current.Resolve(ModuleType);
            }
            catch { }

            if (instance == null)
                instance = EngineContext.Current.ResolveUnregistered(ModuleType);

            var typedInstance = instance as T;
            if (typedInstance != null)
                typedInstance.ModuleInfo = this;

            return typedInstance;
        }

        /// <summary>
        /// Compares this instance with a specified ModuleInfo object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual int CompareTo(ModuleInfo otherModule)
        {
            if (Priority != otherModule.Priority)
                return Priority.CompareTo(otherModule.Priority);

            return FriendlyName.CompareTo(otherModule.FriendlyName);
        }

        /// <summary>
        /// Determines whether this instance and another specified ModuleInfo object have the same SystemName
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Equals(object value)
        {
            return SystemName.Equals((value as ModuleInfo).SystemName);
        }

        /// <summary>
        /// Returns the hash code for this ModuleInfo
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return SystemName.GetHashCode();
        }

        /// <summary>
        /// Returns the module as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FriendlyName;
        }
    }
}