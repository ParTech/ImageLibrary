using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Linq;
using Castle.Core.Logging;
using ParTech.ImageLibrary.Core.Classes;
using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Interfaces;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.Repositories
{
    public interface IImageRepository: IRepository
    {
        int CountImagesForProduct(int productid);

        Image GetImage(int imageId);

        IEnumerable<Image> GetImages();

        IEnumerable<Image> GetImagesForUser(int userid);

        ImageTypeEnum GetImageTypeEnum(ImageFormat rawImageFormat);

        bool SaveImage(Image image);
    }

    public class ImageRepository : IImageRepository
    {
        public ILogger Logger { get; set; }

        public int CountImagesForProduct(int productid)
        {
            var count = 0;

            try
            {
                using (var db = new Entities())
                {
                    count = db.Images.Count(i => i.ProductID == productid);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("CountImagesForProduct - error [{0}] - \r\n {1} \r\n\r\n \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return count;
        }

        public Image GetImage(int imageId)
        {
            Image image = null;

            try
            {
                using (var db = new Entities())
                {
                    image = db.Images.SingleOrDefault(i => i.ImageID == imageId);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetImage - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return image;
        }

        public IEnumerable<Image> GetImages()
        {
            var images = new List<Image>();

            try
            {
                using (var db = new Entities())
                {
                    images = db.Images.Include("Product")
                                      .OrderBy(i => i.Product.Name)
                                      .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetImages - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return images;
        }

        public IEnumerable<Image> GetImagesForUser(int userid)
        {
            var images = new List<Image>();

            try
            {
                using (var db = new Entities())
                {
                    images = db.Images.Where(i => i.Product.UserID == userid)
                                      .Include("Product")
                                      .OrderBy(i => i.Product.Name)
                                      .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetImagesForUser - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return images;
        }

        public ImageTypeEnum GetImageTypeEnum(ImageFormat rawImageFormat)
        {
            if (rawImageFormat.Guid == BitmapProperties.ImageFormatBmp)
            {
                return ImageTypeEnum.Bmp;
            }
            
            if (rawImageFormat.Guid == BitmapProperties.ImageFormatEmf)
            {
                return ImageTypeEnum.Emf;
            }
            
            if (rawImageFormat.Guid == BitmapProperties.ImageFormatExif)
            {
                return ImageTypeEnum.Exif;
            }
            
            if (rawImageFormat.Guid == BitmapProperties.ImageFormatGif)
            {
                return ImageTypeEnum.Gif;
            }
            
            if (rawImageFormat.Guid == BitmapProperties.ImageFormatIcon)
            {
                return ImageTypeEnum.Icon;
            }
            
            if (rawImageFormat.Guid == BitmapProperties.ImageFormatJpeg)
            {
                return ImageTypeEnum.Jpeg;
            }
            
            if (rawImageFormat.Guid == BitmapProperties.ImageFormatMemoryBmp)
            {
                return ImageTypeEnum.MemoryBmp;
            }
            
            if (rawImageFormat.Guid == BitmapProperties.ImageFormatPng)
            {
                return ImageTypeEnum.Png;
            }
            
            if (rawImageFormat.Guid == BitmapProperties.ImageFormatTiff)
            {
                return ImageTypeEnum.Tiff;
            }
            
            if (rawImageFormat.Guid == BitmapProperties.ImageFormatWmf)
            {
                return ImageTypeEnum.Wmf;
            }

            return ImageTypeEnum.Undefined;
        }
        
        public bool SaveImage(Image image)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (image.ImageID > 0)
                    {
                        var tmpImage = db.Images.SingleOrDefault(i => i.ImageID == image.ImageID);
                        if (tmpImage != null)
                        {
                            tmpImage.Filepath = image.Filepath;
                            tmpImage.Resolution = image.Resolution;
                            tmpImage.ProductID = image.ProductID;
                            tmpImage.Thumbnailpath = image.Thumbnailpath;
                        }
                    }
                    else
                    {
                        db.Images.Add(image);
                    }

                    db.SaveChanges();
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveImage - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }        
    }
}
