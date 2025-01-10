using DeadOpsArcade3D.GameElement;
using System.Net.Sockets;
using System.Text;
using DeadOpsArcade3D.Game;
using Raylib_cs;
using System.ComponentModel.Design;

namespace DeadOpsArcade3D.Multiplayer
{
    class Client
    {
        private static bool sendFire = false;
        private static TcpClient client;
        public static NetworkStream stream { get; private set; }

        /// <summary>
        /// Cela démmarre le Jeu 
        /// </summary>
        /// <param name="host">Adresse Ip du serveur</param>
        /// <param name="port"></param>
        public static void StartClient(string host, int port)
        {
            client = new TcpClient();
            client.Connect(host, port);
            Console.WriteLine("Connecté au serveur");
            stream = client.GetStream();

            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();

            GameLoop.StartGame();
        }

        /// <summary>
        /// Cela permet de recevoir les messages du serveur
        /// </summary>
        private static void ReceiveMessages()
        {
            Weapon Default = new Weapon();
            byte[] buffer = new byte[256];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Message reçu : " + message);

                    // Mise à jour des positions des autres joueurs
                    //otherPlayers.Clear();
                    List<Player>  playerList= new List<Player>();
                    foreach (Player player in Player.PlayerList)
                    {
                        playerList.Add(player);
                    }

                    Player.PlayerList.Clear();
                    string[] tempTbl = message.Split("/");
                    string[] allPositions = tempTbl[0].Split(';');
                    for (int i = 0; i < allPositions.Length; i++)
                    {
                        try
                        {
                            allPositions[i] = allPositions[i].Replace("[", "").Replace("]", "");
                            string[] parts = allPositions[i].Split(',');
                            if (parts.Length == 8)
                            {
                                if (!int.TryParse(parts[0], out int id))
                                    throw new ArgumentException("Erreur");

                                if (!float.TryParse(parts[1], out float X))
                                    throw new ArgumentException("Erreur");

                                if (!float.TryParse(parts[2], out float Y))
                                    throw new ArgumentException("Erreur");

                                if (!float.TryParse(parts[3], out float Z))
                                    throw new ArgumentException("Erreur");

                                if (!float.TryParse(parts[4], out float Xrot))
                                    throw new ArgumentException("Erreur");

                                if (!float.TryParse(parts[5], out float Yrot))
                                    throw new ArgumentException("Erreur");

                                if (!float.TryParse(parts[6], out float Zrot))
                                    throw new ArgumentException("Erreur");

                                if (!bool.TryParse(parts[7], out bool Fired))
                                    throw new ArgumentException("Erreur");

                                Player.PlayerList.Add(new Player(X, Y, Z, Xrot, Yrot, Zrot));
                                for (int j = 0; j < Player.PlayerList.Count && j < playerList.Count(); j++)
                                {
                                    if (Player.PlayerList[j].Position.Y != playerList[j].Position.Y)
                                    {
                                        Player.PlayerList[j].animIndex = 3;
                                        Player.PlayerList[i].animCurrentFrame = playerList[j].animCurrentFrame;
                                    }  
                                    else if (Player.PlayerList[j].Position.X != playerList[j].Position.X || Player.PlayerList[j].Position.Z != playerList[j].Position.Z)
                                    {
                                        Player.PlayerList[j].animIndex = 10;
                                        Player.PlayerList[i].animCurrentFrame = playerList[j].animCurrentFrame;
                                    }
                                }

                                if (Fired)
                                    Bullet.BulletsList.Add(new Bullet(new(X,Y,Z),new( Xrot, Yrot,Zrot),Default));
                                //otherPlayers.Add(id, (X, Y, Z, Xrot, Yrot, Zrot));
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Permet d'envoyer les informations du joueur au serveur
        /// </summary>
        /// <param name="camera">toutes les info du joueur</param>
        public static void SendInfo(Camera3D camera)
        {
            string position = camera.Position.X + "," + camera.Position.Y + "," + camera.Position.Z + "," + camera.Target.X + "," + camera.Target.Y + "," + camera.Target.Z + "," + sendFire;
            sendFire = false;
            byte[] data = Encoding.UTF8.GetBytes(position);
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Met la variable sendFire à true
        /// </summary>
        public static void Fire()
        {
            sendFire = true;
        }

        #region Console Message
        
        /// <summary>
        /// Crée un message d'erreur dans la console
        /// </summary>
        /// <param name="message"></param>
        public static void ConsoleError(string message)
        {
            Consolelog(message, ConsoleColor.Red);
        }
        
        /// <summary>
        /// Crée un message d'information dans la console
        /// </summary>
        /// <param name="message"></param>
        public static void ConsoleInfo(string message)
        {
            Consolelog(message, ConsoleColor.White);
        }
        
        /// <summary>
        /// Crée un message de succès dans la console
        /// </summary>
        /// <param name="message"></param>
        public static void ConsoleSuccess(string message)
        {
            Consolelog(message, ConsoleColor.Green);
        }
        
        /// <summary>
        /// Crée un message d'avertissement dans la console
        /// </summary>
        /// <param name="message"></param>
        public static void ConsoleWarning(string message)
        {
            Consolelog(message, ConsoleColor.Yellow);
        }
        private static void Consolelog(string message, ConsoleColor Color)
        {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            Console.WriteLine(message);
            Console.ForegroundColor = old;
        }
        
        #endregion
    }
}