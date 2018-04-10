using Dapper;
using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Dapper;
using microCommerce.Domain.Media;
using microCommerce.Logging;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;

namespace microCommerce.MediaApi.Services
{
    public class PictureService : IPictureService
    {
        #region Contants
        private const string PICTURE_FIND_BY_ID_KEY = "PICTURE_FINDBYID_KEY_{0}";
        #endregion

        #region Fields
        private readonly ICacheManager _cacheManager;
        private readonly IDataContext _dataContext;
        private readonly IWebHelper _webHelper;
        private readonly ILogger _logger;
        #endregion

        #region Ctor
        public PictureService(ICacheManager cacheManager,
            IDataContext dataContext,
            IWebHelper webHelper,
            ILogger logger)
        {
            _cacheManager = cacheManager;
            _dataContext = dataContext;
            _webHelper = webHelper;
            _logger = logger;
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Loads a picture from file
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="mimeType">MIME type</param>
        /// <returns>Picture binary</returns>
        protected virtual byte[] LoadPictureFromFile(int pictureId, string mimeType)
        {
            string lastPart = GetFileExtensionFromMimeType(mimeType);
            string fileName = string.Format("{0}_0.{1}", pictureId.ToString("0000000"), lastPart);
            var filePath = GetPictureLocalPath(fileName);

            if (!File.Exists(filePath))
                return new byte[0];

            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// Returns the file extension from mime type.
        /// </summary>
        /// <param name="mimeType">Mime type</param>
        /// <returns>File extension</returns>
        protected virtual string GetFileExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
                return null;

            //also see System.Web.MimeMapping for more mime types
            string[] parts = mimeType.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }

            return lastPart;
        }

        /// <summary>
        /// Delete picture thumbs
        /// </summary>
        /// <param name="picture">Picture</param>
        protected virtual void DeletePictureThumbs(Picture picture)
        {
            string filter = string.Format("{0}*.*", picture.Id.ToString("0000000"));
            var thumbDirectoryPath = CommonHelper.MapContentPath("~/content/images/thumbs");
            string[] currentFiles = Directory.GetFiles(thumbDirectoryPath, filter, SearchOption.AllDirectories);
            foreach (string currentFileName in currentFiles)
            {
                var thumbFilePath = GetThumbLocalPath(currentFileName);
                File.Delete(thumbFilePath);
            }
        }

        /// <summary>
        /// Get picture (thumb) local path
        /// </summary>
        /// <param name="thumbFileName">Filename</param>
        /// <returns>Local picture thumb path</returns>
        protected virtual string GetThumbLocalPath(string thumbFileName)
        {
            var thumbsDirectoryPath = CommonHelper.MapContentPath("~/content/images/thumbs");
            var thumbFilePath = Path.Combine(thumbsDirectoryPath, thumbFileName);

            return thumbFilePath;
        }

        /// <summary>
        /// Get picture local path. Used when images stored on file system (not in the database)
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <returns>Local picture path</returns>
        protected virtual string GetPictureLocalPath(string fileName)
        {
            return Path.Combine(CommonHelper.MapContentPath("~/content/images/"), fileName);
        }

        /// <summary>
        /// Calculates picture dimensions whilst maintaining aspect
        /// </summary>
        /// <param name="originalSize">The original picture size</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="resizeType">Resize type</param>
        /// <param name="ensureSizePositive">A value indicatingh whether we should ensure that size values are positive</param>
        /// <returns></returns>
        protected virtual Size CalculateDimensions(Size originalSize, int targetSize,
            ResizeType resizeType = ResizeType.LongestSide, bool ensureSizePositive = true)
        {
            float width, height;

            switch (resizeType)
            {
                case ResizeType.LongestSide:
                    if (originalSize.Height > originalSize.Width)
                    {
                        // portrait
                        width = originalSize.Width * (targetSize / (float)originalSize.Height);
                        height = targetSize;
                    }
                    else
                    {
                        // landscape or square
                        width = targetSize;
                        height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    }
                    break;
                case ResizeType.Width:
                    width = targetSize;
                    height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    break;
                case ResizeType.Height:
                    width = originalSize.Width * (targetSize / (float)originalSize.Height);
                    height = targetSize;
                    break;
                default:
                    throw new Exception("Not supported ResizeType");
            }

            if (ensureSizePositive)
            {
                if (width < 1)
                    width = 1;
                if (height < 1)
                    height = 1;
            }

            //we invoke Math.Round to ensure that no white background is rendered
            return new Size((int)Math.Round(width), (int)Math.Round(height));
        }

