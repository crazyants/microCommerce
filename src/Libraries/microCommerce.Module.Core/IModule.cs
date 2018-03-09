namespace microCommerce.Module.Core
{
    public interface IModule
    {
        /// <summary>
        /// Gets or sets the module info
        /// </summary>
        ModuleInfo ModuleInfo { get; set; }

        /// <summary>
        /// Install module
        /// </summary>
        void Install();

        /// <summary>
        /// Uninstall module
        /// </summary>
        void Uninstall();
    }
}