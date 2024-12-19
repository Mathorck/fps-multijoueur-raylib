using static Raylib_cs.Raylib;
using System.Numerics;
using Raylib_cs;
using Classes;

namespace DeadOpsArcade3D
{
    class Program
    {
        static void Main(string[] args)
        {
            InitWindow(1920, 1080, "Dead Ops Arcade");
            ToggleFullscreen();
            SetTargetFPS(60);

            Camera3D camera = new Camera3D();
            camera.Position = new Vector3(0.0f, 2.0f, 4.0f);
            camera.Target = new Vector3(0.0f, 2.0f, 0.0f);
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.FovY = 60.0f;
            camera.Projection = CameraProjection.Perspective;

            List<Player> Players = new List<Player>();
            List<Bullets> Bullets = new List<Bullets>();

            Players.Add(new Player());

            Weapon weapon = new Weapon();


            //Initialisation du model 3d
            Model alien = LoadModel("ressources/model3d/alien/alien.obj");


            DisableCursor();

            while (!WindowShouldClose())
            {
                // Mise a jour de la caméra (Déplacement)
                UpdateCameraPro(ref camera,
                    new Vector3(
                        IsKeyDown(KeyboardKey.W) * 0.1f - IsKeyDown(KeyboardKey.S) * 0.1f,
                        IsKeyDown(KeyboardKey.D) * 0.1f - IsKeyDown(KeyboardKey.A) * 0.1f,
                        0.0f),
                    new Vector3(
                        GetMouseDelta().X * 0.05f,
                        GetMouseDelta().Y * 0.05f,
                        0.0f),
                    0f);

                if(IsMouseButtonPressed(MouseButton.Left))
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
                for (int p = 0; p < Players.Count; p++)
                {
                    Players[p].Update(camera.Position);
                    DrawCubeV(Players[p].Position, Players[p].Size, Color.Green);
                    //Model
                    Raylib.DrawModel(alien, Players[p].Position, 0.2f, Color.DarkGray);
                    for (int b = 0; b < Bullets.Count; b++) 
                    {
                        if (CheckCollisionBoxes(new BoundingBox(Bullets[b].Position, Bullets[b].Size), new BoundingBox(Players[p].Position, Players[p].Size)))
                        {
                            Players[p].life -= Bullets[b].Weapon.damage;
                            Bullets.RemoveAt(b);
                            if (Players[p].life <= 0)
                            {
                                ////tuer le player
                                Console.WriteLine("Il est mort");
                            }
                        }
                    }
                }

                EndMode3D();
                DrawRectangle(-5, -5, 10, 10, Color.Black);

                EndDrawing();
            }

            CloseWindow();
        }

        public void shoot()
        {

        }
    }

}
