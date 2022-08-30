using MyPhotoshop.Data;
using MyPhotoshop.Filters.Parameters;

namespace MyPhotoshop.Filters
{
    public abstract class ParametrizedFilter<TParameters> : IFilter
        where TParameters : IParameters, new()
    {
        private TParameters _parameters = new TParameters();

        public ParameterInfo[] GetParameters() => _parameters.GetDescriptions();

        public Photo Process(Photo original, double[] values)
        {
            _parameters.SetValues(values);
            return Process(original, _parameters);
        }

        public abstract Photo Process(Photo original, TParameters parameters);
    }
}