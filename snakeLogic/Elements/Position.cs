using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snakeLogic.Elements
{
    public class Position
    {
        public Position(int x, int y )
        {
            X = x;
            Y = y;
        }
        public bool CheckCollision(Position position)
        {
            return this.X == position.X
                && this.Y == position.Y;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int CalcDistance(Position position) 
        {
            return Math.Abs(position.X - this.X) + Math.Abs(position.Y - this.Y);

        }

    }
}
