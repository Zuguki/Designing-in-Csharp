using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class LighteningFilter : IFilter
    {
        public ParameterInfo[] GetParameters()
        {
            return new[]
            {
                new ParameterInfo {Name = "Коэффициент", MaxValue = 10, MinValue = 0, Increment = 0.1, DefaultValue = 1}
            };
        }

        public override string ToString()
        {
            return "Осветление/затемнение";
        }

        public Photo Process(Photo original, double[] parameters)
        {
            var result = new Photo
            {
                Width = original.Width,
                Height = original.Height
            };
            result.Data = new Pixel[result.Width, result.Height];

            for (var x = 0; x < result.Width; x++)
            for (var y = 0; y < result.Height; y++)
            for (var z = 0; z < 3; z++)
            {
                result.Data[x, y] = new Pixel();
                result.Data[x, y] = original.Data[x, y] * parameters[0];
            }
            return result;
        }
    }
}