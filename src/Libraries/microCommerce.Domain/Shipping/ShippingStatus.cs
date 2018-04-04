namespace microCommerce.Domain.Shipping
{
    public enum ShippingStatus
    {
        ShippingNotRequired = 10,
        NotYetShipped = 20,
        PartiallyShipped = 25,
        Shipped = 30,
        Delivered = 40
    }
}