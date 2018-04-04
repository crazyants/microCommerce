using System;

namespace microCommerce.Domain.Shipping
{
    public class Shipment : BaseEntity
    {
        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the tracking number of this shipment
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Gets or sets the billing number
        /// </summary>
        public string BillingNumber { get; set; }

        /// <summary>
        /// Gets or sets the barcode url
        /// </summary>
        public string BarcodeUrl { get; set; }

        /// <summary>
        /// Gets or sets the document file path(proforma, shipment, barcode, invoice etc..)
        /// </summary>
        public string DocumentFileUrl { get; set; }

        /// <summary>
        /// Gets or sets the total weight of this shipment
        /// </summary>
        public decimal TotalWeight { get; set; }

        /// <summary>
        /// Gets or sets the shipped date and time
        /// </summary>
        public DateTime? ShippedDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the delivery date and time
        /// </summary>
        public DateTime? DeliveryDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets the entity creation date
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
    }
}