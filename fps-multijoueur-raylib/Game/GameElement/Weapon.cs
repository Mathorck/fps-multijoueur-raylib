using DeadOpsArcade3D.Multiplayer;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Game.GameElement;

public class Weapon
{
    private readonly Timer fireTimer = new(0.15f);
    private readonly Timer reloadTimer = new(1.5f);
    public float damage;

    public Weapon()
    {
        damage = 15f;
    }

    /// <summary>
    ///     S'exécute lorsque l'arme est utilisée pour tirer une balle
    /// </summary>
    public void Fire(List<Bullet> BulletsList, Camera3D camera)
    {
        fireTimer.Update();
        reloadTimer.Update();

        if (IsMouseButtonPressed(MouseButton.Left) && Player.Bullet > 0 && !reloadTimer.IsRunning &&
            !fireTimer.IsRunning)
        {
            BulletsList.Add(new Bullet(camera.Position, camera.Target, this, null));
            Client.Fire();
            Player.Bullet--;
            fireTimer.Reset();
            fireTimer.Start();
        }

        if (IsKeyPressed(KeyboardKey.R) && !reloadTimer.IsRunning && Player.Bullet < 30)
        {
            reloadTimer.Reset();
            reloadTimer.Start();
        }

        if (reloadTimer.IsFinished)
        {
            Player.Bullet = 30;
            reloadTimer.Stop();
            reloadTimer.Reset();
        }

        if (fireTimer.IsFinished)
        {
            fireTimer.Stop();
            fireTimer.Reset();
        }
    }
}