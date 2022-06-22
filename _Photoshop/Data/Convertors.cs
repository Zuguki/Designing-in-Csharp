using System;
using System.Drawing;

namespace MyPhotoshop.Data
{
    public static class Convertors
    {
        public static Photo Bitmap2Photo(Bitmap bmp)
        {
            var photo = new Photo();
            photo.Width = bmp.Width;
            photo.Height = bmp.Height;
            photo.Data = new Pixel[bmp.Width, bmp.Height];
            for (var x = 0; x < bmp.Width; x++)
            for (var y = 0; y < bmp.Height; y++)
            {
                var pixel = bmp.GetPixel(x, y);
                photo.Data[x, y] = new Pixel((double) pixel.R / 255, (double) pixel.G / 255, (double) pixel.B / 255);
            }

            return photo;
        }

        private static int ToChannel(double val)
        {
            if (val < 0 || val > 1)
                throw new Exception($"Wrong channel value {val} (the value must be between 0 and 1");
            return (int) (val * 255);
        }

        public static Bitmap Photo2Bitmap(Photo photo)
        {
            var bmp = new Bitmap(photo.Width, photo.Height);
            for (var x = 0; x < bmp.Width; x++)
            for (var y = 0; y < bmp.Height; y++)
                bmp.SetPixel(x, y, Color.FromArgb(
                    ToChannel(photo.Data[x, y].Red),
                    ToChannel(photo.Data[x, y].Green),
                    ToChannel(photo.Data[x, y].Blue)));

            return bmp;
        }
    }
}