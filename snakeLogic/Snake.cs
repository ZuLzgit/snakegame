using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snakeLogic
{
    public class Snake
    {
        public Snake(int x, int y, int length)
        {
            SnakeElements.Add(new Position(x, y));

            var direction = Direction.Left;
            var segments = 0;
            var steps = 1;
            bool addStep = true;

            for (int i = 1; i < length; i++)
            {
                switch (direction)
                {
                    case Direction.Left:
                        x--;
                        break;
                    case Direction.Right:
                        x++;
                        break;
                    case Direction.Up:
                        y++;
                        break;
                    case Direction.Down:
                        y--;
                        break;
                }
                SnakeElements.Insert(0, new Position(x, y));
                segments++;
                if (segments == steps)
                {
                    segments = 0;
                    addStep = !addStep;
                    direction = getNextDirection(direction);
                    if (addStep)
                    {
                        steps++;
                    }
                }
                

            }
        }
        private static Direction getNextDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return Direction.Up;
                case Direction.Right:
                    return Direction.Down;
                case Direction.Up:
                    return Direction.Right;
                case Direction.Down:
                    return Direction.Left;
            }
            return Direction.Left;
        }
        public List<Position> SnakeElements { get; } = new List<Position>();

        public Position SnakeHead => SnakeElements.First();
        public Position SnakeTail => SnakeElements.Last();
    }
}

