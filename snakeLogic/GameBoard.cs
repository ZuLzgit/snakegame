using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace snakeLogic
{
    public class GameBoard
    {
        public GameBoard(int height, int width)
        {
            Height = height;
            Width = width;
            Rocks = Rock.CreateRocks(10, width, height, Snake);
            Apple = Apple.CreateApple(width, height, Snake, Rocks);

        }
        public int Height { get; }
        public int Width { get; }

        public Snake Snake { get; set; } = new Snake(1, 1, 3);
        public List<Rock> Rocks { get; set; }
        public Apple Apple { get; set; }
        public void NextStep ()
        {
            if (CurrentDirection == Direction.Suspend)
            {
                return;
            }

            var snakeTail = Snake.SnakeTail;
            var snakeHead = Snake.SnakeHead;
            snakeTail.X = snakeHead.X;
            snakeTail.Y = snakeHead.Y;
            switch (CurrentDirection)
            {
                case Direction.Left:
                    snakeTail.X--;
                    break;
                case Direction.Up:
                    snakeTail.Y--;
                    break;
                case Direction.Right:
                    snakeTail.X++;
                    break;
                case Direction.Down:
                    snakeTail.Y++;
                    break;
            }
            Snake.SnakeElements.Remove(snakeTail);
            Snake.SnakeElements.Insert(0, snakeTail);
        }
        public Direction CurrentDirection { private get; set; } = Direction.Suspend;


    }
}
