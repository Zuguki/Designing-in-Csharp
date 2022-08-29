using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    public static class Analysis
    {
        public static int FindMaxPeriodIndex(params DateTime[] data)
        {
            var pairs = data.Pairs();
            return pairs.MaxIndex();
            return new MaxPauseFinder().Analyze(data);
        }

        public static double FindAverageRelativeDifference(params double[] data)
        {
            return new AverageDifferenceFinder().Analyze(data);
        }
    }

    public static class Extention
    {
        public static int MaxIndex(this IEnumerable<Tuple<DateTime, DateTime>> items)
        {
            var itemsArray = items as Tuple<DateTime, DateTime>[] ?? items.ToArray();
            if (itemsArray.Length == 0)
                throw new ArgumentException();

            var maxIndex = 0;
            var maxValue = 0d;
            for (var index = 0; index < itemsArray.Length; index++)
            {
                var value = Math.Abs((itemsArray[index].Item1 - itemsArray[index].Item2).TotalSeconds);
                if (value <= maxValue) 
                    continue;
                
                maxValue = value;
                maxIndex = index;
            }

            return maxIndex;
        }
        
        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> items)
        {
            var itemsArray = items as T[] ?? items.ToArray();
            if (itemsArray.Length < 2)
                throw new InvalidOperationException();

            var previewItem = default(T);
            var counter = 0;

            foreach (var item in itemsArray)
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
