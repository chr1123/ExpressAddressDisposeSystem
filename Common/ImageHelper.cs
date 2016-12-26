using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace EADS.Common
{
   public class ImageHelper
    {
       public ImageHelper()
       {
           SmallImgAppendStr = "_small";
           SmallImgWidth = 200;
       }


        public string SmallImgAppendStr { get; set; } 
        public int SmallImgWidth { get; set; }

        public int ChageSize(int oldWidth, int oldHeight, int changedValue,
           ChangeSizeFixedBy fixedBy = ChangeSizeFixedBy.Width)
        {
            if (oldWidth == 0 || oldHeight == 0)
            {
                return 0;
            }
            switch (fixedBy)
            {
                case ChangeSizeFixedBy.Width:
                    return int.Parse(
                        Math.Ceiling(oldHeight * (((float)changedValue / oldWidth))).ToString());

                case ChangeSizeFixedBy.Height:
                    return int.Parse(
                        Math.Ceiling(oldWidth * (((float)changedValue / oldHeight))).ToString());

            }
            return 0;
        }

        /// <summary>
        /// 固定宽度还是高度
        /// </summary>
        public enum ChangeSizeFixedBy
        {
            Width,
            Height
        }


        public bool SaveSmallImage(string imgPath, int fixedValue,
            ChangeSizeFixedBy fixedBy = ChangeSizeFixedBy.Width)
        {
            if (File.Exists(imgPath))
            {
                Bitmap bigImg = new Bitmap(imgPath);
                int smallWidth = 0;
                int smallHeight = 0;
                switch (fixedBy)
                {
                    case ChangeSizeFixedBy.Width:
                        smallWidth = fixedValue;
                        smallHeight = ChageSize(bigImg.Width, bigImg.Height, fixedValue, ChangeSizeFixedBy.Width);
                        break;
                    case ChangeSizeFixedBy.Height:
                        smallHeight = fixedValue;
                        smallWidth = ChageSize(bigImg.Width, bigImg.Height, fixedValue, ChangeSizeFixedBy.Height);
                        break;
                }
                string smallImgPath = imgPath.Insert(imgPath.LastIndexOf('.'),
                    SmallImgAppendStr);
                Bitmap smallImg = new Bitmap(smallWidth, smallHeight);
                Graphics g = Graphics.FromImage(smallImg);
                g.Clear(Color.Transparent);
                g.DrawImage(bigImg, new Rectangle(0, 0, smallImg.Width, smallImg.Height),
                            new Rectangle(0, 0, bigImg.Width, bigImg.Height),
                            GraphicsUnit.Pixel);
                smallImg.Save(smallImgPath, bigImg.RawFormat);
                g.Dispose();
                bigImg.Dispose();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 以指定宽度或高度重新生成新图片 
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        /// <param name="fixedValue"></param>
        /// <param name="fixedBy"></param>
        /// <param name="createSmallImage">是否生成缩略图</param>
        public void ReSaveImage(string oldPath, string newPath, int fixedValue, ChangeSizeFixedBy fixedBy,bool createSmallImage=true)
        {
            Image oldImg = Image.FromFile(oldPath);
            int width = 0;
            int height = 0;
            if (fixedBy == ChangeSizeFixedBy.Width)
            {
                width = fixedValue;
                height = ChageSize(oldImg.Width, oldImg.Height, fixedValue, fixedBy);
            }
            SendSmallImage(oldPath, newPath, height, width, 100);
            oldImg.Dispose();
            if (createSmallImage)
            {
                SaveSmallImage(newPath, SmallImgWidth);    
            }
            File.Delete(oldPath);
        }


        /// <summary>
        /// 更改图片大小并压缩图片
        /// </summary>
        /// <param name="fileName">原图片地址</param>
        /// <param name="newFile">生成后保存地址</param> 
        /// <param name="qualityValue">图片质量（0~100）</param>
        public void CompressImage(string fileName, string newFile,int qualityValue)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(fileName);
            System.Drawing.Imaging.ImageFormat
            thisFormat = img.RawFormat;
            Bitmap outBmp = new Bitmap(img.Width, img.Height);
            Graphics g = Graphics.FromImage(outBmp);
            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.Default;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height),
            0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时,设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = qualityValue;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];
                    //设置JPEG编码
                    break;
                }
            }
            if (jpegICI != null)
            {
                outBmp.Save(newFile, jpegICI, encoderParams);
            }
            else
            {
                outBmp.Save(newFile, thisFormat);
            }
            img.Dispose();
            outBmp.Dispose();
        }





        /// <summary>
        /// 更改图片大小并压缩图片
        /// </summary>
        /// <param name="fileName">原图片地址</param>
        /// <param name="newFile">生成后保存地址</param>
        /// <param name="maxHeight">最大高度</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="qualityValue">图片质量（0~100）</param>
        public void SendSmallImage(string fileName, string newFile, int maxHeight, int maxWidth, int qualityValue)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(fileName);
            System.Drawing.Imaging.ImageFormat
            thisFormat = img.RawFormat;
            Size newSize = NewSize(maxWidth, maxHeight, img.Width, img.Height);
            Bitmap outBmp = new Bitmap(newSize.Width, newSize.Height);
            Graphics g = Graphics.FromImage(outBmp);
            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.Default;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height),
            0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时,设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = qualityValue;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];
                    //设置JPEG编码
                    break;
                }
            }
            if (jpegICI != null)
            {
                outBmp.Save(newFile, jpegICI, encoderParams);
            }
            else
            {
                outBmp.Save(newFile,thisFormat);
            }
            img.Dispose();
            outBmp.Dispose();
        }

        /// <summary>
        /// 更改图片大小并压缩图片
        /// </summary>
        /// <param name="fileName">原图片地址</param>
        /// <param name="newFile">生成后保存地址</param>
        /// <param name="maxHeight">最大高度</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="qualityValue">图片质量（0~100）</param>
        public void SendSmallImage(HttpPostedFileBase fileStream, string newFile, int maxHeight, int maxWidth, int qualityValue)
        {
            System.Drawing.Image img = System.Drawing.Image.FromStream(fileStream.InputStream);
            System.Drawing.Imaging.ImageFormat thisFormat = img.RawFormat;
            Size newSize = NewSize(maxWidth, maxHeight, img.Width, img.Height);
            Bitmap outBmp = new Bitmap(newSize.Width, newSize.Height);
            Graphics g = Graphics.FromImage(outBmp);
            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.Default;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height),
            0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时,设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = qualityValue;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int x = 0; x < arrayICI.Length; x++)
            {
                if (arrayICI[x].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[x];
                    //设置JPEG编码
                    break;
                }
            }
            try
            {
                if (jpegICI != null)
                {
                    outBmp.Save(newFile, jpegICI, encoderParams);
                }
                else
                {
                    outBmp.Save(newFile, thisFormat);
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                img.Dispose();
                outBmp.Dispose();
            }
        }

        #region CreateThumbnail
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="uploadObject">上传的HttpPostedFile或图片物理路径</param>
        /// <param name="uploaddir">上传文件夹相对路径</param>
        /// <param name="ext">后缀（如：.jpg）</param>
        /// <param name="t_width">缩略图宽</param>
        /// <param name="t_height">缩略图高</param>
        /// <param name="filename">文件夹，不含路径和后缀</param>
        /// <param name="tm">枚举类-缩略图的样式</param>
        /// <returns>返回生成图片的路径</returns>
        public void CreateThumbnail(object uploadObject, string filepath, string ext, int t_width, int t_height,ThumbModel tm)
        {
            System.Drawing.Image thumbnail_image = null;
            System.Drawing.Image original_image = null;
            System.Drawing.Bitmap final_image = null;
            System.Drawing.Graphics graphic = null;
            MemoryStream ms = null;
            //string ThumbnailFilename = "";
            try
            {
                if (uploadObject is HttpPostedFileBase)
                {
                    HttpPostedFileBase jpeg_image_upload = uploadObject as HttpPostedFileBase;
                    original_image = System.Drawing.Image.FromStream(jpeg_image_upload.InputStream);
                }
                else if (uploadObject is HttpPostedFile)
                {
                    HttpPostedFile jpeg_image_upload = uploadObject as HttpPostedFile;
                    original_image = System.Drawing.Image.FromStream(jpeg_image_upload.InputStream);
                }
                else
                {
                    string jpeg_image_upload = uploadObject as string;
                    original_image = System.Drawing.Image.FromFile(jpeg_image_upload);
                }
                // Calculate the new width and height
                int original_paste_x = 0;
                int original_paste_y = 0;
                int original_width = original_image.Width;//截取原图宽度
                int original_height = original_image.Height;//截取原图高度
                int target_paste_x = 0;
                int target_paste_y = 0;
                int target_width1 = t_width;
                int target_height1 = t_height;
                if (tm == ThumbModel.NoDeformationAllThumb)
                {
                    float target_ratio = (float)t_width / (float)t_height;//缩略图 宽、高的比例
                    float original_ratio = (float)original_width / (float)original_height;//原图 宽、高的比例

                    if (target_ratio > original_ratio)//宽拉长
                    {
                        target_height1 = t_height;
                        target_width1 = (int)Math.Floor(original_ratio * (float)t_height);
                    }
                    else
                    {
                        target_height1 = (int)Math.Floor((float)t_width / original_ratio);
                        target_width1 = t_width;
                    }

                    target_width1 = target_width1 > t_width ? t_width : target_width1;
                    target_height1 = target_height1 > t_height ? t_height : target_height1;
                    target_paste_x = (t_width - target_width1) / 2;
                    target_paste_y = (t_height - target_height1) / 2;
                }
                else if (tm == ThumbModel.NoDeformationCenterThumb)
                {
                    float target_ratio = (float)t_width / (float)t_height;//缩略图 宽、高的比例
                    float original_ratio = (float)original_width / (float)original_height;//原图 宽、高的比例

                    if (target_ratio > original_ratio)//宽拉长
                    {
                        original_height = (int)Math.Floor((float)original_width / target_ratio);
                    }
                    else
                    {
                        original_width = (int)Math.Floor((float)original_height * target_ratio);
                    }
                    original_paste_x = (original_image.Width - original_width) / 2;
                    original_paste_y = (original_image.Height - original_height) / 2;
                }
                else if (tm == ThumbModel.NoDeformationCenterBig)
                {
                    original_paste_x = (original_width - target_width1) / 2;
                    original_paste_y = (original_height - target_height1) / 2;
                    if (original_height > target_height1) original_height = target_height1;
                    if (original_width > target_width1) original_width = target_width1;
                }

                final_image = new System.Drawing.Bitmap(t_width, t_height);
                graphic = System.Drawing.Graphics.FromImage(final_image);
                // graphic.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), new System.Drawing.Rectangle(0, 0, t_width, t_height));//背景颜色

                graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High; /* new way */
                graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphic.Clear(Color.White);//背景
                Rectangle SrcRec = new Rectangle(original_paste_x, original_paste_y, original_width, original_height);
                Rectangle targetRec = new Rectangle(target_paste_x, target_paste_y, target_width1, target_height1);
                graphic.DrawImage(original_image, targetRec, SrcRec, GraphicsUnit.Pixel);
                //string saveFileName = uploaddir + filename + "_small" + ext;
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    final_image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //ThumbnailFilename = saveFileName;
                }
            }
            catch
            {
                // If any kind of error occurs return a 500 Internal Server error
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.Write("An error occured");
                HttpContext.Current.Response.End();
            }
            finally
            {
                // Clean up
                if (final_image != null) final_image.Dispose();
                if (graphic != null) graphic.Dispose();
                if (original_image != null) original_image.Dispose();
                if (thumbnail_image != null) thumbnail_image.Dispose();
                if (ms != null) ms.Close();
            }
        }
        #endregion


        public enum ThumbModel
        {
            /// <summary>
            /// 不变形，全部（缩略图）
            /// </summary>
            NoDeformationAllThumb,
            /// <summary>
            /// 变形，全部填充（缩略图）
            /// </summary>
            DeformationAllThumb,
            /// <summary>
            /// 不变形，截中间（缩略图）
            /// </summary>
            NoDeformationCenterThumb,
            /// <summary>
            /// 不变形，截中间（非缩略图）
            /// </summary>
            NoDeformationCenterBig
        }

        private Size NewSize(int maxWidth, int maxHeight, int width, int height)
        {
            double w = 0.0;
            double h = 0.0;
            double sw = Convert.ToDouble(width);
            double sh = Convert.ToDouble(height); double mw = Convert.ToDouble(maxWidth);
            double mh = Convert.ToDouble(maxHeight); if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = maxWidth;
                h = (w * sh) / sw;
            }
            else
            {
                h = maxHeight;
                w = (h * sw) / sh;
            }
            return new Size(Convert.ToInt32(w), Convert.ToInt32(h));
        }

    }
}
