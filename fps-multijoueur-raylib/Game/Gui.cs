using System.Numerics;
using DeadOpsArcade3D.Game.GameElement;
using static Raylib_cs.Raylib;
using DeadOpsArcade3D.Launcher.LauncherElement;

namespace DeadOpsArcade3D.Game;

public static class Gui
{
    public static List<string> DebugContent = new();
    public static List<string> ErrorContent = new();
    public static bool IsTabOppened = false;

    public static bool IsParamOppended = false;

    private static bool ChoseSensi = false;

    private static readonly Rectangle healthBar = new(20, GetScreenHeight() - 80, 180, 60);
    private static Texture2D wepon;

    // Variable globale pour contrôler le mouvement de l'arme
    private static float weaponMovement = 0.0f; // Position de l'arme
    private static float weaponTime = 0.0f; // Temps utilisé pour l'oscillation
    private static float weaponAmplitude = 100.0f; // Amplitude du mouvement

    #region Sys
    /// <summary>
    ///     Affiche le GUI
    /// </summary>
    public static void Render()
    {
        Weapon();
        Crossair();
        MiniMap();
        VieAndBullet();
        if (IsTabOppened)
            ShowTab();
        if (IsParamOppended)
            Parametres();
        Debug();
    }
    
    public static void Init()
    {
        wepon = LoadTexture("./ressources/textures/ShootGun.png");
    }
    public static void Unload()
    {
        UnloadTexture(wepon);
    }
    #endregion

    #region crossair
    /// <summary>
    ///     Affiche le crossair
    /// </summary>
    private static void Crossair()
    {
        Color color = new(0, 0, 0, 50);
        int crossWidth = 20;
        int crossHeight = 20;
        int crossWeight = 2;


        int width = GetScreenWidth();
        int height = GetScreenHeight();

        DrawRectangle(width / 2 - crossWidth / 2, height / 2 - crossWeight / 2, crossWidth, crossWeight, Color.Black);

        DrawRectangle(width / 2 - crossWeight / 2, height / 2 - crossHeight / 2, crossWeight, crossHeight, Color.Black);
    }
    #endregion
    
    #region debug
    private static void Debug()
    {
        string? output = "";
        foreach (string? text in DebugContent) output += text + "\n";
        DrawText(output, 20, 20, 20, Color.Black);
        DebugContent.Clear();

        DrawFPS(10, 10);

        /*
        string? output2 = "";
        foreach (string? text in ErrorContent) output2 += text + "\n";
        DrawText(output2, 100, 20, 20, Color.Red);
        */
    }

    #endregion

    private static void Weapon()
    {
        if (IsKeyDown(KeyboardKey.W) || IsKeyDown(KeyboardKey.S) || IsKeyDown(KeyboardKey.A) ||
            IsKeyDown(KeyboardKey.D))
        {
            float wpnSpeed = IsKeyDown(KeyboardKey.LeftShift) ? 4.0f : 2.0f;
            weaponTime += wpnSpeed * GetFrameTime();
            weaponMovement = (float)(Math.Sin(weaponTime) * weaponAmplitude);
        }
        
        DrawTexture(wepon, 
            (int)((GetScreenWidth() - wepon.Width) * 0.5f + 100) + (int)weaponMovement, 
            GetScreenHeight() - wepon.Height - (int)Math.Abs(weaponMovement * 0.5f) +100, 
            Color.White);
    }

    private static void MiniMap()
    {
        DrawTextureEx(Map.Cubicmap, new Vector2(GetScreenWidth() - Map.Cubicmap.Width * 4 - 20, 20), 0.0f, 4.0f,
            Color.White);
        DrawRectangleLines(GetScreenWidth() - Map.Cubicmap.Width * 4 - 20, 20, Map.Cubicmap.Width * 4,
            Map.Cubicmap.Height * 4, Color.Green);

        // Draw player position radar
        Vector2 playerPos = new(GameLoop.camera.Position.X, GameLoop.camera.Position.Z);

        int playerCellX = (int)(playerPos.X - Map.MapPosition.X + 0.5f);
        int playerCellY = (int)(playerPos.Y - Map.MapPosition.Z + 0.5f);

        DrawRectangle(GetScreenWidth() - Map.Cubicmap.Width * 4 - 20 + playerCellX * 4, 20 + playerCellY * 4, 4, 4,
            Color.Red);
    }

    private static void VieAndBullet()
    {
        int screenWidth = GetScreenWidth();
        int screenHeight = GetScreenHeight();

        DrawRectangleRounded(healthBar, 0.3f, 2, new Color(100, 100, 100, 100));
        DrawText("Vie", 40, screenHeight - 50, 20, Color.Yellow);
        DrawText($"{Player.Health}", 90, screenHeight - 70, 50, Color.Yellow);

        DrawRectangle(screenWidth - 220, screenHeight - 60, 200, 40, Color.Gray);
        DrawText("Balles: " + Player.Bullet, screenWidth - 215, screenHeight - 55, 20, Color.White);

        if (Player.Bullet == 0)
        {
            DrawText("Recharger [R]", screenWidth - 230, screenHeight - 100, 32, Color.Red);
        }
    }

    #region Tab

