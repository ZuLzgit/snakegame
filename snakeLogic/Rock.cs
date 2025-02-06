using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snakeLogic
{
    public class Rock : Position
    {
        public Rock(int x, int y) : base(x, y)
        {
        }
        public List<Rock> Rocks { get; set; } = new List<Rock>(); 
    }
}
