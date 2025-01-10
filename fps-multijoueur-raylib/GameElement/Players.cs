using System.Numerics;
using DeadOpsArcade3D.Game;
using DeadOpsArcade3D.Multiplayer;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.GameElement
{
    public class Player
    {
        public const float JUMP_HEIGHT = 4f;
        public const float JUMP_SPEED = 0.3f;
        public const float PLAYER_SPEED = 0.05f;
        public const float SPRINT_SPEED = 0.09f;

        public static Ray RayA;
        public static Ray RayD;
        public static Ray RayW;
        public static Ray RayS;
        public static Ray RayDown;
        public static Ray RayUp;

        /// <summary>Liste des qui contient tous les joueurs </summary>
        public static List<Player> PlayerList = new List<Player>();

        public static Model DefaultModel;
        private static bool canJump = true;
        private static bool canFall = true;
        private static bool isJumping = false;
        private static float jump = 0;
        public static float rotation = 0;


        public Vector3 Position;
        public Vector3 Rotation;

        public Vector3 Size;
        public float Speed;
        public float Life;
        public BoundingBox HitBox;

        public Player(float x, float y, float z, float xRot, float yRot, float zRot)
        {
            Position = new Vector3(x, y, z);
            Size = GetModelBoundingBox(DefaultModel).Max * 0.3f;
            Speed = 1f;
            Life = 100f;
            HitBox = new BoundingBox(Position, Size);
            Rotation = new Vector3(xRot, yRot, zRot);

            rotation = float.Atan2(Rotation.X - Position.X, Rotation.Z - Position.Z) * (180 / float.Pi);
        }

        /// <summary>
        /// Cela dessine tout les joueurs
        /// </summary>
        /// <param name="playerList"></param>
        public static void DrawAll(List<Player> playerList)
        {
            if (playerList == null || playerList.Count == 0)
            {
                Client.ConsoleError("La liste des joueurs est vide ou null.");
                return;
            }
            try
            {
                for (int i = 0; i < playerList.Count; i++)
                {
                    playerList[i].Draw();
                }
            }
            catch (Exception e)
            {
                Client.ConsoleError($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Permet l'affichage de tous les Joueurs
        /// </summary>
        /// <param name="playerList">Liste des Joueurs</param>
        public void Draw()
        {
            DrawModelEx(
                DefaultModel,                   // Model to draw
                Position - new Vector3(0, 2, 0),// Position in 3D space
                new Vector3(0, 1, 0),           // Rotation axis (Y-axis for character rotation)
                rotation, // Rotation angle (in degrees)
                new Vector3(0.1f, 0.1f, 0.1f),  // Scale (matching the 0.3f from your bounding box)
                Color.Blue                      // Tint color
            );
        }
        
        /// <summary>
        /// Dessine les rayons pour visualisation
        /// </summary>
        /// <param name="camera">Caméra principale</param>
        public static void DrawRays()
        {
            DrawLine3D(RayW.Position, RayW.Position + RayW.Direction * 10, Color.Red); // Rayon W
            DrawLine3D(RayS.Position, RayS.Position + RayS.Direction * 10, Color.Green); // Rayon S
            DrawLine3D(RayD.Position, RayD.Position + RayD.Direction * 10, Color.Blue); // Rayon D
            DrawLine3D(RayA.Position, RayA.Position + RayA.Direction * 10, Color.Yellow); // Rayon A
            DrawLine3D(RayUp.Position, RayUp.Position + RayUp.Direction * 10, Color.Purple); // Rayon UP
            DrawLine3D(RayDown.Position, RayDown.Position + RayDown.Direction * 10, Color.Orange); // Rayon DOWN
        }

        /// <summary>
        /// Gère le mouvement du joueur principal
        /// </summary>
        /// <param name="camera"></param>
        public static unsafe void Movement(ref Camera3D camera)
        {
            /*
            
            canJump = false;
            canFall = true;
            
            RayW = new Ray(camera.Position - new Vector3(0,1f,0), new Vector3(0, 0, -1));
            RayS = new Ray(camera.Position - new Vector3(0,1f,0), new Vector3(0, 0, 1));
            RayD = new Ray(camera.Position - new Vector3(0,1f,0), new Vector3(1, 0, 0));
            RayA = new Ray(camera.Position - new Vector3(0,1f,0), new Vector3(-1, 0, 0));
            RayUp = new Ray(camera.Position - new Vector3(0,1f,0), new Vector3(0, 1, 0));
            RayDown = new Ray(camera.Position - new Vector3(0,1.5f,0), new Vector3(0, -1, 0));
            
            Vector3 deplacement = new Vector3();
            Vector3 initialTarget = camera.Target - camera.Position;
            
            float speed = IsKeyDown(KeyboardKey.LeftShift) ? SPRINT_SPEED : PLAYER_SPEED;
            
            deplacement.X = IsKeyDown(KeyboardKey.W) * speed - IsKeyDown(KeyboardKey.S) * PLAYER_SPEED;
            deplacement.Y = IsKeyDown(KeyboardKey.D) * PLAYER_SPEED - IsKeyDown(KeyboardKey.A) * PLAYER_SPEED;

            foreach (BoundingBox obstacle in Map.Obstacles)
            {
                NormalCollision(ref camera.Position.Z,speed, ref deplacement, RayW, obstacle);
                NormalCollision(ref camera.Position.Z,-speed, ref deplacement, RayS, obstacle);
                NormalCollision(ref camera.Position.X,-speed, ref deplacement, RayD, obstacle);
                NormalCollision(ref camera.Position.X,speed, ref deplacement, RayA, obstacle);
                    
                RayCollision collisionDown = GetRayCollisionBox(RayDown, obstacle);
                if (collisionDown.Hit && float.Round(collisionDown.Distance, 1) <= 0.2f)
                {
                    canJump = true;
                    canFall = false;
                }
                
                //Le plafond n'est pas encore fait
            }
            Jumping(ref camera);

            camera.Target = camera.Position + initialTarget;
            
            // Mise a jour de la caméra (Déplacement)
            UpdateCameraPro(ref camera,
                deplacement,
                new Vector3(
                    GetMouseDelta().X * GameLoop.sensibilité,
                    GetMouseDelta().Y * GameLoop.sensibilité,
                    0.0f),
                0f
            );
            */

            canJump = false;
            canFall = true;

            Gui.DebugContent.Add($"Position: [{float.Round(camera.Position.X, 2)} | {float.Round(camera.Position.Y, 2)} | {float.Round(camera.Position.Z, 2)}]");

            Vector3 oldCamPos = camera.Position;
            Vector3 initialTarget = camera.Target - camera.Position;
            Vector3 deplacement = new Vector3();

            float speed = IsKeyDown(KeyboardKey.LeftShift) ? SPRINT_SPEED : PLAYER_SPEED;

            deplacement.X = IsKeyDown(KeyboardKey.W) * speed - IsKeyDown(KeyboardKey.S) * PLAYER_SPEED;
            deplacement.Y = IsKeyDown(KeyboardKey.D) * PLAYER_SPEED - IsKeyDown(KeyboardKey.A) * PLAYER_SPEED;

            camera.Up = new Vector3(0, 1, 0);


            //Jumping(ref camera);

            // Mise a jour de la caméra (Déplacement)
            UpdateCameraPro(ref camera,
                deplacement,
                new Vector3(
                    GetMouseDelta().X * GameLoop.sensibilité,
                    GetMouseDelta().Y * GameLoop.sensibilité,
                    0.0f),
                0f
            );


            Vector2 playerPos = new(camera.Position.X, camera.Position.Z);

            int playerCellX = (int)(playerPos.X - Map.mapPosition.X + 0.5f);
            int playerCellY = (int)(playerPos.Y - Map.mapPosition.Z + 0.5f);

            if (playerCellX < 0)
            {
                playerCellX = 0;
            }
            else if (playerCellX >= Map.cubicmap.Width)
            {
                playerCellX = Map.cubicmap.Width - 1;
            }

            if (playerCellY < 0)
            {
                playerCellY = 0;
            }
            else if (playerCellY >= Map.cubicmap.Height)
            {
                playerCellY = Map.cubicmap.Height - 1;
            }

            for (int y = 0; y < Map.cubicmap.Height; y++)
            {
                for (int x = 0; x < Map.cubicmap.Width; x++)
                {
                    Color* mapPixelsData = Map.MapPixels;

                    // Collision: Color.white pixel, only check R channel
                    Rectangle rec = new(
                        Map.mapPosition.X - 0.5f + x * 1.0f,
                        Map.mapPosition.Z - 0.5f + y * 1.0f,
                        1.0f,
                        1.0f
                    );

                    bool colisionX = false, colisionZ = false;

                    //if (camera.Position.X - oldCamPos.X > 0)
                    //    colisionX = CheckCollisionCircleRec(new(camera.Position.X, camera.Position.Z), 0.1f, rec);
                    //else
                    //    colisionX = CheckCollisionCircleRec(new(camera.Position.X, camera.Position.Z), 0.1f, rec);
                    //if (camera.Position.Z - oldCamPos.Z > 0)
                    //    colisionZ = CheckCollisionCircleRec(new(camera.Position.Z, camera.Position.Z), 0.1f, rec);
                    //else
                    //    colisionZ = CheckCollisionCircleRec(new(camera.Position.Z, camera.Position.Z), 0.1f, rec);

                    //colisionX = CheckCollisionCircleRec(new(camera.Position.X, 0), 0.1f, rec);
                    //colisionZ = CheckCollisionCircleRec(new(0, camera.Position.Z), 0.1f, rec);


                    const float bSize = 0.2f, sSize = 0.1f;

                    Rectangle recx = new(camera.Position.X - bSize/2, camera.Position.Z - sSize/2, new(bSize, sSize));
                    Rectangle recz = new(camera.Position.X - sSize/2, camera.Position.Z - bSize/2, new(sSize, bSize));

                    DrawRectangleRec(recx, Color.Blue);
                    DrawRectangleRec(recz, Color.Black);

                    colisionX = CheckCollisionRecs(recx, rec);
                    colisionZ = CheckCollisionRecs(recz, rec);



                    if(mapPixelsData[y * Map.cubicmap.Width + x].R == 255)
                    {
                        if(colisionX)
                        {
                            camera.Position.X = oldCamPos.X;
                        }
                        if (colisionZ)
                        {
                            camera.Position.Z = oldCamPos.Z;
                        }
                    }


                    //bool collision = CheckCollisionCircleRec(new Vector2(camera.Position.X, camera.Position.Z), 0.1f, rec);
                    //if ((mapPixelsData[y * Map.cubicmap.Width + x].R == 255) && collision)
                    //{
                    //    // Collision detected, reset camera position
                    //    camera.Position = oldCamPos;
                    //    camera.Target = camera.Position + initialTarget;
                    //}
                }
            }


        }

        /// <summary>
        /// Méthode qui gère le saut et la chute du joueur
        /// </summary>
        /// <param name="camera"></param>
        private static void Jumping(ref Camera3D camera)
        {
            if (canJump && !isJumping && IsKeyDown(KeyboardKey.Space))
                isJumping = true;
            
            if (isJumping)
            {
                float jumpProgress = jump / JUMP_HEIGHT;
                float jumpVelocity = JUMP_SPEED * (1f - jumpProgress + 0.1f);
                camera.Position.Y += jumpVelocity;
                jump += jumpVelocity;
                if (jump >= JUMP_HEIGHT)
                {
                    isJumping = false;
                    jump = 0;
                }
            }
            else if (canFall && !isJumping)
            {
                float fallProgress = jump / JUMP_HEIGHT;
                float fallVelocity = JUMP_SPEED * (fallProgress + 0.1f);
                camera.Position.Y -= fallVelocity;
                jump += fallVelocity;
            }
        }
    }
}
