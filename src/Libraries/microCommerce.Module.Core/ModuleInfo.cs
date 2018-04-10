using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace microCommerce.Module.Core
{
    public class ModuleInfo: IComparable<ModuleInfo>
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