namespace microCommerce.Domain.Products
{
    public class ProductTagMapping : BaseEntity
    {
        public int ProductId { get; set; }
        public int ProductTagId { get; set; }
    }
}