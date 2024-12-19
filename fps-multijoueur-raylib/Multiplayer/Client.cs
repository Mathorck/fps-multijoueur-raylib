using DeadOpsArcade3D.GameElement;
using System.Net.Sockets;
using System.Text;
using DeadOpsArcade3D.Game;
using Raylib_cs;

namespace DeadOpsArcade3D.Multiplayer
{
    class Client
    {
        private static TcpClient client;
        public static NetworkStream stream { get; private set; }
        //private static Model alien = LoadModel("ressources/model3d/alien/alien.obj");
        // private static Dictionary<int, (float x, float y, float z, float xRot, float yRot, float zRot)> otherPlayers = new Dictionary<int, (float x, float y, float z, float xRot, float yRot, float zRot)>();
        private static List<Player> playerList = new List<Player>();

        /// <summary>
        /// Cela démmarre le Jeu 
        /// </summary>
        /// <param name="host">Adresse Ip du serveur</param>
        /// <param name="port">le port</param>
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
        /// <exception cref="ArgumentException">Ce n'est pas censé arriver</exception>
        private static void ReceiveMessages()
        {
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
                    playerList.Clear();
                    string[] tempTbl = message.Split("/");
                    string[] allPositions = tempTbl[0].Split(';');
                    for (int i = 0; i < allPositions.Length; i++)
                    {
                        try
                        {
                            allPositions[i] = allPositions[i].Replace("[", "").Replace("]", "");
                            string[] parts = allPositions[i].Split(',');
                            if (parts.Length == 7)
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

                                playerList.Add(new Player(X, Y, Z, Xrot, Yrot, Zrot));
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
            string position = camera.Position.X + "," + camera.Position.Y + "," + camera.Position.Z + "," + camera.Target.X + "," + camera.Target.Y + "," + camera.Target.Z;
            byte[] data = Encoding.UTF8.GetBytes(position);
            Client.stream.Write(data, 0, data.Length);
        }
    }
}