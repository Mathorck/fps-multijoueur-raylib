using static Raylib_cs.Raylib;
using System.Numerics;
using DeadOpsArcade3D.Game;
using Raylib_cs;

namespace DeadOpsArcade3D.GameElement
{
    public class Player
    {
        const float JUMP_HEIGHT = 5f;
        const float JUMP_SPEED = 15f;
        const float PLAYER_SPEED = 0.1f;

        public static Model DefaultModel;
        public static bool isJumping = false;
        public static bool isGoingUp = false;
        public static float initpos = 0;


        public Vector3 Position;
        public Vector3 Rotation;

        public double AngleHorizontale;

        public Vector3 Size;
        public float Speed;
        public float Life;
        public BoundingBox BoundingBox;

        public Player(float x,float y, float z, float xRot, float yRot, float zRot)
        {
            Position = new Vector3(x, y, z);
            Size = GetModelBoundingBox(DefaultModel).Max * 0.3f;
            Speed = 1f;
            Life = 100f;
            BoundingBox = new BoundingBox(Position, Size);
            Rotation = new Vector3(xRot, yRot, zRot);

            //AngleHorizontale = ;
        }

        /// <summary>
        /// Permet l'affichage de tous les Joueurs
        /// </summary>
        /// <param name="playerList">Liste des Joueurs</param>
        public void Draw(List<Player> playerList)
        {
            // TODO: Gèrer l'orientation
            DrawModel(
                DefaultModel, 
                Position with { Y = Position.Y - 2 }, 
                0.1f, 
                Color.DarkGray
            );
            
            /* C'est pas au bon endroit En plus il me fait crash
            for (int b = 0; b < BulletsList.Count; b++)
            {
                if (CheckCollisionBoxes(BulletsList[b].BoundingBox, playerList[p].BoundingBox))
                {
                    playerList[p].Life -= BulletsList[b].Weapon.damage;
                    BulletsList.RemoveAt(b);
                    b--;
                    if (playerList[p].Life <= 0)
                    {
                        ////tuer le player
                    }
                }
            }
            */
            
        }

        /// <summary>
        /// Gère le mouvement du joueur principal
        /// </summary>
        /// <param name="camera"></param>
        public static void Movement(ref Camera3D camera)
        {
            // TODO: Saut
            if (IsKeyPressed(KeyboardKey.Space) && !isJumping)
            {
                isJumping = true;
                isGoingUp = true;
                initpos = camera.Position.Y;
            }

            // Sauts
            if (isJumping)
            {
                //Console.WriteLine($"if({camera.Position.Y} < {2 + JUMP_HEIGHT})");
                if (camera.Position.Y < initpos + JUMP_HEIGHT && isGoingUp)
                {
                    //camera.Position.Y += JUMP_SPEED * GetFrameTime() / (camera.Position.Y/(initpos + JUMP_HEIGHT / 5));
                    //camera.Target.Y += JUMP_SPEED * GetFrameTime() / (camera.Position.Y / (initpos + JUMP_HEIGHT / 5));
                    camera.Position.Y += camera.Position.Y / ((camera.Position.Y + JUMP_HEIGHT) / 2) * JUMP_SPEED * GetFrameTime();
                    camera.Target.Y += camera.Position.Y / ((camera.Position.Y + JUMP_HEIGHT) / 2) * JUMP_SPEED * GetFrameTime();
                }
                else if (camera.Position.Y > initpos)
                {
                    if (camera.Position.Y - camera.Position.Y / ((camera.Position.Y + JUMP_HEIGHT) / 2) * JUMP_SPEED * GetFrameTime() > initpos)
                    {
                        //camera.Position.Y -= JUMP_SPEED * GetFrameTime() / (camera.Position.Y / (initpos + JUMP_HEIGHT / 5));
                        //camera.Target.Y -= JUMP_SPEED * GetFrameTime() / (camera.Position.Y / (initpos + JUMP_HEIGHT / 5));
                        camera.Position.Y -= camera.Position.Y / ((camera.Position.Y + JUMP_HEIGHT) / 2) * JUMP_SPEED * GetFrameTime();
                        camera.Target.Y -= camera.Position.Y / ((camera.Position.Y + JUMP_HEIGHT) / 2) * JUMP_SPEED * GetFrameTime();
                    }
                    else
                    {
                        camera.Target.Y -= camera.Position.Y - initpos;
                        camera.Position.Y = initpos;
                    }
                    isGoingUp = false;
                }
                else
                {
                    isJumping = false;
                    isGoingUp = false;
                }

            }


            // Mise a jour de la caméra (Déplacement)
            UpdateCameraPro(ref camera,
                new Vector3(
                    IsKeyDown(KeyboardKey.W) * PLAYER_SPEED - IsKeyDown(KeyboardKey.S) * PLAYER_SPEED,
                    IsKeyDown(KeyboardKey.D) * PLAYER_SPEED - IsKeyDown(KeyboardKey.A) * PLAYER_SPEED,
                    0.0f),
                new Vector3(
                    GetMouseDelta().X * GameLoop.Sensibilite,
                    GetMouseDelta().Y * GameLoop.Sensibilite,
                    0.0f),
                0f);


        }
    }
}
