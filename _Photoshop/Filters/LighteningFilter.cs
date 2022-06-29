using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class LighteningFilter : PixelFilter
    {
        public override ParameterInfo[] GetParameters() => 
            new[]
            {
                new ParameterInfo {Name = "Коэффициент", MaxValue = 10, MinValue = 0, Increment = 0.1, DefaultValue = 1}
            };

        protected override Pixel ProcessPixel(Pixel original, double[] parameters) => 
            original * parameters[0];

        public override string ToString() => "Осветление/затемнение";
    }
}