using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public abstract class PixelFilter : ParametrizedFilter
    {
        protected abstract Pixel ProcessPixel(Pixel original, IParameters parameters);

        public override Photo Process(Photo original, IParameters parameters)
        {
            var result = new Photo(original.Width, original.Height);

            for (var x = 0; x < result.Width; x++)
            for (var y = 0; y < result.Height; y++)
                result[x, y] = ProcessPixel(original[x, y], parameters);

            return result;
        }

        public PixelFilter(IParameters parameters) : base(parameters)
        { }
    }
}