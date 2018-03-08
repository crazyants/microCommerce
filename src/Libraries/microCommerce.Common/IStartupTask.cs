namespace microCommerce.Common
{
    public interface IStartupTask 
    {
        /// <summary>
        /// Executes a task
        /// </summary>
        void Execute();

        /// <summary>
        /// Gets order of this startup task implementation
        /// </summary>
        int Priority { get; }
    }
}