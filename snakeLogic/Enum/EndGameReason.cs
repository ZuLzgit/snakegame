using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snakeLogic.Enum
{
    public enum EndGameReason
    {
        None,
        WallCollision,
        RockCollision,
        SelfCollision,
        SnakeCollision
    }
}

