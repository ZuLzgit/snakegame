using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using snakeLogic.Elements;
using snakeLogic.Enum;

namespace snakeLogic
{
    internal class ObjectGenerator
    {

        public static void CreateItems(int itemCount, int boardWidth, int boardHeight, List<Snake> snakes, ObjectType objectType, List<Rock> rocks = null, List<Apple> apples = null) 
        {
            Random random = new Random();
            for (int i = 0; i < itemCount;)
            {
                int x = random.Next(0, boardWidth);
                int y = random.Next(0, boardHeight);


                bool occupiedBySnake = snakes.Any(snake => snake.SnakeElements.Any(e => e.X == x && e.Y == y));


                if (!occupiedBySnake &&
                    (rocks == null || !rocks.Any(r => r.X == x && r.Y == y)) &&
                    (apples == null || !apples.Any(a => a.X == x && a.Y == y)))
                {
                    switch (objectType)
                    {
                        case ObjectType.Apple:
                            apples.Add(new Apple(x, y));
                            break;
                        case ObjectType.Stone:
                            rocks.Add(new Rock(x, y));
                            break;
                    }
                    i++;
                }
            }
        }

    }
}
