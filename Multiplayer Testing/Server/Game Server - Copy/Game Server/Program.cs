using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Game_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server myServer = new Server();
            myServer.Init();
        }

    }
}
