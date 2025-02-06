using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snakeLogic
{
    public class GameBoard
    {
        public GameBoard(int height, int width)
        {
            Height = height;
            Width = width;
        }
        public int Height { get; }
        public int Width { get; }

        public Snake Snake { get; set; } = new Snake(1, 1, 3);

    }
}
