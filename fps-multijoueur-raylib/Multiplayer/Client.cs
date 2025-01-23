using System.Net.Sockets;
using System.Numerics;
using System.Text;
using DeadOpsArcade3D.Game;
using DeadOpsArcade3D.Game.GameElement;
using DeadOpsArcade3D.Launcher;

namespace DeadOpsArcade3D.Multiplayer;

internal class Client
{
    private static bool sendFire;
    private static TcpClient client;
    public static NetworkStream stream { get; private set; }

    /// <summary>
    ///     Cela démmarre le Jeu
    /// </summary>
    /// <param name="host">Adresse Ip du serveur</param>
    /// <param name="port"></param>
    public static void StartClient(string host, int port)
    {
        ConsoleWarning("Starting client ...");
        try
        {
            client = new TcpClient();
            client.Connect(host, port);
        }
        catch (Exception e)
        {
            ConsoleError($"Failed to connect to {host} \n{e}");
            Launcher.Launcher.Init();
            return;
        }
        ConsoleSuccess("Connecté au serveur");
        stream = client.GetStream();


        Thread receiveThread = new(ReceiveMessages);
        receiveThread.Start();

        GameLoop.StartGame();
    }

    /// <summary>
    ///     Cela permet de recevoir les messages du serveur
    /// </summary>
    private static void ReceiveMessages()
    {
        Weapon Default = new();
        byte[] buffer = new byte[256];
        while (true)
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                ConsoleInfo($"Message brut reçu ({bytesRead} octets) : {message}");

                // Mise à jour des positions des autres joueurs
                // Player.PlayerList.Clear();
                string[] tempTbl = message.Split("/");
                string[] allPositions = tempTbl[0].Split(';');
                for (int i = 0; i < allPositions.Length; i++)
                {
                    try
                    {
                        allPositions[i] = allPositions[i].Replace("[", "").Replace("]", "");
                        string[] parts = allPositions[i].Split(',');

                        if (parts.Length == 11)
                        {
                            #region Parsing multiplayer
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

                            if (string.IsNullOrEmpty(parts[7]))
                                throw new ArgumentException("Erreur");
                            string pseudo = parts[7];

                            if (!bool.TryParse(parts[8], out bool Fired))
                                throw new ArgumentException("Erreur");

                            if (!int.TryParse(parts[9], out int animIndex))
                                throw new ArgumentException("Erreur");

                            if (!int.TryParse(parts[10], out int animCurrentFrame))
                                throw new ArgumentException("Erreur");
                            #endregion

                            if (id > Player.PlayerList.Count - 1)
                            {
                                Player.PlayerList.Add(new Player(X, Y, Z, Xrot, Yrot, Zrot, pseudo, animIndex, animCurrentFrame));
                                continue;
                            }

                            try
                            {
                                Player.PlayerList[id].Position = new Vector3(X, Y, Z);
                                Player.PlayerList[id].Rotation = new Vector3(Xrot, Yrot, Zrot);
                                Player.PlayerList[id].Pseudo = pseudo;
                                Player.PlayerList[id].animIndex = animIndex;
                                Player.PlayerList[id].animCurrentFrame = animCurrentFrame;
                            }
                            catch (Exception e)
                            {
                                ConsoleError($"Erreur Client.cs  : {e}");
                            }
                            

                            if (Fired)
                                Bullet.BulletsList.Add(new Bullet(new Vector3(X, Y, Z), new Vector3(Xrot, Yrot, Zrot),
                                    Default, Player.PlayerList[id]));
                            //otherPlayers.Add(id, (X, Y, Z, Xrot, Yrot, Zrot));
                        }
                    }
                    catch (Exception e)
                    {
                        ConsoleError($"Erreur Client.cs  : {e}");
                    }
                }
            }
        }
    }

    /// <summary>
    ///     Permet d'envoyer les informations du joueur au serveur
    /// </summary>
    /// <param name="camera">toutes les info du joueur</param>
    public static void SendInfo(Camera3D camera, int animIndex, int animCurrentFrame)
    {
        string position = float.Round(camera.Position.X, 4) + "," + float.Round(camera.Position.Y, 4) + "," + float.Round(camera.Position.Z, 4) + "," +
                          float.Round(camera.Target.X, 4) + "," + float.Round(camera.Target.Y, 4) + "," + float.Round(camera.Target.Z, 4) + "," +
                          Player.Nom + "," + sendFire + "," + animIndex + "," + animCurrentFrame;
        sendFire = false;
        byte[] data = Encoding.UTF8.GetBytes(position);
        stream.Write(data, 0, data.Length);
    }

    /// <summary>
    ///     Met la variable sendFire à true
    /// </summary>
    public static void Fire()
    {
        sendFire = true;
    }

    #region Console Message

    /// <summary>
    ///     Crée un message d'erreur dans la console
    /// </summary>
    /// <param name="message"></param>
    public static void ConsoleError(string message)
    {
        Consolelog(message, ConsoleColor.Red);
        //Gui.ErrorContent.Add(message);
        if (!File.Exists("Errorlog.txt"))
            File.Create("Errorlog.txt");
        StreamWriter writer = new StreamWriter("Errorlog.txt", true);
        writer.WriteLine(message);
        writer.Close();
    }

    /// <summary>
    ///     Crée un message d'information dans la console
    /// </summary>
    /// <param name="message"></param>
    public static void ConsoleInfo(string message)
    {
        Consolelog(message, ConsoleColor.White);
    }

    /// <summary>
    ///     Crée un message de succès dans la console
    /// </summary>
    /// <param name="message"></param>
    public static void ConsoleSuccess(string message)
    {
        Consolelog(message, ConsoleColor.Green);
    }

    /// <summary>
    ///     Crée un message d'avertissement dans la console
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
        Console.WriteLine("INFO: GAME: "+message);
        Console.ForegroundColor = old;
    }

    #endregion
}