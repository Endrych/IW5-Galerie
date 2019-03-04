using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using MyPhotoBL.Models;
using MyPhotoDb.Enums;

namespace MyPhotoBL
{
    public class PiuctureLoader
    {
        public static PictureDetailModel FromFile(string fullFileName)
        {
            var filename = Path.GetFileName(fullFileName);
            var extension = Path.GetExtension(fullFileName).Replace(".", string.Empty);

            if (!Enum.TryParse(extension, true, out EntityImageFormat format))
                format = EntityImageFormat.None;

            var result = new PictureDetailModel
            {
                Name = filename,
                Source = fullFileName,
                PositionCollection = new List<PositionInPictureModel>(),
                Format = format
            };

            Image image;
            try
            {
                image = new Bitmap(fullFileName);
            }
            catch (FileNotFoundException e)
            {
                return result;
            }

            FormatFromImage(result, image);
            result.ResolutionWidth = image.Size.Width;
            result.ResolutionHeight = image.Size.Height;

            // from https://stackoverflow.com/questions/24971184/extract-image-file-metadata
            var encodings = new ASCIIEncoding();
            foreach (var propItem in image.PropertyItems)
            {
                if (propItem.Id.ToString("x").Equals("9003")) // taken date
                {
                    var datetime = encodings.GetString(propItem.Value, 0, propItem.Value.Length - 1);
                    if (DateTime.TryParseExact(datetime, "yyyy:MM:dd HH:mm:ss", null, DateTimeStyles.None, out var date))
                        result.PhotoTakenDate = date;
                }
            }
            return result;
        }

        private static void FormatFromImage(PictureDetailModel picture, Image image)
        {
            if (image.RawFormat.Equals(ImageFormat.Emf))  { picture.Format = EntityImageFormat.Emf;  return; }
            if (image.RawFormat.Equals(ImageFormat.Exif)) { picture.Format = EntityImageFormat.Exif; return; }
            if (image.RawFormat.Equals(ImageFormat.Gif))  { picture.Format = EntityImageFormat.Gif;  return; }
            if (image.RawFormat.Equals(ImageFormat.Icon)) { picture.Format = EntityImageFormat.Ico;  return; }
            if (image.RawFormat.Equals(ImageFormat.Jpeg)) { picture.Format = EntityImageFormat.Jpg;  return; }
            if (image.RawFormat.Equals(ImageFormat.Png))  { picture.Format = EntityImageFormat.Png;  return; }
            if (image.RawFormat.Equals(ImageFormat.Tiff)) { picture.Format = EntityImageFormat.Tiff; return; }
            if (image.RawFormat.Equals(ImageFormat.Wmf)) picture.Format = EntityImageFormat.Wmf;
        }
    }
}