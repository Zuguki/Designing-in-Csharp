using System;
using System.Collections.Generic;
using System.Linq;

namespace SRP.ControlDigit
{
    public static class Extensions
    {
        public static char ToChar(this int value) => char.Parse(value.ToString());

        private static int ToInt(this char value)
        {
            if (int.TryParse(value.ToString(), out var val))
                return val;

            throw new ArgumentException();
        } 

        public static IEnumerable<int> SplitTheNumberByPositions(this int value)
        {
            var strValue = value.ToString();
            return strValue.Select(val => val.ToInt());
        }
    }

    public static class ControlDigitAlgo
    {
        public static int Upc(long number)
        {
            var sum = GetSumOf(number, 3, value => 4 - value);
            return GetFinalDigitBySum(sum);
        }

        public static int Isbn10(long number)
        {
            var sum = GetSumOf(number, 2, num => num + 1);
            const int division = 11;

            if (division - sum % division == 10)
                return 'X';

            return sum % division == 0 ? 0.ToChar() : (division - sum % division).ToChar();
        }

        public static int Luhn(long number)
        {
            var sum = GetSumOf(number, 2, num => num == 2 ? 1 : 2, num => num.SplitTheNumberByPositions());
            const int division = 10;

            return (10 - sum % division) % division;
        }

        private static int GetSumOf(long number, int startValue, Func<int, int> step,
            Func<int, IEnumerable<int>> addedFunc = null)
        {
            var sum = 0;
            for (var index = startValue; number > 0; number /= 10, index = step(index))
            {
                var num = (int) (number % 10 * index);
                sum += addedFunc is null ? num : addedFunc(num).Sum();
            }

            return sum;
        }

        private static int GetFinalDigitBySum(int sum)
        {
            var result = sum % 10;
            if (result != 0)
                return 10 - result;

            return result;
        }
    }
}
