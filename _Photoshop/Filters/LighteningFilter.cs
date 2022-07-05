using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class LighteningFilter : PixelFilter<LighteningParameters>
    {
        protected override Pixel ProcessPixel(Pixel original, LighteningParameters parameters) => 
            original * parameters.Ratio;

        public override string ToString() => "Осветление/затемнение";
    }
}