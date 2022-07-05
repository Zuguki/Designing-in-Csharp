using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class GrayscaleFilter : PixelFilter<EmptyParameters>
    {
        public override string ToString() => "Оттенки серого";

        protected override Pixel ProcessPixel(Pixel original, EmptyParameters parameters)
        {
            var lightness = original.Red + original.Green + original.Blue;
            lightness /= 3;
            return new Pixel(lightness, lightness, lightness);
        }
    }
}