using System.Net.Sockets;
using System.Net;
using System.Text;

namespace DeadOpsArcade3D.Multiplayer
{
    class Server
    {
        private static TcpListener server;
        private static Dictionary<int, TcpClient> clients = new Dictionary<int, TcpClient>();
        private static int clientCounter = 0;

        private static Dictionary<int, string> playerPositions = new Dictionary<int, string>();

        /// <summary>
        /// Cela démarre le serveur
        /// </summary>
        /// <param name="port">le ports</param>
        public static void StartServer(int port)
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Console.WriteLine("Serveur démarré sur le port " + port);

            Thread acceptThread = new Thread(AcceptClients);
            acceptThread.Start();
        }

        private static void AcceptClients()
        {
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                clients[clientCounter] = client;
                //                               | pos | rot |
                playerPositions[clientCounter] = "0,0,0,0,2,0"; // Position initiale
                Console.WriteLine("Nouveau client connecté : " + clientCounter);
                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(clientCounter);
                clientCounter++;
            }
        }

        private static void HandleClient(object clientIdObj)
        {
            int clientId = (int)clientIdObj;
            TcpClient client = clients[clientId];

            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[256];

            try
            {
                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Client {clientId}: {message}");

                        // Mise à jour de la position du joueur
                        playerPositions[clientId] = message;

                        // Diffuser les positions de tous les joueurs
                        foreach (var Client in clients)
                        {
                            string response = "";
                            foreach (KeyValuePair<int, string> pair in playerPositions)
                            {
                                if (!(pair.Key == Client.Key))
                                {
                                    response += "[" + pair.Key + ", " + pair.Value + "]; ";
                                }
                            }
                            sendPrivately(response, Client.Value);
                        }


                        //string allPositions = string.Join(";", playerPositions);


                        //BroadcastMessage(allPositions + "/");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Client déconnecté : " + clientId);
                clients.Remove(clientId);
                playerPositions.Remove(clientId);
            }
        }

        /// <summary>
        /// Cela envoye un message par le réseau a un Appareil
        /// </summary>
        /// <param name="message">Message à envoyer</param>
        /// <param name="recever">Appareil qui reçoit</param>
        private static void sendPrivately(string message, TcpClient recever)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            recever.GetStream().Write(data, 0, data.Length);
        }

        /// <summary>
        /// Cela envoye un message a toutes les personnes qui sont connectée
        /// </summary>
        /// <param name="message"></param>
        private static void BroadcastMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (var client in clients.Values)
            {
                client.GetStream().Write(data, 0, data.Length);
            }
        }
    }
}
