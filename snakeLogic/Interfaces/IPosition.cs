using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using snakeLogic.Elements;

namespace snakeLogic.Interfaces
{
    public interface IPosition
    {
        bool CheckCollision(Position position);

        int X { get; set; }
        int Y { get; set; }
        int CalcDistance(Position position);

    }
}
