namespace microCommerce.Dapper
{
    public class DatabaseConfig
    {
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
        public int ExecutionTimeout { get; set; }
    }
}