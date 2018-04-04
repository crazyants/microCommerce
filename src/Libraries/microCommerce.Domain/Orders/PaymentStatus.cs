namespace microCommerce.Domain.Orders
{
    public enum PaymentStatus
    {
        Pending = 10,
        Authorized = 20,
        Paid = 30,
        PartiallyRefunded = 35,
        Refunded = 40,
        Voided = 50
    }
}