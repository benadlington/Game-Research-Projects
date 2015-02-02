using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace GameName1
{
    class ContactServer
    {
        static UdpClient Client = new UdpClient();
        //server 31.205.0.233
        //my pc 10.46.193.137
        static IPEndPoint ep = new IPEndPoint(IPAddress.Parse("31.205.0.233"), 7124);
        //public static string GetAllPositions()
        //{

        //}

        public static Boolean ping()
        {
            bool connect = false;
            Client.Connect(ep);
            string message = ("ping");
            Byte[] data = Encoding.ASCII.GetBytes(message);
            Client.Send(data, data.Count());
            bool b = false;
            while(b == false)
            {
                byte[] receivedData = Client.Receive(ref ep);
                string m = Encoding.ASCII.GetString(receivedData);
                if (m == "pong")
                {
                    connect = true;
                    b = true;
                    break;
                }
                else if(m == "refuse")
                {
                    connect = false;
                }
            }
            return connect;

        }
        public static void disconnect(string name)
        {
            Client.Connect(ep);
            string message = "disconnect " + name;
            byte[] data = Encoding.ASCII.GetBytes(message);
            Client.Send(data, data.Count());
        }
        public static void sendPosition(Point position)
        {
            Client.Connect(ep);
            string message = (position.X + "," + position.Y).ToString();
            Byte[] data = Encoding.ASCII.GetBytes(message);
            Client.Send(data, data.Count());
        }
        public static void sendData(Player player)
        {
            Client.Connect(ep);
            string message = JsonConvert.SerializeObject(player);
            Byte[] data = Encoding.ASCII.GetBytes(message);
            Client.Send(data, data.Count());
        }
        public static void sendBulletData(Bullet bullet)
        {
            Client.Connect(ep);
            string message = JsonConvert.SerializeObject(bullet);
            byte[] data = Encoding.ASCII.GetBytes(message);
            Client.Send(data, data.Count());
        }
        public static string Receive()
        {
            byte[] receivedData = Client.Receive(ref ep);
            string message = Encoding.ASCII.GetString(receivedData);
            return message;
        }
    }
}
