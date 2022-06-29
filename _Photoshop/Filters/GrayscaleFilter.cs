using System;
using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class GrayscaleFilter : PixelFilter
    {
        public override ParameterInfo[] GetParameters() => new ParameterInfo[0];
        
        protected override Pixel ProcessPixel(int x, int y, Photo original, double[] parameters)
        {
            var lightness = original[x, y].Red + original[x, y].Green + original[x, y].Blue;
            lightness /= 3;
            return new Pixel(lightness, lightness, lightness);
        }
        
        public override string ToString() => "Оттенки серого";
    }
}