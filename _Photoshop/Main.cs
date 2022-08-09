using System;
using System.Drawing;
using System.Windows.Forms;
using MyPhotoshop.Data;
using MyPhotoshop.Filters;

namespace MyPhotoshop
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var window = new MainWindow();
            window.AddFilter(new PixelFilter<LighteningParameters>(
                "Осветление/затемнение",
                (pixel, parameters) => pixel * parameters.Ratio));
            window.AddFilter(new PixelFilter<EmptyParameters>(
                "Оттенки серого",
                (pixel, parameters) =>
                {
                    var lightness = pixel.Red + pixel.Green + pixel.Blue;
                    lightness /= 3;
                    return new Pixel(lightness, lightness, lightness);
                }));
            
            // window.AddFilter(new TransformFilter(
            //     size => size,
            //     (point, size) => new Point(size.Width - point.X - 1, point.Y),
            //     "Отразить по горизонтали"));
            // window.AddFilter(new TransformFilter(
            //     size => new Size(size.Height, size.Width),
            //     (point, size) => new Point(point.Y, point.X),
            //     "Отразить по ч.с"));

            Func<Size, RotationParameters, Size> sizeRotation = (size, parameters) =>
            {
                var angle = Math.PI * parameters.Angle / 180;
                return new Size(
                    (int) (size.Width * Math.Abs(Math.Cos(angle)) + size.Height * Math.Abs(Math.Sin(angle))),
                    (int) (size.Height * Math.Abs(Math.Cos(angle)) + size.Height * Math.Abs(Math.Sin(angle))));
            };

            Func<Point, Size, RotationParameters, Point?> pointRotator = (point, oldSize, parameters) =>
            {
                var newSize = sizeRotation(oldSize, parameters);
                var angle = Math.PI * parameters.Angle / 180;
                point = new Point(point.X - newSize.Width / 2, point.Y - newSize.Height / 2);
                var x = oldSize.Width / 2 + (int) (point.X * Math.Cos(angle) + point.Y * Math.Sin(angle));
                var y = oldSize.Height / 2 + (int) (-point.X * Math.Sin(angle) + point.Y * Math.Cos(angle));

                if (x < 0 || x >= oldSize.Width || y < 0 || y >= oldSize.Height)
                    return null;
                return new Point(x, y);
            };
            
            window.AddFilter(new TransformFilter<RotationParameters>(
                sizeRotation, pointRotator, "Свободное вращение"));
            
            Application.Run(window);
        }
    }
}