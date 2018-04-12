using microCommerce.Domain.Basket;
using System.Collections.Generic;

namespace microCommerce.Module.Core.Payments
{
    public interface IPaymentModule : IModule
    {
        PaymentConfirmResponse PaymentConfirm(PaymentConfirmRequest request);

        void PaymentProcess(PaymentProcessRequest request);

        decimal GetAdditionalFee(IList<BasketItem> basketItems);


    }
}