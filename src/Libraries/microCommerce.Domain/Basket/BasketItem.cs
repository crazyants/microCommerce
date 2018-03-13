using System;

namespace microCommerce.Domain.Basket
{
    public class BasketItem : BaseEntity
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string AttributesData { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}