using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MyPhotoshop.Filters.Parameters
{
    public class ExpressionsParametersHandler<TParameters> : IParametersHandler<TParameters>
        where TParameters : IParameters, new()

    {
        private static ParameterInfo[] Description { get; }
        
        private static Func<double[], TParameters> _parser;

        static ExpressionsParametersHandler()
        {
            Description =
                typeof(TParameters)
                    .GetProperties()
                    .Select(item => item.GetCustomAttributes(typeof(ParameterInfo), false))
                    .Where(item => item.Length > 0)
                    .Select(item => item[0])
                    .Cast<ParameterInfo>()
                    .ToArray();

            var properties = typeof(TParameters)
                .GetProperties()
                .Where(item => item.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
                .ToArray();
            
            // values => new LightningFilter { Ratio = values[0] };

            var arg = Expression.Parameter(typeof(double[]), "value");

            var bindings = new List<MemberBinding>();
            for (var index = 0; index < properties.Length; index++)
            {
                var binding = Expression.Bind(
                    properties[index],
                    Expression.ArrayIndex(arg, Expression.Constant(index)));
                
                bindings.Add(binding);
            }
            
            /*
             * var bindings = properties
             *      .Select((t, index) => Expression
             *          .Bind(t, Expression.ArrayIndex(arg, Expression.Constant(index))))
             *      .Cast<MemberBinding>()
             *      .ToList();
             */

            var body = Expression.MemberInit(Expression.New(typeof(TParameters).GetConstructor(new Type[0])),
                bindings);

            var lambda = Expression.Lambda<Func<double[], TParameters>>(body, arg);
            _parser = lambda.Compile();
        }

        public ParameterInfo[] GetDescription() => Description;

        public TParameters CreateParameters(double[] values) => _parser(values);
    }
}