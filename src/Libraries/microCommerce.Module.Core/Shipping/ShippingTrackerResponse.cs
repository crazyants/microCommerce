using System;
using System.Collections.Generic;

namespace microCommerce.Module.Core.Shipping
{
    public class ShippingTrackerResponse
    {
        public ShippingTrackerResponse()
        {
            TrackingEvents = new List<ShippingTrackingEvent>();
        }

        public bool Delivered { get; set; }

        public DateTime? DeliveryDateUtc { get; set; }

        public IList<ShippingTrackingEvent> TrackingEvents { get; set; }

        #region Nested Class
        public class ShippingTrackingEvent
        {
            public string EventName { get; set; }

            public string Location { get; set; }

            public DateTime? EventDate { get; set; }
        }
        #endregion
    }
}