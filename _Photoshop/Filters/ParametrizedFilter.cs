using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public abstract class ParametrizedFilter : IFilter
    {
        private IParameters _parameters;

        protected ParametrizedFilter(IParameters parameters)
        {
            _parameters = parameters;
        }

        public ParameterInfo[] GetParameters() => _parameters.GetDescriptions();

        public Photo Process(Photo original, double[] parameters)
        {
            _parameters.SetValues(parameters);
            return Process(original, _parameters);
        }

        public abstract Photo Process(Photo original, IParameters parameters);
    }
}