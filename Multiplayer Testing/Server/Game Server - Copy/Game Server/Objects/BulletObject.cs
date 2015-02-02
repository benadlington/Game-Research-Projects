using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Server.Objects
{
    public class BulletPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Origin
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Bullet
    {
        public BulletPosition bulletPosition { get; set; }
        public string owner { get; set; }
        public Origin origin { get; set; }
        public int speed { get; set; }
        public string direction { get; set; }
    }
}
