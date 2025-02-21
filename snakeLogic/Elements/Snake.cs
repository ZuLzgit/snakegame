using System.Collections.Generic;
using System.Linq;
using snakeLogic.Enum;


namespace snakeLogic.Elements
{
    public class Snake
    {
        public string HexHeadColor { get; set; }
        public string HexBodyColor1 { get; set; }
        public string HexBodyColor2 { get; set; }
        public Snake(int x, int y, int length, SnakeType snakeType, string hexHeadColor, string hexBodyColor1, string hexBodyColor2)
        {

            HexHeadColor = hexHeadColor;
            HexBodyColor1 = hexBodyColor1;
            HexBodyColor2 = hexBodyColor2;
            SnakeType = snakeType;
            SnakeElements.Add(new Position(x, y));

            var direction = Direction.Left;
            var segments = 0;
            var steps = 1;
            bool addStep = true;
            SnakeType = snakeType;

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
        public SnakeType SnakeType { get; }
        public Direction SnakeDirection { get; set; } = Direction.Suspend;

        public string GetHexElementColor(int index)
        {
            return index == 0 ? HexHeadColor : (index % 2 == 0 ? HexBodyColor1 : HexBodyColor2);

        }
    }
}

