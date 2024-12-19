using static Raylib_cs.Raylib;
using System.Numerics;
using DeadOpsArcade3D.Game;
using Raylib_cs;

namespace DeadOpsArcade3D.GameElement
{
    public class Player
    {
        public static Model DefaultModel;
        public Vector3 Position;
        public Vector3 Rotation;

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
            UpdateCameraPro(ref camera,
                new Vector3(
                    IsKeyDown(KeyboardKey.W) * 0.1f - IsKeyDown(KeyboardKey.S) * 0.1f,
                    IsKeyDown(KeyboardKey.D) * 0.1f - IsKeyDown(KeyboardKey.A) * 0.1f,
                    0.0f),
                new Vector3(
                    GetMouseDelta().X * GameLoop.sensibilité,
                    GetMouseDelta().Y * GameLoop.sensibilité,
                    0.0f),
                0f);
        }
    }
}
