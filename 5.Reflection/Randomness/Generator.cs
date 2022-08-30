using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflection.Randomness
{
    public class FromDistribution : Attribute
    {
        public IContinuousDistribution Distribution { get; set; }
        
        public FromDistribution(Type type, params object[] items)
        {
            Distribution = (IContinuousDistribution)Activator.CreateInstance(type, items);
        }
    }

    public class Generator<T> 
        where T : new()
    {
        public readonly Dictionary<string, IContinuousDistribution> Distributions;
        public readonly PropertyInfo[] PropertyInfos;

        public Generator()
        {
            Distributions = new Dictionary<string, IContinuousDistribution>();
            PropertyInfos = typeof(T).GetProperties();
        }

        public T Generate(Random rnd)
        {
            var result = new T();
            foreach (var propertyInfo in PropertyInfos)
            {
                if (!Distributions.ContainsKey(propertyInfo.Name))
                {
                    var attribute = propertyInfo
                        .GetCustomAttributes(false)
                        .OfType<FromDistribution>()
                        .FirstOrDefault();
                    
                    if (attribute != null)
                        propertyInfo.SetValue(result, attribute.Distribution.Generate(rnd));
                }
                else
                    propertyInfo.SetValue(result, Distributions[propertyInfo.Name].Generate(rnd));
            }

            return result;
        }
    }
}
