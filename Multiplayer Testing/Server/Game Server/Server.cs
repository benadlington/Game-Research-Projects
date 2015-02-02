using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using Game_Server.Objects;

namespace Game_Server
{
    class Server
    {
        List<IPEndPoint> ClientList = new List<IPEndPoint>();
        List<Player> PlayerList = new List<Player>();
        Player[] myArray = new Player[10];
        UdpClient udpServer;
        public void Init()
        {
            Thread listenForClients = new Thread(new ThreadStart(listenForClientsThread));
            listenForClients.Start();
        }

        private void listenForClientsThread()
        {

            udpServer = new UdpClient(7124);
            Thread handleClient;
            IPEndPoint EP = new IPEndPoint(IPAddress.Any, 7124);
            Console.WriteLine("Listening for clients..");
            while (true)
            {
                byte[] data = udpServer.Receive(ref EP);
                string message = Encoding.ASCII.GetString(data);
                if (!ClientList.Contains(EP) && message == "ping")
                {
                    Console.WriteLine("New Client attempted Connection!");
                    if(ClientList.Count < 2)
                    {
                        HandleConnection(EP, "pong");
                        ClientList.Add(EP);
                        handleClient = new Thread(new ParameterizedThreadStart(handleClientThread));
                        handleClient.Start(EP);
                        Console.WriteLine("Creating thread for :" + EP);
                    }
                    else
                    {
                        Console.WriteLine("Refusing connection, server full");
                        HandleConnection(EP, "refuse");
                    }
                }
            }
        }


        private void handleClientThread(object EP)
        {
            IPEndPoint clientIP = (IPEndPoint)EP;
            bool running = true;
            while (running)
            {
                byte[] data = null;
                try
                {
                    data = (udpServer.Receive(ref clientIP));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                string message = Encoding.ASCII.GetString(data);
                Player player = new Player();
                if (message != "ping" && !message.Contains("disconnect") && !message.Contains("bullet"))
                {
                    try
                    {
                        player = JsonConvert.DeserializeObject<Player>(message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(message);
                    }

                }
                else if (message.Contains("origin"))
                {
                    try
                    {
                        Bullet bullet = JsonConvert.DeserializeObject<Bullet>(message);
                        SendBulletData(clientIP, bullet);
                    }
                    catch (Exception)
                    {
                        
                        throw;
                    }
                }
                if(player.name != null)
                {
                    Boolean ok = PlayerList.Any(s => s.name == player.name);
                    if (ok == false)
                    {
                        PlayerList.Add(player);
                        Console.WriteLine("Adding player: " + player.name);
                    }
                    try
                    {
                        var toUpdate = PlayerList.Single(x => x.name == player.name);
                        toUpdate.position = player.position;
                    }
                    catch (Exception)
                    {
                        
                    }
                    sendDataToClient(clientIP);

                }
                if (message.Contains("disconnect"))
                {
                    //disconnect
                    List<string> myList = message.Split(' ').ToList();
                    for (int i = 0; i <= PlayerList.Count; i++)
                    {
                        if (PlayerList[i].name == myList[1])
                        {
                            try
                            {
                                var p = PlayerList[i];
                                PlayerList.Remove(p);
                                Console.WriteLine("Removing player: " + p.name);
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                    ClientList.Remove(clientIP);
                    Console.WriteLine("Client Removed: " + clientIP);
                    Console.WriteLine("Client Disconnected {" + clientIP + "}");
                    running = false;
                    Thread.CurrentThread.Abort();
                }
            }
        }
        private void sendDataToClient(IPEndPoint EP)
        {
            string message;
            byte[] data;
            try
            {
                message = JsonConvert.SerializeObject(PlayerList);
                data = Encoding.ASCII.GetBytes(message);
                udpServer.Send(data, data.Length, EP);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void SendBulletData(IPEndPoint EP, Bullet bullet)
        {
            string message;
            byte[] data;
            foreach(var c in ClientList)
            {
                try
                {
                    message = JsonConvert.SerializeObject(bullet);
                    data = Encoding.ASCII.GetBytes(message);
                    udpServer.Send(data, data.Length, c);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }
        private void HandleConnection(IPEndPoint EP, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            udpServer.Send(data, data.Length, EP);
        }
    }
}
