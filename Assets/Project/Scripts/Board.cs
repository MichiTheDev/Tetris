namespace Tetris
{
    public sealed class Board
    {
        public int Width { get; }
        public int Height { get; }

        private int[,] _grid;
        
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            _grid = new int[width, height];
        }
    }
}