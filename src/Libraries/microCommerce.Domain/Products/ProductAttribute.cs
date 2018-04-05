namespace microCommerce.Domain.Products
{
    public class ProductAttribute : BaseEntity
    {
        /// <summary>
        /// Gets or sets the attribute name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the attribute description
        /// </summary>
        public string Description { get; set; }
    }
}