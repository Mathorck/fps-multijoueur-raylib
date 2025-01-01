using System.Numerics;
using DeadOpsArcade3D.Game;
using DeadOpsArcade3D.Multiplayer;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.GameElement
{
    public class Player
    {
        const float JUMP_HEIGHT = 5f;
        const float JUMP_SPEED = 0.2f;
        const float PLAYER_SPEED = 0.1f;

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


        public Vector3 Position;
        public Vector3 Rotation;

        public Vector3 Size;
        public float Speed;
        public float Life;
        public BoundingBox BoundingBox;

        public Player(float x, float y, float z, float xRot, float yRot, float zRot)
        {
            Position = new Vector3(x, y, z);
            Size = GetModelBoundingBox(DefaultModel).Max * 0.3f;
            Speed = 1f;
            Life = 100f;
            BoundingBox = new BoundingBox(Position, Size);
            Rotation = new Vector3(xRot, yRot, zRot);
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
                float.Atan2(Rotation.X - Position.X, Rotation.Z - Position.Z) * (180 / float.Pi), // Rotation angle (in degrees)
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
        public static void Movement(ref Camera3D camera)
        {
            canJump = false;
            canFall = true;
            
            RayW = new Ray(camera.Position - new Vector3(0,0.5f,0), new Vector3(0, 0, -1));
            RayS = new Ray(camera.Position - new Vector3(0,0.5f,0), new Vector3(0, 0, 1));
            RayD = new Ray(camera.Position - new Vector3(0,0.5f,0), new Vector3(1, 0, 0));
            RayA = new Ray(camera.Position - new Vector3(0,0.5f,0), new Vector3(-1, 0, 0));
            RayUp = new Ray(camera.Position - new Vector3(0,0.5f,0), new Vector3(0, 1, 0));
            RayDown = new Ray(camera.Position - new Vector3(0,0.5f,0), new Vector3(0, -1, 0));
            
            Vector3 deplacement = new Vector3();
            
            deplacement.X = IsKeyDown(KeyboardKey.W) * PLAYER_SPEED - IsKeyDown(KeyboardKey.S) * PLAYER_SPEED;
            deplacement.Y = IsKeyDown(KeyboardKey.D) * PLAYER_SPEED - IsKeyDown(KeyboardKey.A) * PLAYER_SPEED;

            foreach (BoundingBox obstacle in Map.Obstacles)
            {
                Gui.DebugContent.Add("Obstacle : " + obstacle.Max + " " + obstacle.Min);
                
                RayCollision collisionW = GetRayCollisionBox(RayW, obstacle);
                if (collisionW.Hit && float.Round(collisionW.Distance,1) <= 0.9f)
                {
                    camera.Position.Z += PLAYER_SPEED;
                    deplacement.X = 0;
                    deplacement.Y = 0;
                }
                
                RayCollision collisionS = GetRayCollisionBox(RayS, obstacle);
                if (collisionS.Hit && float.Round(collisionS.Distance, 1) <= 0.9f )
                {
                    camera.Position.Z -= PLAYER_SPEED;
                    deplacement.X = 0;
                    deplacement.Y = 0;
                }
                
                
                RayCollision collisionD = GetRayCollisionBox(RayD, obstacle);
                if (collisionD.Hit && float.Round(collisionD.Distance, 1) <= 0.9f )
                {
                    camera.Position.X -= PLAYER_SPEED;
                    deplacement.X = 0;
                    deplacement.Y = 0;
                }
                
                
                RayCollision collisionA = GetRayCollisionBox(RayA, obstacle);
                if (collisionA.Hit && float.Round(collisionA.Distance, 1) <= 0.9f )
                {
                    camera.Position.X += PLAYER_SPEED;
                    deplacement.X = 0;
                    deplacement.Y = 0;
                }
                    
                RayCollision collisionDown = GetRayCollisionBox(RayDown, obstacle);
                if (collisionDown.Hit && float.Round(collisionDown.Distance, 1) <= 0.9f)
                {
                    canJump = true;
                    canFall = false;
                }
            }
            
            if (canJump && !isJumping && IsKeyPressed(KeyboardKey.Space))
                isJumping = true;
            
            if (isJumping)
            {
                camera.Position.Y += JUMP_SPEED;
                jump += JUMP_SPEED;
                if (jump >= JUMP_HEIGHT)
                {
                    isJumping = false;
                    jump = 0;
                }
            }
            
            if (canFall && !isJumping)
                camera.Position.Y -= PLAYER_SPEED;
            
            // Mise a jour de la caméra (Déplacement)
            UpdateCameraPro(ref camera,
                deplacement,
                new Vector3(
                    GetMouseDelta().X * GameLoop.sensibilité,
                    GetMouseDelta().Y * GameLoop.sensibilité,
                    0.0f),
                0f
            );
        }
    }
}
