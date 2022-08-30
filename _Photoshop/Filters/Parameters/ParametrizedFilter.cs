using MyPhotoshop.Data;
using MyPhotoshop.Filters.Parameters;

namespace MyPhotoshop.Filters
{
    public abstract class ParametrizedFilter<TParameters> : IFilter
        where TParameters : IParameters, new()
    {
        private readonly IParametersHandler<TParameters> _handler = new SimpleParametersHandler<TParameters>();

        public ParameterInfo[] GetParameters() => _handler.GetDescription();

        public Photo Process(Photo original, double[] values)
        {
            var parameters = _handler.CreateParameters(values);
            return Process(original, parameters);
        }

        protected abstract Photo Process(Photo original, TParameters parameters);
    }
}