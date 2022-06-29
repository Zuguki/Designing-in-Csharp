using System;
using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class GrayscaleFilter : PixelFilter
    {
        protected override Pixel ProcessPixel(Pixel original, IParameters parameters)
        {
            var lightness = original.Red + original.Green + original.Blue;
            lightness /= 3;
            return new Pixel(lightness, lightness, lightness);
        }
        
        public override string ToString() => "Оттенки серого";

        public GrayscaleFilter() : base(new EmptyParameters())
        { }
    }
}