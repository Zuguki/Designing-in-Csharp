namespace MyPhotoshop.Data
{
    public class Pixel
    {
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }

        public Pixel() : this(0, 0, 0)
        { }

        public Pixel(double red, double green, double blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public static Pixel operator *(Pixel pixel, double ratio) => 
            new Pixel(pixel.Red * ratio, pixel.Green * ratio, pixel.Blue * ratio);
    }
}