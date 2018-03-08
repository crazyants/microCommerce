using microCommerce.Common;
using microCommerce.Domain.Customers;
using microCommerce.Domain.Globalization;

namespace microCommerce.Mvc
{
    public class WebWorkContext : IWorkContext
    {
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

        public virtual bool IsAdmin { get; set; }
    }
}