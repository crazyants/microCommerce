using microCommerce.Dapper;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace microCommerce.ProductApi.Controllers
{
    public class ProductController : ServiceBaseController
    {
        private readonly IDataContext _dataContext;

        public ProductController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        [HttpGet("/products")]
        public virtual async Task<IActionResult> ProductSearch(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return await Task.FromResult(Json(null));
        }
        
        [HttpGet("/products/{categoryId:int}")]
        public virtual async Task<IActionResult> CategoryProducts(int categoryId)
        {
            return await Task.FromResult(Json(null));
        }
        
        [HttpGet("/products/homepage")]
        public virtual async Task<IActionResult> HomePageProducts()
        {
            return await Task.FromResult(Json(null));
        }
        
        [HttpGet("/products/detail/{Id:int}")]
        public virtual async Task<IActionResult> ProductDetail(int Id)
        {
            return await Task.FromResult(Json(null));
        }
        
        [HttpGet("/products/related/{Id:int}")]
        public virtual async Task<IActionResult> RelatedProducts(int Id)
        {
            return await Task.FromResult(Json(null));
        }
    }
}