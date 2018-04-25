using Dapper;
using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Dapper;
using microCommerce.Domain.Media;
using microCommerce.Setting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace microCommerce.Services.Media
{
    public class PictureService : IPictureService
    {
        #region Constants
        private const string ThumbFolderPath = "~/content/images/thumbs";
        private const string ThumbFolder = "content/images/thumbs";

        private const string PictureFolderPath = "~/content/images";
        private const string PictureFolder = "content/images";

        private const string PictureFileNameFormat = "{0:0000000}_0{1}";
        private const string DefaultPictureFileName = "default-image.jpg";

        private const string PICTURE_FIND_BY_ID_KEY = "PICTURE_FINDBYID_KEY_{0}";
        #endregion

        #region Fields
        private readonly IDataContext _dataContext;
        private readonly IWebHelper _webHelper;
        private readonly ICacheManager _cacheManager;

        private readonly MediaSettings _mediaSettings;
        #endregion

        #region Ctor
        public PictureService(IDataContext dataContext,
            IWebHelper webHelper,
            ICacheManager cacheManager,
            MediaSettings mediaSettings)
        {
            _dataContext = dataContext;
            _webHelper = webHelper;
            _cacheManager = cacheManager;

            _mediaSettings = mediaSettings;
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Gets the loaded picture binary
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        protected virtual byte[] LoadPictureFromFile(int pictureId, string mimeType)
        {
            string fileName = string.Format(PictureFileNameFormat, pictureId, MimeTypeMap.GetExtension(mimeType));
            var filePath = GetPictureLocalPath(fileName);
            if (!File.Exists(filePath))
                return new byte[0];

            return File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// Save picture binary to file system
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="pictureBinary"></param>
        /// <param name="mimeType"></param>
        protected virtual void SavePictureToFile(int pictureId, byte[] pictureBinary, string mimeType)
        {
            //ensure content/images directory exists
            var picturesDirectoryPath = CommonHelper.MapContentPath(PictureFolderPath);
            if (!Directory.Exists(picturesDirectoryPath))
                Directory.CreateDirectory(picturesDirectoryPath);

            string fileName = string.Format(PictureFileNameFormat, pictureId, MimeTypeMap.GetExtension(mimeType));
            var filePath = GetPictureLocalPath(fileName);

            File.WriteAllBytes(filePath, pictureBinary);
        }

        /// <summary>
        /// Save picture thumbs to file system
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="pictureBinary"></param>
        protected virtual void SaveThumbToFile(string filePath, byte[] pictureBinary)
        {
            //ensure content/images/thumbs directory exists
            var thumbsDirectoryPath = CommonHelper.MapContentPath(ThumbFolderPath);
            if (!Directory.Exists(thumbsDirectoryPath))
                Directory.CreateDirectory(thumbsDirectoryPath);

            File.WriteAllBytes(filePath, pictureBinary);
        }

        /// <summary>
        /// Delete picture thumbs
        /// </summary>
        /// <param name="picture">Picture</param>
        protected virtual void DeletePictureThumbs(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

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
        /// Delete picture on file system
        /// </summary>
        /// <param name="picture"></param>
        protected virtual void DeletePictureOnFile(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            string fileName = string.Format(PictureFileNameFormat, picture.Id, MimeTypeMap.GetExtension(picture.MimeType));
            var filePath = GetPictureLocalPath(fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        /// <summary>
        /// Get picture local path
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected virtual string GetPictureLocalPath(string fileName)
        {
            return Path.Combine(CommonHelper.MapContentPath(PictureFolderPath), fileName);
        }

        /// <summary>
        /// Get picture (thumb) local path
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <returns>Local picture thumb path</returns>
        protected virtual string GetThumbLocalPath(string fileName)
        {
            return Path.Combine(CommonHelper.MapContentPath(ThumbFolderPath), fileName);
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
            float width = 0;
            float height = 0;

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

            return new Size((int)Math.Round(width), (int)Math.Round(height));
        }

        /// <summary>
        /// Validates input picture dimensions
        /// </summary>
        /// <param name="pictureBinary">Picture binary</param>
        /// <param name="mimeType">MIME type</param>
        /// <returns>Picture binary or throws an exception</returns>
        protected virtual byte[] ValidatePicture(byte[] pictureBinary, string mimeType)
        {
            using (Image<Rgba32> b = Image.Load(pictureBinary))
            {
                using (var destStream = new MemoryStream())
                {
                    var options = new ResizeOptions
                    {
                        Size = new SixLabors.Primitives.Size(_mediaSettings.MaximumPictureSize, _mediaSettings.MaximumPictureSize),
                        Mode = ResizeMode.Max
                    };
                    b.Mutate(x => x.Resize(options));

                    if (ImageFormats.Jpeg.MimeTypes.Contains(mimeType))
                    {
                        b.Save(destStream, ImageFormats.Jpeg);
                    }
                    else if (ImageFormats.Png.MimeTypes.Contains(mimeType))
                    {
                        b.Save(destStream, ImageFormats.Png);
                    }
                    else if (ImageFormats.Gif.MimeTypes.Contains(mimeType))
                    {
                        b.Save(destStream, ImageFormats.Gif);
                    }
                    else if (ImageFormats.Bmp.MimeTypes.Contains(mimeType))
                    {
                        b.Save(destStream, ImageFormats.Bmp);
                    }

                    return destStream.ToArray();
                }
            }
        }
        #endregion

        #region Get Picture Urls
        /// <summary>
        /// Gets the default picture URL
        /// </summary>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="defaultPictureType">Default picture type</param>
        /// <returns>Picture URL</returns>
        public virtual string GetDefaultPictureUrl(int targetSize = 0)
        {
            string filePath = GetPictureLocalPath(DefaultPictureFileName);
            if (!File.Exists(filePath))
                return string.Empty;

            if (targetSize == 0)
            {
                return string.Format("{0}{1}/{2}",
                    _webHelper.GetCurrentLocation(),
                    PictureFolder,
                    DefaultPictureFileName);
            }

            string fileExtension = Path.GetExtension(filePath);
            string thumbFileName = string.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(filePath), targetSize, fileExtension);
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
                        using (Image<Rgba32> b = Image.Load(filePath))
                        {
                            using (var ms = new MemoryStream())
                            {
                                var newSize = CalculateDimensions(new Size(b.Width, b.Height), targetSize);
                                var options = new ResizeOptions
                                {
                                    Size = new SixLabors.Primitives.Size(newSize.Width, newSize.Height),
                                    Mode = ResizeMode.Pad
                                };

                                b.Mutate(x => x.Resize(options));
                                b.Save(ms, ImageFormats.Jpeg);

                                //save to file
                                SaveThumbToFile(thumbFilePath, ms.ToArray());
                            }
                        }
                    }

                    mutex.ReleaseMutex();
                }
            }

            return string.Format("{0}{1}/{2}",
                    _webHelper.GetCurrentLocation(),
                    ThumbFolder,
                    thumbFileName);
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
            byte[] pictureBinary = null;
            if (picture != null)
                pictureBinary = LoadPictureFromFile(picture.Id, picture.MimeType);

            if (picture == null || pictureBinary == null || pictureBinary.Length == 0)
            {
                if (showDefaultPicture)
                    return GetDefaultPictureUrl(targetSize);
            }

            string seoFileName = picture.SeoFilename;
            string lastPart = MimeTypeMap.GetExtension(picture.MimeType);
            string thumbFileName;
            if (targetSize == 0)
            {
                if (string.IsNullOrEmpty(seoFileName))
                    thumbFileName = string.Format("{0:0000000}{1}", picture.Id, lastPart);
                else
                    thumbFileName = string.Format("{0:0000000}_{1}{2}", picture.Id, seoFileName, lastPart);
            }
            else
            {

                if (string.IsNullOrEmpty(seoFileName))
                    thumbFileName = string.Format("{0:0000000}_{1}{2}", picture.Id, targetSize, lastPart);
                else
                    thumbFileName = string.Format("{0:0000000}_{1}_{2}{3}", picture.Id, seoFileName, targetSize, lastPart);

                thumbFileName = !String.IsNullOrEmpty(seoFileName)
                    ? $"{picture.Id.ToString("0000000")}_{seoFileName}_{targetSize}.{lastPart}"
                    : $"{picture.Id.ToString("0000000")}_{targetSize}.{lastPart}";
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
                        if (targetSize == 0)
                        {
                            pictureBinaryResized = pictureBinary;
                        }
                        else
                        {
                            using (Image<Rgba32> b = Image.Load(pictureBinary))
                            {
                                using (var ms = new MemoryStream())
                                {
                                    var newSize = CalculateDimensions(new Size(b.Width, b.Height), targetSize);
                                    var options = new ResizeOptions
                                    {
                                        Size = new SixLabors.Primitives.Size(newSize.Width, newSize.Height),
                                        Mode = ResizeMode.Pad
                                    };

                                    b.Mutate(x => x.Resize(options));
                                    b.Save(ms, ImageFormats.Jpeg);
                                    pictureBinaryResized = ms.ToArray();
                                }
                            }
                        }

                        //save to file
                        SaveThumbToFile(thumbFilePath, pictureBinaryResized);
                    }

                    mutex.ReleaseMutex();
                }
            }

            return string.Format("{0}{1}/{2}",
                    _webHelper.GetCurrentLocation(),
                    ThumbFolder,
                    thumbFileName);
        }
        #endregion

        #region Picture Crud Operations

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
            SavePictureToFile(picture.Id, pictureBinary, mimeType);

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
            SavePictureToFile(picture.Id, pictureBinary, mimeType);

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
            DeletePictureOnFile(picture);

            //delete from database
            _dataContext.Delete(picture);
        }
        #endregion
    }
}