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
            var freeElements = new List<Position>();
            for (int x = 0; x < boardWidth; x++)
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    bool occupiedBySnake = snakes.Any(snake => snake.SnakeElements.Any(se => se.X == x && se.Y == y));
                    bool occupiedByApple = apples != null && apples.Any(a => a.X == x && a.Y == y);
                    bool occupiedByRock = rocks != null && rocks.Any(r => r.X == x && r.Y == y);

                    if (!occupiedBySnake && !occupiedByApple && !occupiedByRock)
                    {
                        var position = new Position(x, y);
                        freeElements.Add(position);
                    }

                }
            }

            for (int i = 0; i < itemCount; i++)
            {
                if (freeElements.Count == 0)
                {
                    break;
                }
                var idx = random.Next(freeElements.Count);
                var randomFreeElement = freeElements.ElementAt(idx);

                switch (objectType)
                {
                    case ObjectType.Apple:
                        apples.Add(new Apple(randomFreeElement.X, randomFreeElement.Y));
                        break;
                    case ObjectType.Stone:
                        rocks.Add(new Rock(randomFreeElement.X, randomFreeElement.Y));
                        break;
                }
                freeElements.Remove(randomFreeElement);

            }
            
              
        }

    }
}
