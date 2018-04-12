namespace microCommerce.Module.Core.Shipping
{
    public class ShippingResponse
    {
        public bool Success { get; set; }

        public string TrackingNumber { get; set; }

        public string CargoKey { get; set; }

        public string ErrorMessage { get; set; }

        public string StatusCode { get; set; }

        public string BarcodeUrl { get; set; }

        public string ConsignmentUrl { get; set; }

        public string DocumentFileUrl { get; set; }
    }
}