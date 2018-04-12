namespace microCommerce.Module.Core.Shipping
{
    public interface IShippingMethod : IModule
    {
        ShippingMethodInfo GetShippingInfo();

        ShippingResponse CreateShipment(ShippingRequest shippingRequest);

        ShippingResponse DeleteShipment(ShippingRequest shippingRequest);

        IShippingTracker ShippingTracker { get; }
    }
}