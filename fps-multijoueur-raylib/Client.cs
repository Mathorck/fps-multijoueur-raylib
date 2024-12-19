using Classes;
using static Raylib_cs.Raylib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace DeadOpsArcade3D
{
    class Client
    {
        private static TcpClient client;
        private static NetworkStream stream;
        //private static Model alien = LoadModel("ressources/model3d/alien/alien.obj");
        // private static Dictionary<int, (float x, float y, float z, float xRot, float yRot, float zRot)> otherPlayers = new Dictionary<int, (float x, float y, float z, float xRot, float yRot, float zRot)>();
        private static List<Player> playerList = new List<Player>();

        public static void StartClient(string host, int port)
        {
            client = new TcpClient();
            client.Connect(host, port);
            Console.WriteLine("Connecté au serveur");
            stream = client.GetStream();

            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();

            RunGameLoop();
        }

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

        private static void RunGameLoop()
        {
            /*
            Raylib.InitWindow(800, 600, "Raylib Multijoueur");
            Raylib.SetTargetFPS(60);

            float playerX = 0;
            float playerY = 0;
            float playerZ = 0;

            float playerXrot = 0;
            float playerYrot = 2;
            float playerZrot = 0;

            float speed = 5.0f;

            while (!Raylib.WindowShouldClose())
            {
                // Gestion des mouvements
                if (Raylib.IsKeyDown(KeyboardKey.W)) playerY -= speed;
                if (Raylib.IsKeyDown(KeyboardKey.S)) playerY += speed;
                if (Raylib.IsKeyDown(KeyboardKey.A)) playerX -= speed;
                if (Raylib.IsKeyDown(KeyboardKey.D)) playerX += speed;

                // Envoi de la position au serveur
                string position = playerX + "," + playerY + "," + playerZ + "," + playerXrot + "," + playerYrot + "," + playerZrot;
                byte[] data = Encoding.UTF8.GetBytes(position);
                stream.Write(data, 0, data.Length);

                // Affichage du jeu
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);

                Raylib.DrawText("Client connecté", 10, 10, 20, Color.White);

                // Dessiner les autres joueurs
                try
                {
                    foreach (KeyValuePair<int, (float x, float y, float z, float xRot, float yRot, float zRot)> player in otherPlayers)
                    {
                        Raylib.DrawCircle((int)player.Value.x, (int)player.Value.y, 10, Color.Blue);
                    }
                }
                catch (Exception e) { Console.WriteLine("Erreur : " + e); }

                // Dessiner le joueur local
                Raylib.DrawCircle((int)playerX, (int)playerY, 10, Color.Red);

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
            */

            InitWindow(GetScreenWidth(), GetScreenHeight(), "Dead Ops Arcade");
            ToggleFullscreen();
            SetTargetFPS(60);

            Player.DefaultModel = LoadModel("ressources/model3d/alien.obj");

            Camera3D camera = new Camera3D();
            camera.Position = new Vector3(0.0f, 2.0f, 4.0f);
            camera.Target = new Vector3(0.0f, 2.0f, 0.0f);
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.FovY = 60.0f;
            camera.Projection = CameraProjection.Perspective;

            List<Player> Players = new List<Player>();
            List<Bullets> Bullets = new List<Bullets>();

            Weapon weapon = new Weapon();


            //Initialisation du model 3d


            DisableCursor();

            float sensibilité = 0.05f;

            while (!WindowShouldClose())
            {
                string position = camera.Position.X + "," + camera.Position.Y + "," + camera.Position.Z + "," + camera.Target.X + "," + camera.Target.Y + "," + camera.Target.Z;
                byte[] data = Encoding.UTF8.GetBytes(position);
                stream.Write(data, 0, data.Length);


                if (IsKeyPressed(KeyboardKey.F11))
                {
                    ToggleFullscreen();
                }

                // Mise a jour de la caméra (Déplacement)
                UpdateCameraPro(ref camera,
                    new Vector3(
                        IsKeyDown(KeyboardKey.W) * 0.1f - IsKeyDown(KeyboardKey.S) * 0.1f,
                        IsKeyDown(KeyboardKey.D) * 0.1f - IsKeyDown(KeyboardKey.A) * 0.1f,
                        0.0f),
                    new Vector3(
                        GetMouseDelta().X * sensibilité,
                        GetMouseDelta().Y * sensibilité,
                        0.0f),
                    0f);
                if (IsMouseButtonPressed(MouseButton.Left))
                {
                    Bullets.Add(new Bullets(camera.Position, camera.Target, GetFrameTime(), weapon));
                }
                BeginDrawing();
                BeginMode3D(camera);

                //Map
                ClearBackground(Color.White);
                DrawPlane(new Vector3(0.0f, 0.0f, 0.0f), new Vector2(32.0f, 32.0f), Color.LightGray);

                //Munitions
                for (int b = 0; b < Bullets.Count; b++)
                {
                    DrawCubeV(Bullets[b].Position, Bullets[b].Size, Color.Black);
                    if (Bullets[b].update())
                        Bullets.RemoveAt(b);
                }

                //Joueurs
                for (int p = 0; p < playerList.Count; p++)
                {
                    //Players[p].Update(camera.Position);
                    //DrawCubeV(Players[p].Position, Players[p].Size, Color.Green);
                    //Model
                    DrawModel(Player.DefaultModel, new Vector3(playerList[p].Position.X, playerList[p].Position.Y - 2, playerList[p].Position.Z), 0.1f, Color.DarkGray);
                    for (int b = 0; b < Bullets.Count; b++)
                    {
                        if (CheckCollisionBoxes(Bullets[b].BoundingBox, playerList[p].BoundingBox))
                        {
                            playerList[p].Life -= Bullets[b].Weapon.damage;
                            Bullets.RemoveAt(b);
                            b--;
                            if (playerList[p].Life <= 0)
                            {
                                ////tuer le player
                            }
                        }
                    }
                }

                EndMode3D();
                DrawRectangle(GetScreenWidth() / 2, GetScreenHeight() / 2, 10, 10, Color.Black);

                EndDrawing();
            }

            CloseWindow();
        }
    }
}
