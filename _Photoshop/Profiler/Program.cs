using System;
using System.Diagnostics;
using MyPhotoshop.Filters;
using MyPhotoshop.Filters.Parameters;

namespace Profiler
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var simpleHandler = new SimpleParametersHandler<LighteningParameters>();
            var staticHandler = new StaticParametersHandler<LighteningParameters>();
            
            Test(values => simpleHandler.CreateParameters(values), 100_000);
            Test(values => staticHandler.CreateParameters(values), 100_000);
            Test(values => new LighteningParameters {Ratio = values[0]}, 100_000);
        }

        private static void Test(Func<double[], LighteningParameters> action, int upperBound)
        {
            var args = new double[] {0};
            action(args);

            var watch = new Stopwatch();
            watch.Start();
            for (var counter = 0; counter < upperBound; counter++)
                action(args);
            watch.Stop();

            Console.WriteLine(1000 * (double) watch.ElapsedMilliseconds / upperBound + " сек");
        }
    }
}