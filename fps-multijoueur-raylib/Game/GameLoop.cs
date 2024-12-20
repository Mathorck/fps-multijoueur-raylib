using System.Numerics;
using DeadOpsArcade3D.GameElement;
using DeadOpsArcade3D.Multiplayer;
using static Raylib_cs.Raylib;
using Raylib_cs;

namespace DeadOpsArcade3D.Game;

public static class GameLoop
{
    private static Camera3D camera;
    
    private static Weapon weapon = new Weapon();
    
    /// <summary>Variable qui dit si la fenêtre devrait se fermer </summary>
    private static bool ferme = false;
    
    // sera sûrement mise dans les param
    public static float sensibilité = 0.05f;
    
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
        weapon.Fire(Bullet.BulletsList, camera);
        
        BeginDrawing();
        BeginMode3D(camera);

        Map.Render();
        
        Bullet.Draw(Bullet.BulletsList);

        Player.DrawAll(Player.PlayerList);
        
        EndMode3D();
        Gui.Render();
        EndDrawing();
    }
    
    
}