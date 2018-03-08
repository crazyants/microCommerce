using microCommerce.Domain.Customers;
using microCommerce.Domain.Globalization;

namespace microCommerce.Common
{
    public interface IWorkContext
    {
        Customer CurrentCustomer { get; set; }
        Language CurrentLanguage { get; set; }
        Currency CurrentCurrency { get; set; }
        bool IsAdmin { get; set; }
    }
}