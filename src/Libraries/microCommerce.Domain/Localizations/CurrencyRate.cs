using System;

namespace microCommerce.Domain.Globalization
{
    public class CurrencyRate : BaseEntity
    {
        /// <summary>
        /// Creates a new instance of the ExchangeRate class
        /// </summary>
        public CurrencyRate()
        {
            CurrencyCode = string.Empty;
            Rate = 1.0m;
        }

        /// <summary>
        /// The three letter ISO code for the Exchange Rate, e.g. USD
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// The conversion rate of this currency from the base currency
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// When was this exchange rate updated from the data source (the internet data xml feed)
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Format the rate into a string with the currency code, e.g. "USD 0.72543"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", CurrencyCode, Rate);
        }
    }
}