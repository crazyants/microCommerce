using microCommerce.Common;
using microCommerce.MediaApi.Services;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        #region Methods
        [HttpGet]
        [Route("/pictures/{Id:int}")]
        public virtual IActionResult Get(int Id)
        {
            return Json(_pictureService.GetPictureById(Id));
        }

        [HttpPost]
        [Route("/pictures/upload")]
        public virtual async Task<IActionResult> Upload()
        {
            if (Request.Form == null || !Request.Form.Files.Any())
                return Content("File not selected");

            var file = Request.Form.Files[0];
            var fileBinary = new byte[file.Length];
            using (MemoryStream stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                fileBinary = stream.ToArray();
            }

            string fileName = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName);
            string contentType = MimeTypeMap.GetMimeType(fileExtension);
            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            //insert picture
            var picture = _pictureService.InsertPicture(fileBinary, contentType, null);

            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                pictureUrl = _pictureService.GetPictureUrl(picture)
            });
        }
        #endregion
    }
}