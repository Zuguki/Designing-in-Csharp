using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }
        public readonly bool IsNan;

        public Rational(int numerator = 1, int denominator = 1)
        {
            Numerator = numerator;
            Denominator = denominator;

            if (Numerator == 0 && Denominator != 0)
                Denominator = 1;
            
            if (denominator < 0 && numerator > 0 || denominator < 0 && numerator < 0)
            {
                Numerator *= -1;
                Denominator *= -1;
            }
            else if (denominator < 0)
                Denominator *= -1;

            var divisor = GetBiggerCommonDivisor(Numerator, Denominator);
            Numerator /= divisor;
            Denominator /= divisor;

            IsNan = Denominator == 0;
        }

        public static Rational operator +(Rational rational1, Rational rational2)
        {
            return new Rational(rational1.Numerator * rational2.Denominator +
                                rational1.Denominator * rational2.Numerator,
                rational1.Denominator * rational2.Denominator);
        }

        public static Rational operator -(Rational rational1, Rational rational2)
        {
            return new Rational(rational1.Numerator * rational2.Denominator -
                                rational1.Denominator * rational2.Numerator,
                rational1.Denominator * rational2.Denominator);
        }

        public static Rational operator *(Rational rational1, Rational rational2)
        {
            return new Rational(rational1.Numerator * rational2.Numerator,
                rational1.Denominator * rational2.Denominator);
        }

        public static Rational operator /(Rational rational1, Rational rational2)
        {
            if (rational1.IsNan || rational2.IsNan)
                return new Rational(0, 0);
            
            return new Rational(rational1.Numerator * rational2.Denominator,
                rational1.Denominator * rational2.Numerator);
        }
        
        public static implicit operator int(Rational p1)
        {
            if (p1.Numerator % p1.Denominator == 0)
            {
                return p1.Numerator / p1.Denominator;
            }

            throw new ArgumentException();
        }

        public static implicit operator double(Rational p1)
        {
            if (p1.Denominator == 0)
                return double.NaN;
            
            return (double) p1.Numerator / p1.Denominator;
        }

        public static implicit operator Rational(int p1) => new Rational(p1);

        private static int GetBiggerCommonDivisor(int numerator, int denominator)
        {
            var biggerDivisor = 1;
            for (var counter = 1; counter < Math.Max(numerator, denominator); counter++)
            {
                if (numerator % counter == 0 && denominator % counter == 0)
                    biggerDivisor = counter;
            }

            return biggerDivisor;
        }
    }
}