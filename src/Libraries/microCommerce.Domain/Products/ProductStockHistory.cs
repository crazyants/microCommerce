using System;

namespace microCommerce.Domain.Products
{
    public class ProductStockHistory : BaseEntity
    {
        /// <summary>
        /// Gets or sets the stock quantity adjustment
        /// </summary>
        public int QuantityAdjustment { get; set; }

        /// <summary>
        /// Gets or sets current stock quantity
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product attribute combination identifier
        /// </summary>
        public int? CombinationId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedDateUtc { get; set; }
    }
}