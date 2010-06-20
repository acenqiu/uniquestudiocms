using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace UniqueStudio.Common.Imaging
{
    public class ImageConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width">宽度。设为0时表示按比例缩放。</param>
        /// <param name="height">高度。设为0时表示按比例缩放。</param>
        /// <param name="quality"></param>
        /// <param name="format"></param>
        public static void Convert(string source, string dest, int width, int height, long quality, UniqueStudio.Common.Model.ImageFormat format)
        {
            //验证

            Image oldImage = Image.FromFile(source);
            Bitmap newBitmap;
            ImageCodecInfo ici = GetCodecInfo(format);
            EncoderParameters parms = new EncoderParameters(1);
            parms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            Convert(oldImage, out newBitmap, width, height, format);
            newBitmap.Save(dest, ici, parms);

            oldImage.Dispose();
            newBitmap.Dispose();
        }

        public static void Convert(string source, out Bitmap newImage, int width, int height, UniqueStudio.Common.Model.ImageFormat format)
        {
            Image oldImage = Image.FromFile(source);
            Convert(oldImage, out newImage, width, height,format);
            oldImage.Dispose();
        }

        public static void Convert(Stream source, string dest, int width, int height, long quality, UniqueStudio.Common.Model.ImageFormat format)
        {
            Image oldImage = Image.FromStream(source);
            Bitmap newBitmap;
            ImageCodecInfo ici = GetCodecInfo(format);
            EncoderParameters parms = new EncoderParameters(1);
            parms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            Convert(oldImage, out newBitmap, width, height, format);
            newBitmap.Save(dest, ici, parms);

            //oldImage.Dispose();
            newBitmap.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// oldImage和newImage均不会被释放。
        /// </remarks>
        /// <param name="oldImage"></param>
        /// <param name="newImage"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="quality"></param>
        /// <param name="format"></param>
        public static void Convert(Image oldImage, out Bitmap newImage, int width, int height, UniqueStudio.Common.Model.ImageFormat format)
        {
            //验证

            int newWidth = width;
            int newHeight = height;
            if (newHeight == 0)
            {
                newHeight = (int)(oldImage.Height * (newWidth * 1.0 / oldImage.Width));
            }
            if (newWidth == 0)
            {
                newWidth = (int)(oldImage.Width * (newHeight * 1.0 / oldImage.Height));
            }

            newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);
            Graphics graphic = Graphics.FromImage(newImage);
            graphic.Clear(Color.Transparent);
            graphic.DrawImage(oldImage, new Rectangle(0, 0, newWidth, newHeight));
            graphic.Dispose();
        }

        private static ImageCodecInfo GetCodecInfo(UniqueStudio.Common.Model.ImageFormat format)
        {
            string mimeType = "";
            switch (format)
            {
                case UniqueStudio.Common.Model.ImageFormat.Jpeg:
                    mimeType = "image/jpeg";
                    break;
                case UniqueStudio.Common.Model.ImageFormat.Png:
                    mimeType = "image/png";
                    break;
                case UniqueStudio.Common.Model.ImageFormat.Gif:
                    mimeType = "image/gif";
                    break;
                default:
                    mimeType = "image/jpeg";
                    break;
            }
            return GetCodecInfo(mimeType);
        }

        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in icis)
            {
                if (ici.MimeType == mimeType)
                {
                    return ici;
                }
            }
            throw new Exception("指定类型不存在。");
        }
    }
}
