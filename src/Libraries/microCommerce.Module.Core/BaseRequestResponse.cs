using System.Collections.Generic;

namespace microCommerce.Module.Core
{
    public abstract class BaseRequestResponse
    {
        public BaseRequestResponse()
        {
            CustomValues = new Dictionary<string, object>();
        }

        public virtual IDictionary<string, object> CustomValues { get; set; }
    }
}