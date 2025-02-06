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
            for (var i = 0; i < length; i++)
            {
                SnakeElements.Add(new Position(1, i));
            }
        }
        public List<Position> SnakeElements { get; } = new List<Position>();
        public Position SnakeHead
        {
            get
            {
                return SnakeElements.First();
            }
        }
        public Position SnakeTail
        {
            get
            {
                return SnakeElements.Last();
            }
        }
    }
}
