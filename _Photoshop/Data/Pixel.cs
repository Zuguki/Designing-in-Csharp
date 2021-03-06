using System;

namespace MyPhotoshop.Data
{
    public struct Pixel
    {
        private static int LowerBound => 0;
        private static int UpperBound => 1;

        public double Red
        {
            get => _red;
            private set => _red = CheckValue(value);
        }

        public double Green
        {
            get => _green;
            private set => _green = CheckValue(value);
        }

        public double Blue
        {
            get => _blue;
            private set => _blue = CheckValue(value);
        }

        private double _red;
        private double _green;
        private double _blue;

        public Pixel(double red, double green, double blue)
        {
            _red = _green = _blue = LowerBound;
            
            Red = red;
            Green = green;
            Blue = blue;
        }

        public static Pixel operator *(Pixel pixel, double ratio) =>
            new Pixel(Trim(pixel.Red * ratio), Trim(pixel.Green * ratio), Trim(pixel.Blue * ratio));

        public static Pixel operator *(double ratio, Pixel pixel) => pixel * ratio;

        private static double CheckValue(double value)
        {
            if (value < LowerBound || value > UpperBound)
                throw new ArgumentException();

            return value;
        }

        private static double Trim(double value)
        {
            if (value < LowerBound)
                return LowerBound;
            if (value > UpperBound)
                return UpperBound;

            return value;
        }
    }
}