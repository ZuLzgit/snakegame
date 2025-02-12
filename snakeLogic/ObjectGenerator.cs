using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace snakeLogic
{
    internal class ObjectGenerator
    {

        public static void CreateItems(int itemCount, int boardWidth, int boardHeight, Snake snake, ObjectType objectType, List<Rock> rocks = null, List<Apple> apples = null) 
        {
            Random random = new Random();
            for (int i = 0; i < itemCount;)
            {
                int x = random.Next(1, boardWidth - 2);
                int y = random.Next(1, boardHeight - 2);


                if (!snake.SnakeElements.Any(e => e.X == x && e.Y == y) &&
                    (rocks == null || !rocks.Any(r => r.X == x && r.Y == y)) &&
                    (apples == null || !apples.Any(r => r.X == x && r.Y == y)))
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
