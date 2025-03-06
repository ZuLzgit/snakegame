using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snakeLogic.Elements
{
    public class Teleporter
    {
        public Position Entry { get; set; }
        public Position Exit { get; set; }

        public Teleporter(Position entry, Position exit)
        {
            Entry = entry;
            Exit = exit;
        }
    }
}
