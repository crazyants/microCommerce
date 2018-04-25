using microCommerce.Dapper;
using microCommerce.Domain.Basket;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.BasketApi.Controllers
{
    public class BasketController : ServiceBaseController
    {
        private readonly IDataContext _dataContext;
        
        public BasketController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        [HttpGet("/basket/items")]
        public virtual IActionResult GetItems()
        {
            var items = _dataContext.Query<BasketItem>("select * from BasketItem");
            return Json(items);
        }
    }
}