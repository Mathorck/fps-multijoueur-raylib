using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.GameElement
{
    public class Weapon
    {
        public float damage;
        public Weapon() 
        {
            damage = 15f;
        }

        /// <summary>
        /// S'exécute lorsque l'arme est utilisée pour tirer une balle
        /// </summary>
        public void Fire(List<Bullet> BulletsList, Camera3D camera)
        {
            if (IsMouseButtonPressed(MouseButton.Left))
            {
                BulletsList.Add(new Bullet(camera.Position, camera.Target, GetFrameTime(), this));
            }
        }
    }
}
