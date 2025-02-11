using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snakeLogic
{
    public class Rock : Position
    {
        public Rock(int x, int y) : base(x, y)
        {
        }
        public static List<Rock> CreateRocks(int rockCount, int boardWidth, int boardHeight, Snake snake)
        {
            List<Rock> rocks = new List<Rock>();
            Random random = new Random();

            for (int i = 0; i < rockCount; i++)
            {
                int x = random.Next(0, boardWidth -2);
                int y = random.Next(0, boardHeight -2);

                // Ensure that the rock is not placed on the snake.
                while (snake.SnakeElements.Any(e => e.X == x && e.Y == y))
                {
                    x = random.Next(0, boardWidth);
                    y = random.Next(0, boardHeight);
                }

                rocks.Add(new Rock(x, y));
            }
            return rocks;
        }
    }
}
