using System.Linq;
using System.Reflection;

namespace MyPhotoshop.Filters.Parameters
{
    public class SimpleParametersHandler<TParameters> : IParametersHandler<TParameters>
        where TParameters : IParameters, new()
    {
        private PropertyInfo[] _propertiesField;
        private PropertyInfo[] Properties => 
            _propertiesField ?? (_propertiesField = 
                typeof(TParameters)
                    .GetProperties()
                    .Where(item => item.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
                    .ToArray());

        public ParameterInfo[] GetDescription()
        {
            return typeof(TParameters)
                .GetProperties()
                .Select(item => item.GetCustomAttributes(typeof(ParameterInfo), false))
                .Where(item => item.Length > 0)
                .Select(item => item[0])
                .Cast<ParameterInfo>()
                .ToArray();
        }

        public TParameters CreateParameters(double[] values)
        {
            var parameters = new TParameters();

            for (var index = 0; index < values.Length; index++)
                Properties[index].SetValue(parameters, values[index], new object[0]);

            return parameters;
        }
    }
}