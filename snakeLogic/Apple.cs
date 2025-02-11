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
        public static List<Apple> CreateApples(int appleCount, int boardWidth, int boardHeight, Snake snake, List<Rock> rocks = null)
        {
            List<Apple> apples = new List<Apple>();
            Random random = new Random();
            while (apples.Count < appleCount)
            {
                int x = random.Next(1, boardWidth - 2);  
                int y = random.Next(1, boardHeight - 2); 

                
                if (!snake.SnakeElements.Any(e => e.X == x && e.Y == y) &&
                    (rocks == null || !rocks.Any(r => r.X == x && r.Y == y)) &&
                    !apples.Any(a => a.X == x && a.Y == y)) 
                {
                    apples.Add(new Apple(x, y));
                }
            }

            return apples;
        }
        public static bool CheckCollisionWithApple(Position snakeHead, List<Apple> apples, out Apple eatenApple)
        {
            eatenApple = apples.FirstOrDefault(apple => apple.X == snakeHead.X && apple.Y == snakeHead.Y);
            return eatenApple != null;

        }
    }
}
