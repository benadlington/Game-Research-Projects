using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Server.Objects
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Player
    {
        public Position position { get; set; }
        public int speed { get; set; }
        public string name { get; set; }
    }
}
