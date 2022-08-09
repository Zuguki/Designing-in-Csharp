using System;
using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class PixelFilter<TParameters> : ParametrizedFilter<TParameters> 
        where TParameters : IParameters, new()
    {
        private readonly string _name;
        private readonly Func<Pixel, TParameters, Pixel> _processor;

        public PixelFilter(string name, Func<Pixel, TParameters, Pixel> processor)
        {
            _name = name;
            _processor = processor;
        }

        public override Photo Process(Photo original, TParameters parameters)
        {
            var result = new Photo(original.Width, original.Height);

            for (var x = 0; x < result.Width; x++)
            for (var y = 0; y < result.Height; y++)
                result[x, y] = _processor(original[x, y], parameters);

            return result;
        }

        public override string ToString() => _name;
    }
}