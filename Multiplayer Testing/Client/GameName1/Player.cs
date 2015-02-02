using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GameName1
{
    class Player
    {
        public Point position;
        public int speed = 5;
        public string name;
        public void GetName()
        {
            StreamReader myReader = new StreamReader("name.txt");
            name = myReader.ReadLine();
        }
    }
}
