using System;
using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public static class Filter
    {
        public static Photo Process(Photo original, Func<int, int, Pixel> processPixel)
        {
            var result = new Photo(original.Width, original.Height);

            for (var x = 0; x < result.Width; x++)
            for (var y = 0; y < result.Height; y++)
                result[x, y] = processPixel(x, y);

            return result;
        }
    }
}