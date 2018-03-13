using microCommerce.Dapper;
using microCommerce.Domain.Basket;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.BasketApi.Controllers
{
    [Route("/basket")]
    public class BasketController : ServiceBaseController
    {
        #region Fields
        private readonly IDataContext _dataContext;
        #endregion

        #region Ctor
        public BasketController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        #endregion

        #region Methods
        [HttpGet, Route("/basket/items")]
        public virtual IActionResult GetItems()
        {
            var items = _dataContext.Query<BasketItem>("select * from BasketItem");
            return Json(items);
        }
        #endregion
    }
}