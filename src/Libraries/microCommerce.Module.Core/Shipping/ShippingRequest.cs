using microCommerce.Domain.Common;
using microCommerce.Domain.Customers;
using microCommerce.Domain.Shipping;
using System;
using System.Collections.Generic;

namespace microCommerce.Module.Core.Shipping
{
    public class ShippingRequest : BaseRequestResponse
    {
        public ShippingRequest()
        {
            Items = new List<ShippingItem>();
        }

        public string BillingNo { get; set; }

        public string OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public Customer Customer { get; set; }

        public decimal OrderSubtotalPrice { get; set; }

        public decimal OrderTotalPrice { get; set; }

        public Address BillingAddress { get; set; }

        public Address ShippingAddress { get; set; }

        public IList<ShippingItem> Items { get; set; }

        public string CurrencyCode { get; set; }

        public ShippingStatus ShippingStatus { get; set; }

        public string PaymentMethodSystemName { get; set; }

        #region Nested Class
        public class ShippingItem : BaseRequestResponse
        {
            public string ProductSKU { get; set; }

            public int ProductId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public string UnitPrice { get; set; }

            public decimal UnitPriceValue { get; set; }

            public string TotalPrice { get; set; }

            public decimal TotalPriceValue { get; set; }

            public int Quantity { get; set; }

            public decimal Weight { get; set; }

            public string Barcode { get; set; }

            public string HSCode { get; set; }
        }
        #endregion
    }
}