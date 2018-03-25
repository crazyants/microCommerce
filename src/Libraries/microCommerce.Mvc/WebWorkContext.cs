using microCommerce.Common;
using microCommerce.Domain.Customers;
using microCommerce.Domain.Globalization;
using microCommerce.Mvc.UI;

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
                return null;
            }
            set
            {

            }
        }

        public virtual Language CurrentLanguage
        {
            get
            {
                return null;
            }
            set
            {

            }
        }

        public virtual Currency CurrentCurrency
        {
            get
            {
                return null;
            }
            set
            {

            }
        }
    }
}