namespace microCommerce.Domain.Shipping
{
    public class ShipmentItem : BaseEntity
    {
        /// <summary>
        /// Gets or sets the shipment identifier
        /// </summary>
        public int ShipmentId { get; set; }

        /// <summary>
        /// Gets or sets the order item identifier
        /// </summary>
        public int OrderItemId { get; set; }

        /// <summary>
        /// Gets or sets the productId item identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the item weight
        /// </summary>
        public decimal ItemWeight { get; set; }

        /// <summary>
        /// Gets or sets the quantity
        /// </summary>
        public int Quantity { get; set; }
    }
}