using microCommerce.Domain.Basket;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace microCommerce.Module.Core.Payments
{
    public interface IPaymentModule : IModule
    {
        PaymentConfirmResponse PaymentConfirm(PaymentConfirmRequest request);

        void PaymentProcess(PaymentProcessRequest request);

        IList<string> ValidateForm(IFormCollection form);

        PaymentConfirmRequest GetPaymentConfirmRequest(IFormCollection form);

        decimal GetAdditionalFee(IList<BasketItem> basketItems);

        string GetViewComponent();

        string GetConfigureUrl();
    }
}