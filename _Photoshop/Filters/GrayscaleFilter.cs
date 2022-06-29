using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class GrayscaleFilter : IFilter
    {
        public ParameterInfo[] GetParameters() => new ParameterInfo[0];

        public Photo Process(Photo original, double[] parameters)
        {
            var result = new Photo(original.Width, original.Height);

            for (var x = 0; x < result.Width; x++)
            for (var y = 0; y < result.Height; y++)
            {
                var lightness = original[x, y].Red + original[x, y].Green + original[x, y].Blue;
                lightness /= 3;
                result[x, y] = new Pixel(lightness, lightness, lightness);
            }

            return result;
        }
        
        public override string ToString() => "Оттенки серого";
    }
}