namespace MyPhotoshop.Data
{
    public class Photo
    {
        public readonly int Width;
        public readonly int Height;
        public readonly Pixel[,] Data;

        public Photo(int width, int height)
        {
            Width = width;
            Height = height;
            Data = new Pixel[width, height];
        }

        public Pixel this[int x, int y]
        {
            get => Data[x, y];
            set => Data[x, y] = value;
        }
    }
}