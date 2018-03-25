using microCommerce.Domain.Customers;
using microCommerce.Domain.Globalization;

namespace microCommerce.Common
{
    public interface IWorkContext
    {
        ThemeInfo CurrentTheme { get; set; }
        Customer CurrentCustomer { get; set; }
        Language CurrentLanguage { get; set; }
        Currency CurrentCurrency { get; set; }
    }
}