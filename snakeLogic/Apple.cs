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
        
        public static bool CheckCollisionWithApple(Position snakeHead, List<Apple> apples, out Apple eatenApple)
        {
            eatenApple = apples.FirstOrDefault(apple => apple.X == snakeHead.X && apple.Y == snakeHead.Y);
            return eatenApple != null;

        }
    }
}
