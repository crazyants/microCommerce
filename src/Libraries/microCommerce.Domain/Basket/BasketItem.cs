using System;

namespace microCommerce.Domain.Basket
{
    public class BasketItem : BaseEntity
    {
        public int BasketItemTypeId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string AttributesData { get; set; }
        public int Quantity { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public DateTime CreatedDateUtc { get; set; }

        [ColumnIgnore]
        public BasketItemType BasketItemType
        {
            get { return (BasketItemType)BasketItemTypeId; }
            set { BasketItemTypeId = (int)value; }
        }
    }
}