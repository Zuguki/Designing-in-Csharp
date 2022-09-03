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
            var sum = 0;
            var factor = 3;
            do
            {
                var digit = (int)(number % 10);
                sum += factor * digit;
                factor = 4 - factor;
                number /= 10;
            }
            while (number > 0);

            return GetFinalDigitBySum(sum);
        }

        public static int Isbn10(long number)
        {
            var sum = 0;
            const int division = 11;
            
            for (var index = 2; number > 0; index++, number /= 10)
                sum += (int) (number % 10) * index;

            if (division - sum % division == 10)
                return 'X';

            return sum % division == 0 ? 0.ToChar() : (division - sum % division).ToChar();
        }

        public static int Luhn(long number)
        {
            var sum = 0;
            const int division = 10;
            var checker = 1;
            for (var multiplyValue = 2; number > 0; multiplyValue += checker, number /= 10)
            {
                sum += ((int) (number % 10) * multiplyValue).SplitTheNumberByPositions().Sum();
                checker = -checker;
            }

            return (10 - sum % division) % division;
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
