namespace MyPhotoshop.Filters
{
    public class RotationParameters : IParameters
    {
        public double Angle { get; private set; }

        public ParameterInfo[] GetDescriptions() =>
            new[]
            {
                new ParameterInfo {Name = "Угол", MaxValue = 360, MinValue = 0, Increment = 5, DefaultValue = 0}
            };

        public void SetValues(double[] values) => Angle = values[0];
    }
}