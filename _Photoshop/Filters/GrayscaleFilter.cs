using System;
using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class GrayscaleFilter : IFilter
    {
        public ParameterInfo[] GetParameters() => new ParameterInfo[0];

        public Photo Process(Photo original, double[] parameters)
        {
            var processPixel = new Func<int, int, Pixel>((x, y) =>
            {
                var lightness = original[x, y].Red + original[x, y].Green + original[x, y].Blue;
                lightness /= 3;
                return new Pixel(lightness, lightness, lightness);
            });

            return Filter.Process(original, processPixel);
        }

        public override string ToString() => "Оттенки серого";
    }
}