using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.PairsAnalysis
{
    public static class Analysis
    {
        public static int FindMaxPeriodIndex(params DateTime[] data)
        {
            var pairs = data.Pairs();
            var collection = pairs.MakeCollection((date1, date2) => (date2 - date1).TotalSeconds);
            return collection.MaxIndex();
        }

        public static double FindAverageRelativeDifference(params double[] data)
        {
            var pairs = data.Pairs();
            var collection = pairs.MakeCollection((value1, value2) => (value2 - value1) / value1).ToList();
            return collection.Average();
        }
    }

    public static class Extention
    {
        public static int MaxIndex<T>(this IEnumerable<T> items)
        where T : IComparable<T>
        {
            return items.Select((value, index) => new {Value = value, Index = index})
                .Aggregate((a, b) => a.Value.CompareTo(b.Value) > 0 ? a : b)
                .Index;
        }

        public static IEnumerable<TOut> MakeCollection<TIn, TOut>(this IEnumerable<Tuple<TIn, TIn>> pairs,
            Func<TIn, TIn, TOut> process) => pairs.Select(item => process(item.Item1, item.Item2));

        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> items)
        {
            var previewItem = default(T);
            var counter = 0;

            foreach (var item in items)
            {
                if (counter++ == 0)
                {
                    previewItem = item;
                    continue;
                }

                yield return new Tuple<T, T>(previewItem, item);
                previewItem = item;
            }
        }
    }
}