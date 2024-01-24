using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game1
{
    class Network
    {

        public static IPAddress GetIpAddress()
        {
            foreach (var ipAddress in Dns.GetHostAddresses("localhost").ToList())
            {
                if (ipAddress.AddressFamily.ToString() == AddressFamily.InterNetwork.ToString() && PingAddress("google.com", 1000))
                {
                    Debug.WriteLine("MY IP " + ipAddress.ToString());
                    return IPAddress.Parse("10.158.176.63");
                }
            }
            throw new Exception("could not find IP");
        }
        public static void Client(IPAddress ipAddress, int port, byte[] data, int offset)//send
        {
            try
            {
                TcpClient client = new TcpClient(ipAddress.ToString(), port);
                NetworkStream stream = client.GetStream();
                //Console.WriteLine("Sending data");
                stream.Write(data, offset, data.Length);
                client.Close();
                //Console.WriteLine("Closed Client stream");
            }
            catch (Exception e)
            {
                //Debug.WriteLine(e);
            }
        }

        #region PING

        public static bool PingAddress(string address, int timeout)
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(address, timeout, GenerateData(10));
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed To Ping {address}");
                return false;
            }

        }

        public static byte[] GenerateData(int length)
        {
            byte[] arr = new byte[length];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (byte)Game1.RNG.Next(0, 256);
            }

            return arr;
        }

        #endregion

        public static void MultithreadeServer(IPAddress ipAddress, int port, List<Sprite> _sprites, GameObjects gameObjects)
        {
            TcpListener listener = new TcpListener(ipAddress, port);
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                ClientStream(client, _sprites, gameObjects);
                //new Thread((() => ClientStream(client, _sprites, gameObjects))).Start();
            }
        }

        public static void ClientStream(TcpClient client, List<Sprite> _sprites, GameObjects gameObjects)//receive data 
        {
            IPAddress Sender = IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
            try
            {

                byte[] bytes = new byte[256];
                string data = null;
                //Console.WriteLine("Connected");
                NetworkStream stream = client.GetStream();
                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    //Console.WriteLine($"RECIVED DATA:{data}");
                }

                client.Close();
                //Debug.WriteLine($"RETURNED DATA:{data}");
                
                DataHandle(data, _sprites, gameObjects, Sender);
            }
            catch (Exception e)
            {

            }
        }
        static public List<IPAddress> AllClients = new List<IPAddress>();
        static public Dictionary<IPAddress,Ship> Players = new Dictionary<IPAddress,Ship>();
        private static void DataHandle(string _data, List<Sprite> _sprites, GameObjects gameObjects, IPAddress Sender)
        {
            string OT = ""; //object type
            int HP = 0;
            bool IsPlayer = false;
            float X = 0, Y = 0, R = 0, S = 0;
            string[] data = _data.Split(';');
            int objNum = int.MaxValue;
            for (int i = 0; i < data.Length; i++)
            {
                string[] d = data[i].Split(':');

                switch (d[0])
                {
                    case "Player":
                        IsPlayer = true;
                        break;
                    case "First":
                        AllClients.Add(IPAddress.Parse(d[1]));
                        Players.Add(Sender,gameObjects.ship.Clone() as Ship);
                        return;
                    case "X": //position X
                        X = float.Parse(d[1]);
                        break;
                    case "Y": //position Y
                        Y = float.Parse(d[1]);
                        break;
                    case "R": //rotation
                        R = float.Parse(d[1]);
                        break;
                    case "S": //shoot
                        S = float.Parse(d[1]); //FIX OR REMOVE
                        break;
                    case "OT": //object type
                        OT = d[1];
                        break;
                    case "HP": //health
                        HP = int.Parse(d[1]);
                        break;
                    case "O":
                        objNum = int.Parse(d[1]);
                        break;
                }
            }
            if(IsPlayer == true)
            {
                Players[Sender].Position = new Vector2(X, Y);
                Players[Sender]._rotation = R;
                Players[Sender].Health = HP;

            }
            //add
            else if (objNum >= _sprites.Count)
            {
                //Debug.WriteLine("Creating object");
                Vector2 position = new Vector2(X, Y);

                Sprite sprite = null;
                switch (OT)
                {
                    case "Ship":
                        sprite = gameObjects.ship.Clone() as Ship;
                        break;
                    case "Enemy":
                        sprite = gameObjects.enemy.Clone() as Enemy;
                        break;
                    case "Bullet":
                        sprite = gameObjects.bullet.Clone() as Bullet;
                        break;
                    case "Missile":
                        sprite = gameObjects.missile.Clone() as Missile;
                        break;
                    case "HealthKit":
                        sprite = gameObjects.healthKit.Clone() as HealthKit;
                        break;
                }
                sprite.Position = position;
                sprite._rotation = R;
                _sprites.Add(sprite);
            }
            else
            {

                _sprites[objNum].Position.X = X;
                _sprites[objNum].Position.Y = Y;
                _sprites[objNum]._rotation = R;
                //add health
            }
        }


    }
    public class GameObjects
    {
        public Ship ship;
        public Enemy enemy;
        public Bullet bullet;
        public Missile missile;
        public HealthKit healthKit;
    }
}