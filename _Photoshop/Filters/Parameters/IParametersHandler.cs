namespace MyPhotoshop.Filters.Parameters
{
    public interface IParametersHandler<out TParameters>
    {
        ParameterInfo[] GetDescription();
        TParameters CreateParameters(double[] values);
    }
}