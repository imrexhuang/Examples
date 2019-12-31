﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Simple.Common.Drawing
{
    public class ImageUtils
    {
        #region GDI+ Compress

        private static Bitmap ScaleImage(Bitmap image, double scale)
        {
            int newWidth = (int)(image.Width * scale);
            int newHeight = (int)(image.Height * scale);

            Bitmap result = new Bitmap(image, newWidth, newHeight);
            //builder.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //using (Graphics g = Graphics.FromImage(builder))
            //{
            //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    g.CompositingQuality = CompositingQuality.HighQuality;
            //    g.SmoothingMode = SmoothingMode.HighQuality;
            //    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //    g.DrawImage(image, 0, 0, builder.Width, builder.Height);
            //}
            return result;
        }

        public static byte[] CompressByGdi(byte[] byteImageIn, int targetSize)
        {
            if (byteImageIn == null)
            {
                throw new ArgumentNullException("byteImageIn");
            }

            if (byteImageIn.Length <= targetSize)
            {
                return byteImageIn;
            }

            using (var ms = new MemoryStream(byteImageIn))
            {
                var bmp = (Bitmap)Image.FromStream(ms, false, true);

                while (ms.Length > targetSize)
                {
                    var scale = Math.Sqrt((double)targetSize / (double)ms.Length);
                    scale = scale - 0.03;
                    if (scale >= 1)
                    {
                        scale = 0.97;
                    }

                    ms.SetLength(0);
                    bmp = ScaleImage(bmp, scale);
                    bmp.Save(ms, ImageFormat.Jpeg);
                }

                if (bmp != null)
                {
                    bmp.Dispose();
                }

                return ms.ToArray();
            }
        }

        #endregion

        #region WPF Compress

        private static BitmapImage ResizeImage(byte[] imageData, int height, int width)
        {
            Contract.Requires(imageData != null);
            Contract.Requires(height > 0 && width > 0);

            var newBitmap = new BitmapImage();
            newBitmap.BeginInit();
            newBitmap.DecodePixelHeight = height;
            newBitmap.DecodePixelWidth = width;
            newBitmap.StreamSource = new MemoryStream(imageData);
            newBitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            newBitmap.CacheOption = BitmapCacheOption.Default;
            RenderOptions.SetBitmapScalingMode(newBitmap, BitmapScalingMode.HighQuality);
            newBitmap.EndInit();
            return newBitmap;
        }

        private static BitmapImage LoadBitmapImage(byte[] bytes)
        {
            Contract.Requires(bytes != null);

            var newBitmap = new BitmapImage();
            newBitmap.BeginInit();
            newBitmap.StreamSource = new MemoryStream(bytes);
            newBitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            newBitmap.CacheOption = BitmapCacheOption.Default;
            newBitmap.EndInit();
            return newBitmap;
        }

        public static byte[] Compress(byte[] byteImageIn, int targetSize, int quality = 35)
        {
            if (byteImageIn == null)
            {
                throw new ArgumentNullException("byteImageIn");
            }

            long length = byteImageIn.Length;

            if (length <= targetSize)
            {
                return byteImageIn;
            }
            byte[] result = null;

            var stream = new MemoryStream();
            var newBitmap = LoadBitmapImage(byteImageIn);
            var scale = 0.99d;
            try
            {
                while (length > targetSize)
                {
                    var width = (int)(newBitmap.PixelWidth * scale);
                    var height = (int)(newBitmap.PixelHeight * scale);

                    newBitmap = ResizeImage(byteImageIn, height, width);
                    var encoder = new JpegBitmapEncoder() { QualityLevel = quality };
                    var bitmapFrame = BitmapFrame.Create(newBitmap as BitmapSource);
                    encoder.Frames.Add(bitmapFrame);
                    encoder.Save(stream);
                    length = stream.Length;

                    if (!(length > targetSize))
                    {
                        result = stream.ToArray();
                        break;
                    }

                    stream.Dispose();
                    stream = new MemoryStream();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                stream.Dispose();
                if (newBitmap != null && newBitmap.StreamSource != null)
                {
                    newBitmap.StreamSource.Dispose();
                }
                stream = null;
                newBitmap = null;
            }

            return result;
        }

        #endregion

        public static Image GetImageFromPath(string path)
        {
            var bytes = File.ReadAllBytes(path);
            var ms = new MemoryStream(bytes);
            var img = Image.FromStream(ms);
            return img;
        }
    }
}
