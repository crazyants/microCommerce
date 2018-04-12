using microCommerce.Domain.Orders;
using System.Collections.Generic;
using System.Linq;

namespace microCommerce.Module.Core.Payments
{
    public class PaymentConfirmResponse : BaseRequestResponse
    {
        private PaymentStatus _paymentStatus = PaymentStatus.Pending;
        private OrderStatus _orderStatus = OrderStatus.Pending;
        public readonly IList<string> Errors;

        public PaymentConfirmResponse()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// Gets a value indicating whether request has been completed successfully
        /// </summary>
        public bool Success
        {
            get
            {
                return (!Errors.Any());
            }
        }

        /// <summary>
        /// Add error
        /// </summary>
        /// <param name="error">Error</param>
        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public PaymentStatus PaymentStatus
        {
            get
            {
                return _paymentStatus;
            }
            set
            {
                _paymentStatus = value;
            }
        }

        public OrderStatus OrderStatus
        {
            get
            {
                return _orderStatus;
            }
            set
            {
                _orderStatus = value;
            }
        }

        public string TransactionId { get; set; }
    }
}