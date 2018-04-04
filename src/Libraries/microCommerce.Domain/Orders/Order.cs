using microCommerce.Domain.Shipping;
using System;

namespace microCommerce.Domain.Orders
{
    public class Order : BaseEntity
    {
        /// <summary>
        /// Gets or sets thr order number
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the billing address identifier
        /// </summary>
        public int BillingAddressId { get; set; }

        /// <summary>
        /// Gets or sets the shipping address identifier
        /// </summary>
        public int? ShippingAddressId { get; set; }

        /// <summary>
        /// Gets or sets an order status identifier
        /// </summary>
        public int OrderStatusId { get; set; }

        /// <summary>
        /// Gets or sets the shipping status identifier
        /// </summary>
        public int ShippingStatusId { get; set; }

        /// <summary>
        /// Gets or sets the payment status identifier
        /// </summary>
        public int PaymentStatusId { get; set; }

        /// <summary>
        /// Gets or sets the payment method name
        /// </summary>
        public string PaymentMethodName { get; set; }

        /// <summary>
        /// Gets or sets the shipping method name
        /// </summary>
        public string ShippingMethodName { get; set; }

        /// <summary>
        /// Gets or sets the currency rate
        /// </summary>
        public decimal CurrencyRate { get; set; }

        /// <summary>
        /// Gets or sets the currency identifier
        /// </summary>
        public string CurrencyId { get; set; }

        /// <summary>
        /// Gets or sets language identifier
        /// </summary>
        public string LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the customer ip adress
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the customer browser name
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// Gets or sets the payment transaction error message
        /// </summary>
        public string PaymentErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the date and time of order creation
        /// </summary>
        public DateTime CreatedDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the order status
        /// </summary>
        [ColumnIgnore]
        public OrderStatus OrderStatus
        {
            get
            {
                return (OrderStatus)OrderStatusId;
            }
            set
            {
                OrderStatusId = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the payment status
        /// </summary>
        [ColumnIgnore]
        public PaymentStatus PaymentStatus
        {
            get
            {
                return (PaymentStatus)PaymentStatusId;
            }
            set
            {
                PaymentStatusId = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the shipping status
        /// </summary>
        [ColumnIgnore]
        public ShippingStatus ShippingStatus
        {
            get
            {
                return (ShippingStatus)ShippingStatusId;
            }
            set
            {
                ShippingStatusId = (int)value;
            }
        }
    }
}