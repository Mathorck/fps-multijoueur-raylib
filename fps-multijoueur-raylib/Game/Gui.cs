using System.Numerics;
using DeadOpsArcade3D.Game.GameElement;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Game;

public static class Gui
{
    public static List<string> DebugContent = new();
    public static List<string> ErrorContent = new();
    public static bool IsTabOppened = false;

    private static readonly Rectangle healthBar = new(20, GetScreenHeight() - 80, 180, 60);
    private static Texture2D wepon;

    public static void Init()
    {
        wepon = LoadTexture("./ressources/textures/ShootGun.png");
    }

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
        Debug();
    }
    
    public static void Unload()
    {
        UnloadTexture(wepon);
    }

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

    private static void Debug()
    {
        string? output = "";
        foreach (string? text in DebugContent) output += text + "\n";
        DrawText(output, 20, 20, 20, Color.Black);
        DebugContent.Clear();

        DrawFPS(10, 10);

        string? output2 = "";
        foreach (string? text in ErrorContent) output2 += text + "\n";
        DrawText(output2, 100, 20, 20, Color.Red);
    }

    private static void Weapon()
    {
        DrawTexture(wepon, (GetScreenWidth() - wepon.Width) / 2 + 100, GetScreenHeight() - wepon.Height, Color.White);
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
    }

    private static void ShowTab()
    {
        float SHeight = GetScreenHeight() * 0.5f;
        float SWidth = GetScreenWidth();

        int tabWidth = 400;

        DrawRectangle(
            (int)(SWidth - tabWidth),
            50,
            (int)(SWidth + tabWidth),
            (int)(SHeight - 100),
            new Color(0, 0, 0, 100)
        );
    }
}