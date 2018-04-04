using System;

namespace microCommerce.Domain.Customers
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int BillingAddressId { get; set; }
        public int ShippingAddressId { get; set; }
        public string LastIpAddress { get; set; }
        public DateTime? LastActivityDateUtc { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}