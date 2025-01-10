using Raylib_cs;
using System;
using System.ComponentModel;
using System.Numerics;
using DeadOpsArcade3D.Game.GameElement;
using DeadOpsArcade3D.Game;

namespace DeadOpsArcade3D.Game.GameElement
{
    public class Bullet
    {

        public static List<Bullet> BulletsList = new List<Bullet>();
        private static Model bulletModel;

        public Vector3 Position;
        public Vector3 PlayerPosition;
        public Vector3 Target;
        public Vector3 Size;
        public Vector3 Direction;
        public float speed;
        public Weapon Weapon;
        public BoundingBox BoundingBox;
        public float Rotation;

        public Bullet(Vector3 playerPos, Vector3 Direction, Weapon w)
        {
            Position = new Vector3(playerPos.X, playerPos.Y - 0.05f, playerPos.Z);
            Target = new(Direction.X, Direction.Y - 0.05f, Direction.Z);
            Size = new Vector3(0.1f, 0.1f, 0.1f);
            this.Direction = Vector3.Normalize(Target - Position);
            speed = 75.0f;
            Weapon = w;
            BoundingBox = new BoundingBox(Position, Size);

            PlayerPosition = Position;

            Rotation = float.Atan2(Direction.X - PlayerPosition.X, Direction.Z - PlayerPosition.Z) * (180 / float.Pi);

        }

        public bool Update()
        {
            Position += Direction * speed * Raylib.GetFrameTime();
            BoundingBox.Min = Position;
            if (Position.Y <= 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Méthode qui affiche toutes les balles de la liste
        /// </summary>
        /// <param name="bullets">Liste des balles</param>
        public static void Draw(List<Bullet> bullets)
        {

            for (int b = 0; b < bullets.Count; b++)
            {
                Raylib.DrawModelEx(
                    bulletModel,                    // Le modèle de la balle
                    bullets[b].Position,            // Position de la balle
                    new System.Numerics.Vector3(0, 1, 0), // Axe de rotation (axe Y)
                    float.Atan2(bullets[b].Target.X - bullets[b].PlayerPosition.X, bullets[b].Target.Z - bullets[b].PlayerPosition.Z) * (180 / float.Pi),                          // L'angle en radians
                    new System.Numerics.Vector3(0.025f, 0.025f, 0.025f), // Échelle du modèle
                    Color.White                     // Couleur du modèle
                );

                if (bullets[b].Update())
                    bullets.RemoveAt(b);
            }
        }

        /// <summary>
        /// Pas utilisé
        /// </summary>
        private static unsafe void CollisionVerif()
        {
            List<BoundingBox> walls = new List<BoundingBox>();
            for (int i = 0; i < walls.Count; i++)
            {
                walls.Add(Raylib.GetMeshBoundingBox(Map.model.Meshes[i]));
                Gui.DebugContent.Add(walls[i].ToString());
            }

            for (int i = 0; i < BulletsList.Count; i++)
            {

            }
        }

        public static void Init()
        {
            bulletModel = Raylib.LoadModel("./ressources/model3d/ammo/bullet.glb");
        }
    }
}