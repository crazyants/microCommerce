namespace microCommerce.Domain.Products
{
    public class RelatedProductRelatedProduct : BaseEntity
    {
        /// <summary>
        /// Gets or sets the first product identifier
        /// </summary>
        public int ProductId1 { get; set; }

        /// <summary>
        /// Gets or sets the second product identifier
        /// </summary>
        public int ProductId2 { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}