using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public abstract class PixelFilter : IFilter
    {
        public abstract ParameterInfo[] GetParameters();
        protected abstract Pixel ProcessPixel(int x, int y, Photo original, double[] parameters);

        public Photo Process(Photo original, double[] parameters)
        {
            var result = new Photo(original.Width, original.Height);

            for (var x = 0; x < result.Width; x++)
            for (var y = 0; y < result.Height; y++)
                result[x, y] = ProcessPixel(x, y, original, parameters);

            return result;
        }
    }
}