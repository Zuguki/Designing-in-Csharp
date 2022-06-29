using MyPhotoshop.Data;

namespace MyPhotoshop.Filters
{
    public class LighteningFilter : IFilter
    {
        public ParameterInfo[] GetParameters()
        {
            return new[]
            {
                new ParameterInfo {Name = "Коэффициент", MaxValue = 10, MinValue = 0, Increment = 0.1, DefaultValue = 1}
            };
        }

        public override string ToString() => "Осветление/затемнение";

        public Photo Process(Photo original, double[] parameters) => 
            Filter.Process(original, (x, y) => original.Data[x, y] * parameters[0]);
    }
}