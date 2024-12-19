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
            InitWindow(GetScreenWidth(), GetScreenHeight(), "Dead Ops Arcade");
            ToggleFullscreen();
            SetTargetFPS(60);

            Camera3D camera = new Camera3D();
            camera.Position = new Vector3(0.0f, 2.0f, 4.0f);
            camera.Target = new Vector3(0.0f, 2.0f, 0.0f);
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.FovY = 60.0f;
            camera.Projection = CameraProjection.Perspective;


            Model alien = LoadModel("ressources/model3d/alien/alien.obj");


            List<Player> Players = new List<Player>();
            List<Bullets> Bullets = new List<Bullets>();

            Players.Add(new Player(alien));

            Weapon weapon = new Weapon();


            //Initialisation du model 3d


            DisableCursor();

            float sensibilité = 0.05f;

            while (!WindowShouldClose())
            {
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
                    //DrawCubeV(Players[p].Position, Players[p].Size, Color.Green);
                    //Model
                    DrawModel(alien, Players[p].Position, 0.2f, Color.DarkGray);
                    for (int b = 0; b < Bullets.Count; b++) 
                    {
                        if (CheckCollisionBoxes(Bullets[b].BoundingBox, Players[p].BoundingBox))
                        {
                            Players[p].life -= Bullets[b].Weapon.damage;
                            Bullets.RemoveAt(b);
                            if (Players[p].life <= 0)
                            {
                                ////tuer le player
                            }
                        }
                    }
                }

                EndMode3D();
                DrawRectangle(GetScreenWidth()/2, GetScreenHeight()/2, 10, 10, Color.Black);

                EndDrawing();
            }

            CloseWindow();
        }
    }

}
