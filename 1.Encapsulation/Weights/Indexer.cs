using System;
using System.Collections.Generic;

namespace Incapsulation.Weights
{
    public class Indexer
    {
        public int Length { get; }

        public Indexer(double[] doubles, int start, int length)
        {
            CheckArguments(doubles, start, length);
            Doubles = doubles;
            Start = start;
            Length = length;
        }

        private double[] Doubles { get; }
        private int Start { get; }

        public double this[int index]
        {
            get
            {
                CheckIndex(index);
                return Doubles[Start + index];
            }
            set
            {
                CheckIndex(index);
                Doubles[Start + index] = value;
            }
        }

        private void CheckIndex(int index)
        {
            if (Start + index < 0 || Start + index > Length || index < 0)
                throw new IndexOutOfRangeException();
        }

        private static void CheckArguments(IReadOnlyCollection<double> doubles, int start, int length)
        {
            if (start < 0 || start > doubles.Count || length < 0 || length + start > doubles.Count)
                throw new ArgumentException();
        }
    }
}