using System;
using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class GrayscaleFilter : PixelFilter
    {
        public override ParameterInfo[] GetParameters() => new ParameterInfo[0];
        
        protected override Pixel ProcessPixel(Pixel original, double[] parameters)
        {
            var lightness = original.Red + original.Green + original.Blue;
            lightness /= 3;
            return new Pixel(lightness, lightness, lightness);
        }
        
        public override string ToString() => "Оттенки серого";
    }
}