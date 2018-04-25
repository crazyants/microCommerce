namespace microCommerce.Ioc
{
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="context">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        void Register(DependencyContext context);

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        int Priority { get; }
    }
}