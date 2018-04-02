using microCommerce.MediaApi.Services;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.MediaApi.Controllers
{
    [Route("/pictures")]
    public class PictureController : ServiceBaseController
    {
        #region Fields
        private readonly IPictureService _pictureService;
        #endregion

        #region Ctor
        public PictureController(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }
        #endregion

        public virtual IActionResult Get()
        {
            return Json(_pictureService.GetPictures());
        }
    }
}