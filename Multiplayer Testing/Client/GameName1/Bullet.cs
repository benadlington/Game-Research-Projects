using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameName1
{
    class Bullet
    {
        public string owner { get; set; }
        public Point origin { get; set; }
        public Point bulletPosition;
        public int speed { get; set; }
        public string direction { get; set; }
        //public Texture2D texture { get; set; }


        public Bullet()
        {

        }
        public Bullet(string _owner, Point _origin, Point _position, int _speed, string _direction, Texture2D _texture)
        {
            owner = _owner;
            origin = _origin;
            bulletPosition = _position;
            speed = _speed;
            direction = _direction;
            //texture = _texture;
        }
    }
}
