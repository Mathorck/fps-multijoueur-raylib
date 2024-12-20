using Raylib_cs;
using System.Numerics;

namespace DeadOpsArcade3D.GameElement
{
    public class Bullet
    {
        public static List<Bullet> BulletsList = new List<Bullet>();

        public Vector3 Position;
        public Vector3 Size;
        public Vector3 Direction;
        public float speed;
        public Weapon Weapon;
        public BoundingBox BoundingBox;

        public Bullet(Vector3 playerPos, Vector3 Direction, Weapon w) 
        { 
            Position = new Vector3(playerPos.X, playerPos.Y - 0.1f, playerPos.Z);
            Size = new Vector3(0.1f, 0.1f, 0.1f);
            this.Direction = Vector3.Normalize(Direction - Position);
            speed = 50.0f;
            Weapon = w;
            BoundingBox = new BoundingBox(Position, Size);
        }

        public bool update()
        {
            Position += Direction * speed * Raylib.GetFrameTime();
            BoundingBox.Min = Position;
            if(Position.Y <= 0) 
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
                Raylib.DrawCubeV(bullets[b].Position, bullets[b].Size, Color.Black);
                if (bullets[b].update())
                    bullets.RemoveAt(b);
            }
        }
    }
}
