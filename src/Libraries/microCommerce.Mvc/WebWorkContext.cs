using microCommerce.Common;
using microCommerce.Domain.Customers;
using microCommerce.Domain.Globalization;

namespace microCommerce.Mvc
{
    public class WebWorkContext : IWorkContext
    {
        public virtual ThemeInfo CurrentTheme
        {
            get
            {
                return new ThemeInfo { Name = "Default" };
            }
            set
            {

            }
        }

        public virtual Customer CurrentCustomer
        {
            get
            {
                return new Customer { FirstName = "Sefa", LastName = "Can", Email = "fsefacan@gmail.com", UserName = "sefacan" };
            }
            set
            {

            }
        }

        public virtual Language CurrentLanguage
        {
            get
            {
                return new Language { LanguageCulture = "tr-TR", UniqueSeoCode = "tr" };
            }
            set
            {

            }
        }

        public virtual Currency CurrentCurrency
        {
            get
            {
                return new Currency { CurrencyCode = "TRY", Name = "Türk Lirasý" };
            }
            set
            {

            }
        }
    }
}