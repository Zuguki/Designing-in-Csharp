using System.Drawing;

namespace MyPhotoshop.Filters
{
    public interface ITransformer<TParameters>
        where TParameters : IParameters, new()
    {
        void Prepare(Size size, TParameters parameters);
        
        Size Result { get; }
        
        Point PointMap(Point newPoint);
    }
}