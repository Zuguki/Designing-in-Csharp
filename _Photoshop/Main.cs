using System;
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
            Application.Run(window);
        }
    }
}