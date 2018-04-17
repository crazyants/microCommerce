using microCommerce.Domain.Customers;
using microCommerce.Domain.Globalization;

namespace microCommerce.Common
{
    public interface IWorkContext
    {
        /// <summary>
        /// Gets the current store theme
        /// </summary>
        ThemeInfo CurrentTheme { get; set; }

        /// <summary>
        /// Gets the current customer
        /// </summary>
        Customer CurrentCustomer { get; set; }

        /// <summary>
        /// Gets the current store language
        /// </summary>
        Language CurrentLanguage { get; set; }

        /// <summary>
        /// Gets the current store currency
        /// </summary>
        Currency CurrentCurrency { get; set; }

        /// <summary>
        /// Gets the current customer country
        /// </summary>
        Country CurrentCountry { get; set; }
    }
}