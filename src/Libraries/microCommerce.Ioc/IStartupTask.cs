namespace microCommerce.Ioc
{
    public interface IStartupTask
    {
        void Execute();
        int Priority { get; }
    }
}