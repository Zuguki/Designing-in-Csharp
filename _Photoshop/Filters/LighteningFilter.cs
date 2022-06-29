using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class LighteningFilter : PixelFilter
    {
        protected override Pixel ProcessPixel(Pixel original, IParameters parameters) => 
            original * ((LighteningParameters) parameters).Ratio;

        public override string ToString() => "Осветление/затемнение";

        public LighteningFilter() : base(new LighteningParameters())
        { }
    }
}