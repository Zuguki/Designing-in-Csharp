using System;
using System.Drawing;
using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class TransformFilter<TParameters> : ParametrizedFilter<TParameters>
        where TParameters : IParameters, new()
    {
        private readonly string _name;
        private ITransformer<TParameters> _transformer;

        public TransformFilter(string name, ITransformer<TParameters> transformer)
        {
            _name = name;
            _transformer = transformer;
        }

        public override string ToString() => _name;

        public override Photo Process(Photo original, TParameters parameters)
        {
            var oldSize = new Size(original.Width, original.Height);
            _transformer.Prepare(oldSize, parameters);
            var result = new Photo(_transformer.ResultSize.Width, _transformer.ResultSize.Height);

            for (var x = 0; x < _transformer.ResultSize.Width; x++)
            {
                for (var y = 0; y < _transformer.ResultSize.Height; y++)
                {
                    var point = new Point(x, y);
                    var oldPoint = _transformer.MapPoint(point);
                    if (oldPoint.HasValue)
                        result[x, y] = original[oldPoint.Value.X, oldPoint.Value.Y];
                }
            }

            return result;
        }
    }
}