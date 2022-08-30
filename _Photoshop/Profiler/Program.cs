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
            Test((values, pairs) => pairs.SetValues(values), 100_000);
            Test((values, pairs) => pairs.Ratio = values[0], 100_000);
        }

        public static void Test(Action<double[], LighteningParameters> action, int upperBound)
        {
            var args = new double[] {0};
            var obj = new LighteningParameters();
            action(args, obj);

            var watch = new Stopwatch();
            watch.Start();
            for (var counter = 0; counter < upperBound; counter++)
                action(args, obj);
            watch.Stop();

            Console.WriteLine(1000 * (double) watch.ElapsedMilliseconds / upperBound + " сек");
        }
    }
}