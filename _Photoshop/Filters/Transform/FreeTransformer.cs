using System;
using System.Drawing;

namespace MyPhotoshop.Filters
{
    public class FreeTransformer : ITransformer<EmptyParameters>
    {
        private readonly Func<Size, Size> _sizeTransform;
        private readonly Func<Point, Size, Point> _pointTransform;
        private Size _oldSize;

        public Size ResultSize { get; private set; }

        public FreeTransformer(Func<Size, Size> sizeTransform, Func<Point, Size, Point> pointTransform)
        {
            _sizeTransform = sizeTransform;
            _pointTransform = pointTransform;
        }

        public void Prepare(Size size, EmptyParameters parameters)
        {
            _oldSize = size;
            ResultSize = _sizeTransform(_oldSize);
        }

        public Point? MapPoint(Point newPoint) => _pointTransform(newPoint, _oldSize);
    }
}