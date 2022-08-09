using System;
using System.Drawing;
using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class TransformFilter<TParameters> : ParametrizedFilter<TParameters>
        where TParameters : IParameters, new()
    {
        private readonly Func<Size, TParameters, Size> _sizeTransform;
        private readonly Func<Point, Size, TParameters, Point?> _pointTransform;
        private readonly string _name;

        public TransformFilter(Func<Size, TParameters, Size> sizeTransform,
            Func<Point, Size, TParameters, Point?> pointTransform, string name)
        {
            _sizeTransform = sizeTransform;
            _pointTransform = pointTransform;
            _name = name;
        }

        public override string ToString() => _name;

        public override Photo Process(Photo original, TParameters parameters)
        {
            var oldSize = new Size(original.Width, original.Height);
            var newSize = _sizeTransform(oldSize, parameters);
            var result = new Photo(newSize.Width, newSize.Height);

            for (var x = 0; x < newSize.Width; x++)
            {
                for (var y = 0; y < newSize.Height; y++)
                {
                    var point = new Point(x, y);
                    var oldPoint = _pointTransform(point, oldSize, parameters);
                    if (oldPoint.HasValue)
                        result[x, y] = original[oldPoint.Value.X, oldPoint.Value.Y];
                }
            }

            return result;
        }
    }
}