        /// <summary>
        /// Save picture on file system
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="pictureBinary">Picture binary</param>
        /// <param name="mimeType">MIME type</param>
        protected virtual void SavePictureInFile(int pictureId, byte[] pictureBinary, string mimeType)
        {
            string lastPart = GetFileExtensionFromMimeType(mimeType);
            string fileName = string.Format("{0}_0.{1}", pictureId.ToString("0000000"), lastPart);
            string filePath = GetPictureLocalPath(fileName);
            File.WriteAllBytes(filePath, pictureBinary);
        }

        /// <summary>
        /// Delete a picture on file system
        /// </summary>
        /// <param name="picture">Picture</param>
        protected virtual void DeletePictureOnFileSystem(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            string lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string fileName = string.Format("{0}_0.{1}", picture.Id.ToString("0000000"), lastPart);
            string filePath = GetPictureLocalPath(fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        /// <summary>
        /// Save a value indicating whether some file (thumb) already exists
        /// </summary>
        /// <param name="thumbFilePath">Thumb file path</param>
        /// <param name="thumbFileName">Thumb file name</param>
        /// <param name="mimeType">MIME type</param>
        /// <param name="binary">Picture binary</param>
        protected virtual void SaveThumb(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary)
        {
            File.WriteAllBytes(thumbFilePath, binary);
        }

        /// <summary>
        /// Get picture (thumb) URL
        /// </summary>
        /// <param name="thumbFileName">Filename</param>
        /// <returns>Local picture thumb path</returns>
        protected virtual string GetThumbUrl(string thumbFileName)
        {
            return "content/images/thumbs/" + thumbFileName;
        }


        /// <summary>
        /// Gets the default picture URL
        /// </summary>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <returns>Picture URL</returns>
        public virtual string GetDefaultPictureUrl(int targetSize = 0)
        {
            string defaultImageFileName = "default-image.png";
            string filePath = GetPictureLocalPath(defaultImageFileName);

            if (!File.Exists(filePath))
                return string.Empty;

            if (targetSize == 0)
            {
                return _webHelper.GetCurrentLocation() + "content/images/" + defaultImageFileName;
            }
            else
            {
                string fileExtension = Path.GetExtension(filePath);
                string thumbFileName = string.Format("{0}_{1}{2}",
                    Path.GetFileNameWithoutExtension(filePath),
                    targetSize,
                    fileExtension);

                var thumbFilePath = GetThumbLocalPath(thumbFileName);
                if (!File.Exists(thumbFilePath))
                {
                    using (var b = new Bitmap(filePath))
                    {
                        using (var destStream = new MemoryStream())
                        {
                            var newSize = CalculateDimensions(b.Size, targetSize);
                            Image destinationImage = new Bitmap(newSize.Width, newSize.Height);
                            Graphics g = Graphics.FromImage(destinationImage);
                            g.DrawImage(b,
                                new Rectangle(0, 0, newSize.Width, newSize.Height),
                                0,
                                0,
                                newSize.Width,
                                newSize.Height,
                                GraphicsUnit.Pixel);
                            destinationImage.Save(destStream, ImageFormat.Jpeg);

                            var destBinary = destStream.ToArray();
                            SaveThumb(thumbFilePath, thumbFileName, "", destBinary);
                        }
                    }
                }

                return _webHelper.GetCurrentLocation() + GetThumbUrl(thumbFileName);
            }
        }
        #endregion

        /// <summary>
        /// Gets the loaded picture binary depending on picture storage settings
        /// </summary>
        /// <param name="picture">Picture</param>
        /// <returns>Picture binary</returns>
        public virtual byte[] LoadPictureBinary(Picture picture)
        {
            return LoadPictureFromFile(picture.Id, picture.MimeType);
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <returns>Picture URL</returns>
        public virtual string GetPictureUrl(int pictureId,
            int targetSize = 0,
            bool showDefaultPicture = true)
        {
            var picture = GetPictureById(pictureId);
            return GetPictureUrl(picture, targetSize, showDefaultPicture);
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <returns>Picture URL</returns>
        public virtual string GetPictureUrl(Picture picture,
            int targetSize = 0,
            bool showDefaultPicture = true)
        {
            string url = string.Empty;
            byte[] pictureBinary = null;
            if (picture != null)
                pictureBinary = LoadPictureBinary(picture);

            if (picture == null || pictureBinary == null || pictureBinary.Length == 0)
            {
                if (showDefaultPicture)
                    return GetDefaultPictureUrl(targetSize);
            }

            var seoFileName = picture.SeoFilename;
            string lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string thumbFileName;
            if (targetSize == 0)
            {
                thumbFileName = !string.IsNullOrEmpty(seoFileName)
                    ? string.Format("{0}_{1}.{2}", picture.Id.ToString("0000000"), seoFileName, lastPart)
                    : string.Format("{0}.{1}", picture.Id.ToString("0000000"), lastPart);
            }
            else
            {
                thumbFileName = !string.IsNullOrEmpty(seoFileName)
                    ? string.Format("{0}_{1}_{2}.{3}", picture.Id.ToString("0000000"), seoFileName, targetSize, lastPart)
                    : string.Format("{0}_{1}.{2}", picture.Id.ToString("0000000"), targetSize, lastPart);
            }

            string thumbFilePath = GetThumbLocalPath(thumbFileName);

            //the named mutex helps to avoid creating the same files in different threads,
            //and does not decrease performance significantly, because the code is blocked only for the specific file.
            using (var mutex = new Mutex(false, thumbFileName))
            {
                if (!File.Exists(thumbFilePath))
                {
                    mutex.WaitOne();

                    //check, if the file was created, while we were waiting for the release of the mutex.
                    if (!File.Exists(thumbFilePath))
                    {
                        byte[] pictureBinaryResized;

                        //resizing required
                        if (targetSize != 0)
                        {
                            using (var stream = new MemoryStream(pictureBinary))
                            {
                                Bitmap b = null;
                                try
                                {
                                    //try-catch to ensure that picture binary is really OK.
                                    //Otherwise, we can get "Parameter is not valid" exception if binary is corrupted for some reasons
                                    b = new Bitmap(stream);
                                }
                                catch (ArgumentException exc)
                                {
                                    _logger.Error(string.Format("Error generating picture thumb. ID={0}", picture.Id), exc);
                                }

                                //bitmap could not be loaded for some reasons
                                if (b == null)
                                    return url;

                                using (var destStream = new MemoryStream())
                                {
                                    var newSize = CalculateDimensions(b.Size, targetSize);
                                    Image destinationImage = new Bitmap(newSize.Width, newSize.Height);
                                    Graphics g = Graphics.FromImage(destinationImage);
                                    g.DrawImage(b,
                                        new Rectangle(0, 0, newSize.Width, newSize.Height),
                                        0,
                                        0,
                                        newSize.Width,
                                        newSize.Height,
                                        GraphicsUnit.Pixel);

                                    destinationImage.Save(destStream, ImageFormat.Jpeg);

                                    pictureBinaryResized = destStream.ToArray();
                                    b.Dispose();
                                }
                            }
                        }
                        else
                        {
                            //create a copy of pictureBinary
                            pictureBinaryResized = pictureBinary.ToArray();
                        }

                        SaveThumb(thumbFilePath, thumbFileName, picture.MimeType, pictureBinaryResized);
                    }

                    mutex.ReleaseMutex();
                }
            }
            url = _webHelper.GetCurrentLocation() + GetThumbUrl(thumbFileName);

            return url;
        }

        /// <summary>
        /// Get a picture local path
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <returns></returns>
        public virtual string GetThumbLocalPath(Picture picture,
            int targetSize = 0,
            bool showDefaultPicture = true)
        {
            string url = GetPictureUrl(picture, targetSize, showDefaultPicture);
            if (String.IsNullOrEmpty(url))
                return String.Empty;

            return GetThumbLocalPath(Path.GetFileName(url));
        }

        /// <summary>
        /// Gets a picture
        /// </summary>
        /// <param name="pictureId">Picture identifier</param>
        /// <returns>Picture</returns>
        public virtual Picture GetPictureById(int pictureId)
        {
            if (pictureId == 0)
                return null;

            return _cacheManager.Get(string.Format(PICTURE_FIND_BY_ID_KEY, pictureId), () =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("pictureId", pictureId, DbType.Int32);
                return _dataContext.FirstProcedure<Picture>("Picture_FindById", parameters);
            });
        }

        /// <summary>
        /// Gets a collection of pictures
        /// </summary>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Items on each page</param>
        /// <returns>Paged list of pictures</returns>
        public virtual IPagedList<Picture> GetPictures(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var parameters = new DynamicParameters();
            parameters.Add("pageIndex", pageIndex, DbType.Int32);
            parameters.Add("pageSize", pageSize, DbType.Int32);
            parameters.Add("totalRecords", pageSize, DbType.Int32, ParameterDirection.Output);

            var query = _dataContext.QueryProcedure<Picture>("Picture_Search", parameters);
            return new PagedList<Picture>(query, pageIndex, pageSize, parameters.Get<int>("totalRecords"));
        }

        /// <summary>
        /// Inserts a picture
        /// </summary>
        /// <param name="pictureBinary">The picture binary</param>
        /// <param name="mimeType">The picture MIME type</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <param name="altAttribute">"alt" attribute for "img" HTML element</param>
        /// <param name="titleAttribute">"title" attribute for "img" HTML element</param>
        /// <param name="isNew">A value indicating whether the picture is new</param>
        /// <param name="validateBinary">A value indicating whether to validated provided picture binary</param>
        /// <returns>Picture</returns>
        public virtual Picture InsertPicture(byte[] pictureBinary,
            string mimeType,
            string seoFilename,
            string altAttribute = null,
            string titleAttribute = null,
            bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            if (validateBinary)
                pictureBinary = ValidatePicture(pictureBinary, mimeType);

            var picture = new Picture
            {
                MimeType = mimeType,
                SeoFilename = seoFilename,
                AltAttribute = altAttribute,
                TitleAttribute = titleAttribute
            };

            //insert
            _dataContext.Insert(picture);

            //save to file
            SavePictureInFile(picture.Id, pictureBinary, mimeType);

            return picture;
        }

        /// <summary>
        /// Updates the picture
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="pictureBinary"></param>
        /// <param name="mimeType"></param>
        /// <param name="seoFilename"></param>
        /// <param name="altAttribute"></param>
        /// <param name="titleAttribute"></param>
        /// <param name="validateBinary"></param>
        /// <returns></returns>
        public virtual Picture UpdatePicture(int pictureId,
            byte[] pictureBinary,
            string mimeType,
            string seoFilename,
            string altAttribute = null,
            string titleAttribute = null,
            bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            if (validateBinary)
                pictureBinary = ValidatePicture(pictureBinary, mimeType);

            var picture = GetPictureById(pictureId);
            if (picture == null)
                return null;

            //delete old thumbs if a picture has been changed
            if (seoFilename != picture.SeoFilename)
                DeletePictureThumbs(picture);

            picture.MimeType = mimeType;
            picture.SeoFilename = seoFilename;
            picture.AltAttribute = altAttribute;
            picture.TitleAttribute = titleAttribute;

            //update
            _dataContext.Update(picture);

            //save to file
            SavePictureInFile(picture.Id, pictureBinary, mimeType);

            return picture;
        }

        /// <summary>
        /// Deletes a picture
        /// </summary>
        /// <param name="picture">Picture</param>
        public virtual void DeletePicture(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            //delete thumbs
            DeletePictureThumbs(picture);

            //delete from file
            DeletePictureOnFileSystem(picture);

            //delete from database
            _dataContext.Delete(picture);
        }

        /// <summary>
        /// Updates a SEO filename of a picture
        /// </summary>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <returns>Picture</returns>
        public virtual Picture SetSeoFilename(int pictureId, string seoFilename)
        {
            var picture = GetPictureById(pictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");

            //update if it has been changed
            if (seoFilename != picture.SeoFilename)
            {
                //update picture
                picture = UpdatePicture(picture.Id,
                    LoadPictureBinary(picture),
                    picture.MimeType,
                    seoFilename,
                    picture.AltAttribute,
                    picture.TitleAttribute,
                    false);
            }

            return picture;
        }

        /// <summary>
        /// Validates input picture dimensions
        /// </summary>
        /// <param name="pictureBinary">Picture binary</param>
        /// <param name="mimeType">MIME type</param>
        /// <returns>Picture binary or throws an exception</returns>
        public virtual byte[] ValidatePicture(byte[] pictureBinary, string mimeType)
        {
            //using (var destStream = new MemoryStream())
            //{
            //    ImageBuilder.Current.Build(pictureBinary, destStream, new ResizeSettings
            //    {
            //        MaxWidth = _mediaSettings.MaximumImageSize,
            //        MaxHeight = _mediaSettings.MaximumImageSize,
            //        Quality = _mediaSettings.DefaultImageQuality
            //    });
            //    return destStream.ToArray();
            //}

            return pictureBinary;
        }
    }
}