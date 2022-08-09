using System;
using System.Drawing;
using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class TransformFilter : ParametrizedFilter<EmptyParameters>
    {
        private readonly Func<Size, Size> _sizeTransform;
        private readonly Func<Point, Size, Point> _pointTransform;
        private readonly string _name;

        public TransformFilter(Func<Size, Size> sizeTransform, Func<Point, Size, Point> pointTransform, string name)
        {
            _sizeTransform = sizeTransform;
            _pointTransform = pointTransform;
            _name = name;
        }

        public override string ToString() => _name;

        public override Photo Process(Photo original, EmptyParameters parameters)
        {
            var oldSize = new Size(original.Width, original.Height);
            var newSize = _sizeTransform(oldSize);
            var result = new Photo(newSize.Width, newSize.Height);

            for (var x = 0; x < newSize.Width; x++)
            {
                for (var y = 0; y < newSize.Height; y++)
                {
                    var point = new Point(x, y);
                    var oldPoint = _pointTransform(point, oldSize);
                    result[x, y] = original[oldPoint.X, oldPoint.Y];
                }
            }

            return result;
        }
    }
}