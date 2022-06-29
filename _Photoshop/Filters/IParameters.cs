namespace MyPhotoshop.Filters
{
    public interface IParameters
    {
        ParameterInfo[] GetDescriptions();
        void SetValues(double[] values);
    }
}