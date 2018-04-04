namespace microCommerce.Domain.Products
{
    public class ProductCategoryMapping : BaseEntity
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
    }
}