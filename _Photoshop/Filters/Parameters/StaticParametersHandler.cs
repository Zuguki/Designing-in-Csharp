using System.Linq;
using System.Reflection;

namespace MyPhotoshop.Filters.Parameters
{
    public class StaticParametersHandler<TParameters> : IParametersHandler<TParameters>
        where TParameters : IParameters, new()

    {
        private static PropertyInfo[] Properties { get; }
        private static ParameterInfo[] Description { get; }

        static StaticParametersHandler()
        {
            Properties =
                typeof(TParameters)
                    .GetProperties()
                    .Where(item => item.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
                    .ToArray();

            Description =
                typeof(TParameters)
                    .GetProperties()
                    .Select(item => item.GetCustomAttributes(typeof(ParameterInfo), false))
                    .Where(item => item.Length > 0)
                    .Select(item => item[0])
                    .Cast<ParameterInfo>()
                    .ToArray();
        }

        public ParameterInfo[] GetDescription() => Description;

        public TParameters CreateParameters(double[] values)
        {
            var parameters = new TParameters();

            for (var index = 0; index < values.Length; index++)
                Properties[index].SetValue(parameters, values[index], new object[0]);

            return parameters;
        }
    }
}