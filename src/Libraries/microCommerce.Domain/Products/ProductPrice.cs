namespace microCommerce.Domain.Products
{
    public class ProductPrice : BaseEntity
    {
        /// <summary>
        /// Gets or sets the product price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the currency identifier
        /// </summary>
        public int CurrencyId { get; set; }
    }
}