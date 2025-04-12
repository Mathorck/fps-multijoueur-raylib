using System.Numerics;
using DeadOpsArcade3D.Game.GameElement;
using DeadOpsArcade3D.Multiplayer;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Game;

public static class GameLoop
{
    public static Camera3D camera;

    private static readonly Weapon weapon = new();

    /// <summary>Variable qui dit si la fenêtre devrait se fermer </summary>
    public static bool ferme;

    // sera sûrement mise dans les param
    public static float sensibilite = 0.05f;
    public const float MIN_SENSI = 0.1f, MAX_SENSI = 10;

    // Pour les animations
    private static int animIndex = 0, animCurrentFrame = 0;
    public static Random random;


    /// <summary>Liste des qui contient tous les spawn possible </summary>
    public static List<Vector3> ListSpawn = new List<Vector3> { new Vector3(17, 0.4f, -7), new Vector3(-8, 0.4f, -7), new Vector3(-15, 0.4f, -7), new Vector3(-15, 0.4f, 15), new Vector3(-15, 0.4f, 25), new Vector3(0, 0.4f, 24), new Vector3(2, 0.4f, 25), new Vector3(17, 0.4f, 25) };

    /// <summary>
    ///     Démarre le jeu
    /// </summary>
    public static void StartGame()
    {
        InitWindow(GetScreenWidth(), GetScreenHeight(), "Dead Ops 3D");
        ToggleFullscreen();
        SetTargetFPS(60);
        SetConfigFlags(ConfigFlags.VSyncHint);
        InitAudioDevice();

        SetVariables();

        DisableCursor();
        
        //StreamReader rd1 = new StreamReader("sensi.txt");
        //float rewriteSens = float.Parse(rd1.ReadLine());
        //if (rewriteSens != null && rewriteSens * 10 < MAX_SENSI && rewriteSens * 10 > MIN_SENSI)
        //    sensibilite = rewriteSens;
        //rd1.Close();

        while (!ferme)
        {
            Update();

            //// Contrôles ////
            // Fullscreen
            if (IsKeyPressed(KeyboardKey.F11))
                ToggleFullscreen();

            // Tab
            Gui.IsTabOppened = IsKeyDown(KeyboardKey.Tab);

            // Esc
            if (IsKeyPressed(KeyboardKey.Escape))
                Gui.IsParamOppended = !Gui.IsParamOppended;


            // Empêche de fermer le jeu avec ESC
            if (WindowShouldClose() && !IsKeyDown(KeyboardKey.Escape) /*C'est un objet magique qui nous servira plus tard */)
            {
                UnloadModel(Player.DefaultModel);
                ferme = true;
            }
            ///////////////////s
        }

        UnloadAll();

        CloseAudioDevice();
        CloseWindow();
        StreamWriter wr1 = new StreamWriter("sensi.txt");
        wr1.WriteLine(sensibilite);
        wr1.Close();
        Environment.Exit(0);
    }

    /// <summary>
    ///     Méthode qui permet d'initialiser toutes les variables après l'affichage de la fenêtre
    ///     pour éviter l'erreur de toi même, tu sais
    /// </summary>
    private static void SetVariables()
    {


        // Modèle utilisé par les joueurs
        Player.DefaultModel = LoadModel("ressources/model3d/character/robot.glb");

        //Spawn aléatoire
        random = new Random();
        int spawnRandom = random.Next(0, 7);

        // Caméra du joueur
        camera = new Camera3D
        {
            Position = ListSpawn[spawnRandom],
            Target = new Vector3(0.0f, 1.0f, 0.0f),
            Up = new Vector3(0.0f, 1.0f, 0.0f),
            FovY = 60.0f,
            Projection = CameraProjection.Perspective
        };

        // Map
        Map.Init();
        Gui.Init();
        Bullet.Init();
    }

    /// <summary>
    ///     Méthode qui s'éxécute chaque frame
    /// </summary>
    private static void Update()
    {
        Client.SendInfo(camera, animIndex, animCurrentFrame);

        if (!Gui.IsParamOppended)
        {
            Player.Movement(ref camera);
            weapon.Fire(Bullet.BulletsList, camera);
            DisableCursor();
        }
        else
            ShowCursor();

        Player.Animation(ref animIndex, ref animCurrentFrame);


        //Player.VerifPacket();s

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        BeginMode3D(camera);

        Map.Render();

        Bullet.Draw(Bullet.BulletsList);

        Player.DrawAll(Player.PlayerList);

        EndMode3D();
        Gui.Render();
        EndDrawing();
    }

    private static void UnloadAll()
    {
        Map.Unload(); 
        Gui.Unload();
        Bullet.Unload();
        
        UnloadModel(Player.DefaultModel);
    }
}