using System.Numerics;
using DeadOpsArcade3D.GameElement;
using DeadOpsArcade3D.Multiplayer;
using static Raylib_cs.Raylib;
using Raylib_cs;

namespace DeadOpsArcade3D.Game;

public static class GameLoop
{
    private static Camera3D camera;
    
    /// <summary>Liste des qui contient tous les joueurs </summary>
    private static List<Player> playerList = new List<Player>();
    private static List<Bullet> BulletsList = new List<Bullet>();
    private static Weapon weapon = new Weapon();
    
    /// <summary>Variable qui dit si la fenêtre devrait se fermer </summary>
    private static bool ferme = false;
    
    // sera sûrement mise dans les param
    public static float Sensibilite = 0.05f;
    
    /// <summary>
    /// Démarre le jeu
    /// </summary>
    public static void StartGame()
    {
        InitWindow(GetScreenWidth(), GetScreenHeight(), "Dead Ops Arcade");
        ToggleFullscreen();
        SetTargetFPS(60);
        
        SetVariables();
        
        DisableCursor();

        while (!ferme)
        {
            Update();
            
            //// Contrôles ////
            // Fullscreen
            if (IsKeyPressed(KeyboardKey.F11))
                ToggleFullscreen();
            
            // Empêche de fermer le jeu avec ESC
            if (WindowShouldClose() /*&& !IsKeyDown(KeyboardKey.Escape) C'est un objet magique qui nous servira plus tard */)
                ferme = true;
            ///////////////////

            //Console.WriteLine("Target : " + camera.Target);
            //Console.WriteLine("Position : " + camera.Position);

            //Console.WriteLine("Soustraction : " + (camera.Position - camera.Target));

            //Console.WriteLine("Camera UP : " + camera.Up);

            //Console.WriteLine(Math.Atan2(camera.Target.X - camera.Position.X, camera.Target.Y - camera.Target.Y));
            //Console.WriteLine(Math.Atan2(camera.Target.X - camera.Position.X, camera.Target.Z - camera.Position.Z) * (180 / Math.PI) + 180);

        }
        CloseWindow();
    }

    /// <summary>
    /// Méthode qui permet d'initialiser toutes les variables après l'affichage de la fenêtre
    /// pour éviter l'erreur de toi même, tu sais
    /// </summary>
    private static void SetVariables()
    {
        // Modèle utilisé par les joueurs
        Player.DefaultModel = LoadModel("ressources/model3d/tinker.obj");

        // Caméra du joueur
        camera = new Camera3D
        {
            Position = new Vector3(0.0f, 2.0f, 4.0f),
            Target = new Vector3(0.0f, 2.0f, 0.0f),
            Up = new Vector3(0.0f, 1.0f, 0.0f),
            FovY = 60.0f,
            Projection = CameraProjection.Perspective
        };
    }

    /// <summary>
    /// Méthode qui s'éxécute chaque frame
    /// </summary>
    private static void Update()
    {
        Client.SendInfo(camera);
        Player.Movement(ref camera);
        weapon.Fire(BulletsList, camera);
        
        BeginDrawing();
        BeginMode3D(camera);

        // Map
        Map.Render();
        
        // Bullets
        Bullet.Draw(BulletsList);

        //Joueurs
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].Draw(playerList);
        }
        
        EndMode3D();
        Gui.Render();
        EndDrawing();
    }
    
    
}