    private static void ShowTab()
    {
        float sHeight = GetScreenHeight();
        float sWidth = GetScreenWidth();

        int tabWidth = 400;
        int tabPaddingR = (int)(sWidth - tabWidth * 2);
        
        DrawRectangle(
            (int)(tabWidth),
            50,
            tabPaddingR,
            (int)(sHeight - 100),
            new Color(0, 0, 0, 100)
        );
        
        int hgt = 60;
        
        DrawPlayerElement(Player.Nom, ref hgt, tabWidth, tabPaddingR);
        hgt += 20;
        
        foreach (Player plr in Player.PlayerList)
        {
            DrawPlayerElement(plr.Pseudo, ref hgt, tabWidth, tabPaddingR);
            hgt += 20;
        }
    }

    private static void DrawPlayerElement(string pseudo, ref int hgt, int tabWidth, int tabPaddingR)
    {
        int elementHeight = 30; 
        DrawRectangle(
            tabWidth + 5,
            hgt,
            tabPaddingR - 10,
            elementHeight,
            new Color(0, 0, 0, 100)
        );

        DrawText(
            pseudo,
            tabWidth + 10,
            hgt + 5,
            20,
            Color.White
        );
        
        hgt += elementHeight;
    }

    #endregion

    #region Params
    
    public static void Parametres()
    {
        const float SPACEBETWWENBUTTONS = 50f;

        Vector2 BackRecSize = new(GetScreenWidth() * 0.5f, 3 * 100f);
        Vector2 BackRecPosition = new(GetScreenWidth() * 0.5f - BackRecSize.X * 0.5f, GetScreenHeight() * 0.5f - BackRecSize.Y * 0.5f);
        Rectangle backRec = new(BackRecPosition, BackRecSize);

        Vector2 SensibiliteSize = new(backRec.X - 10, 10f);
        Vector2 SensibilitePosition = new(BackRecPosition.X + SensibiliteSize.X * 0.5f, backRec.Y + backRec.Height / 2 - SPACEBETWWENBUTTONS / 2);
        Rectangle sensibiliteRec = new(SensibilitePosition, SensibiliteSize);

        Vector2 SensibiliteActuelSize = new((sensibiliteRec.Width) * (GameLoop.sensibilite * 10), SensibiliteSize.Y);
        Vector2 SensibiliteActuelPosition = new(SensibilitePosition.X, SensibilitePosition.Y);
        Rectangle sensibiliteActuelRec = new(SensibiliteActuelPosition, SensibiliteActuelSize);

        Vector2 btnQuitterSize = new(backRec.Width - 3 * SPACEBETWWENBUTTONS, 50f);
        Vector2 btnQuitterPosition = new(backRec.X + backRec.Width/2 - btnQuitterSize.X/2, backRec.Y + backRec.Height/2 + SPACEBETWWENBUTTONS / 2);
        Rectangle btnQuitter = new(btnQuitterPosition, btnQuitterSize);

        Boutton bouton = new(btnQuitter, "Quitter", Color.Red, Color.White, new(209, 14, 0, 255), Color.White);

        int fontSize = 25;
        float decalage = 10;

        Console.WriteLine(GameLoop.sensibilite * 10);

        //DrawRectangleRec(backRec, new(0, 0, 0, 100));

        if (IsMouseButtonReleased(MouseButton.Left))
            ChoseSensi = false;

        if (CheckCollisionPointRec(GetMousePosition(), sensibiliteRec))
        {
            SetMouseCursor(MouseCursor.PointingHand);
            if (IsMouseButtonDown(MouseButton.Left))
                ChoseSensi = true;
        }
        else
            SetMouseCursor(MouseCursor.Default);

        if (ChoseSensi)
        {
            GameLoop.sensibilite = ((GetMousePosition().X - sensibiliteRec.X) / sensibiliteRec.Width) / 10;
            //GameLoop.sensibilité = sensibiliteRec.Width / GameLoop.MAX_SENSI * (GetMousePosition().X - SensibiliteActuelPosition.X) / 10;
        }

        if (GameLoop.sensibilite * 10 < GameLoop.MIN_SENSI - 0.01f)
            GameLoop.sensibilite = GameLoop.MIN_SENSI / 10;
        else if (GameLoop.sensibilite * 100 > GameLoop.MAX_SENSI)
            GameLoop.sensibilite = GameLoop.MAX_SENSI / 100;


        if (bouton.CheckCollision(GetMousePosition()))
        {
            SetMouseCursor(MouseCursor.PointingHand);
            if(IsMouseButtonDown(MouseButton.Left))
                GameLoop.ferme = true;

        }
        bouton.Draw();
        bouton.DrawContour(Color.Black, 1);



        DrawRectangleRounded(backRec, 0.075f, 1, new(0, 0, 0, 100));

        DrawRectangleRounded(sensibiliteRec, 1f, 100, Color.Gray);
        DrawRectangleRounded(sensibiliteActuelRec, 1f, 100, Color.Black);

        DrawText($"Sensibilité", (int)(sensibiliteRec.X - MeasureText("Sensibilité", fontSize) - decalage), (int)(sensibiliteRec.Y + sensibiliteRec.Height*0.5f - fontSize*0.5f), fontSize, Color.White);
        DrawText($"{Math.Round(GameLoop.sensibilite * 100, 2)}", (int)(sensibiliteRec.X + sensibiliteRec.Width + MeasureText($"{Math.Round(GameLoop.sensibilite * 100, 2)}", fontSize) + decalage), (int)(sensibiliteRec.Y + sensibiliteRec.Height*0.5f - fontSize*0.5f), fontSize, Color.White);
    }
    
    #endregion
}