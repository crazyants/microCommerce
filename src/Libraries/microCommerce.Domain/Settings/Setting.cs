namespace microCommerce.Domain.Settings
{
    public class Setting : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}