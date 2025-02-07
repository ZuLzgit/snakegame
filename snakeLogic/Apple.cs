using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snakeLogic
{
    public class Apple : Position
    {
        public Apple(int x, int y) : base(x, y)
        {
        }
        public static Apple CreateApple(int boardWidth, int boardHeight, Snake snake, List<Rock> rocks = null)
        {
            Random random = new Random();
            int x = random.Next(0, boardWidth);
            int y = random.Next(0, boardHeight);

            //damit der apfel nicht auf dem stein oder schlange spawnt
            while (snake.SnakeElements.Any(e => e.X == x && e.Y == y) ||
                   (rocks != null && rocks.Any(r => r.X == x && r.Y == y)))
            {
                x = random.Next(0, boardWidth);
                y = random.Next(0, boardHeight);
            }

            return new Apple(x, y);
        }
    }
}
