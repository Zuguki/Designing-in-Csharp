namespace MyPhotoshop.Filters
{
    public class EmptyParameters : IParameters
    {
        public ParameterInfo[] GetDescriptions() => new ParameterInfo[0];

        public void SetValues(double[] values)
        { }
    }
}