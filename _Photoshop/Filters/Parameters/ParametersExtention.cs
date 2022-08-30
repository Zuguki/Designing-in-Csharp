using System.Linq;

namespace MyPhotoshop.Filters.Parameters
{
    public static class ParametersExtention
    {
        public static ParameterInfo[] GetDescriptions(this IParameters parameters)
        {
            return parameters
                .GetType()
                .GetProperties()
                .Select(item => item.GetCustomAttributes(typeof(ParameterInfo), false))
                .Where(item => item.Length > 0)
                .Select(item => item[0])
                .Cast<ParameterInfo>()
                .ToArray();
        }

        public static void SetValues(this IParameters parameters, double[] values)
        {
            var properties = parameters
                .GetType()
                .GetProperties()
                .Where(item => item.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
                .ToArray();

            for (var index = 0; index < values.Length; index++)
                properties[index].SetValue(parameters, values[index], new object[0]);
        }
    }
}