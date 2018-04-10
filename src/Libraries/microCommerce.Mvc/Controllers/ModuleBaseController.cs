using microCommerce.Module.Core;

namespace microCommerce.Mvc.Controllers
{
    public class ModuleBaseController : BaseController
    {
        // <summary>
        /// Gets or sets the module info
        /// </summary>
        public ModuleInfo ModuleInfo { get; set; }
    }
}