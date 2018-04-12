namespace microCommerce.Module.Core.Payments
{
    public class PaymentConfirmRequest : BaseRequestResponse
    {
        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the order total value
        /// </summary>
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// Gets or sets the payment method system name
        /// </summary>
        public string PaymentMethodSystemName { get; set; }

        #region Credit Card Properties
        /// <summary>
        /// Gets or sets a credit card type (Visa, Master Card, etc...). We leave it empty if not used by a payment gateway
        /// </summary>
        public string CreditCardType { get; set; }

        /// <summary>
        /// Gets or set the credit card association(Platinium, Business etc...)
        /// </summary>
        public string CreditCardDegree { get; set; }

        /// <summary>
        /// Gets or set the credit card bank name
        /// </summary>
        public string CreditCardBankName { get; set; }

        /// <summary>
        /// Gets or sets a credit card owner name
        /// </summary>
        public string CreditCardName { get; set; }

        /// <summary>
        /// Gets or sets a credit card number
        /// </summary>
        public string CreditCardNumber { get; set; }

        /// <summary>
        /// Gets or sets a credit card expire year
        /// </summary>
        public string CreditCardExpireYear { get; set; }

        /// <summary>
        /// Gets or sets a credit card expire month
        /// </summary>
        public string CreditCardExpireMonth { get; set; }

        /// <summary>
        /// Gets or sets a credit card CVV2 (Card Verification Value)
        /// </summary>
        public string CreditCardCvv2 { get; set; }

        /// <summary>
        /// Gets or sets the installment count(3 month, 6 month etc..)
        /// </summary>
        public int InstallmentCount { get; set; }

        /// <summary>
        /// Gets or sets the installment rate(just like 1.1421)
        /// </summary>
        public decimal InstallmentRate { get; set; }
        #endregion
    }
}