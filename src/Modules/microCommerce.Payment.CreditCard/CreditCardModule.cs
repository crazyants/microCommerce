using microCommerce.Domain.Basket;
using microCommerce.Module.Core;
using microCommerce.Module.Core.Payments;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace microCommerce.Payment.CreditCard
{
    public class CreditCardModule : IPaymentModule
    {
        public virtual PaymentConfirmResponse PaymentConfirm(PaymentConfirmRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual void PaymentProcess(PaymentProcessRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual IList<string> ValidateForm(IFormCollection form)
        {
            throw new NotImplementedException();
        }

        public virtual PaymentConfirmRequest GetPaymentConfirmRequest(IFormCollection form)
        {
            throw new NotImplementedException();
        }

        public virtual decimal GetAdditionalFee(IList<BasketItem> basketItems)
        {
            throw new NotImplementedException();
        }

        public virtual string GetViewComponent()
        {
            throw new NotImplementedException();
        }

        public virtual string GetConfigureUrl()
        {
            throw new NotImplementedException();
        }

        public virtual void Install()
        {
            throw new NotImplementedException();
        }

        public virtual void Uninstall()
        {
            throw new NotImplementedException();
        }

        public virtual ModuleInfo ModuleInfo { get; set; }
    }
}