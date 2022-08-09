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
            
            window.AddFilter(new TransformFilter(
                size => size,
                (point, size) => new Point(size.Width - point.X - 1, point.Y),
                "Отразить по горизонтали"));
            window.AddFilter(new TransformFilter(
                size => new Size(size.Height, size.Width),
                (point, size) => new Point(point.Y, point.X),
                "Отразить по ч.с"));

            window.AddFilter(new TransformFilter<RotationParameters>(
                "Свободное вращение", new RotateTransformer()));
            
            Application.Run(window);
        }
    }
}