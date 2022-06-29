namespace MyPhotoshop.Filters
{
    public class LighteningParameters : IParameters
    {
        public double Ratio { get; private set; }

        public ParameterInfo[] GetDescriptions() =>
            new[]
            {
                new ParameterInfo {Name = "Коэффициент", MaxValue = 10, MinValue = 0, Increment = 0.1, DefaultValue = 1}
            };


        public void SetValues(double[] values) => Ratio = values[0];
    }
}