using System.Numerics;
using DeadOpsArcade3D.Game.GameElement;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Game;

public static class Gui
{
    public static List<string> DebugContent = new();
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

    /// <summary>
    ///     Affiche le crossair
    /// </summary>
    private static void Crossair()
    {
        var color = new Color(0, 0, 0, 50);
        var crossWidth = 20;
        var crossHeight = 20;
        var crossWeight = 2;

        var width = GetScreenWidth();
        var height = GetScreenHeight();

        DrawRectangle(width / 2 - crossWidth / 2, height / 2 - crossWeight / 2, crossWidth, crossWeight, Color.Black);

        DrawRectangle(width / 2 - crossWeight / 2, height / 2 - crossHeight / 2, crossWeight, crossHeight, Color.Black);
    }

    private static void Debug()
    {
        Console.WriteLine();

        var output = "";
        foreach (var text in DebugContent) output += text + "\n";
        DrawText(output, 20, 20, 20, Color.Black);
        DebugContent.Clear();

        DrawFPS(10, 10);
    }

    private static void Weapon()
    {
        DrawTexture(wepon, (GetScreenWidth() - wepon.Width) / 2 + 100, GetScreenHeight() - wepon.Height, Color.White);
    }

    private static void MiniMap()
    {
        DrawTextureEx(Map.cubicmap, new Vector2(GetScreenWidth() - Map.cubicmap.Width * 4 - 20, 20), 0.0f, 4.0f,
            Color.White);
        DrawRectangleLines(GetScreenWidth() - Map.cubicmap.Width * 4 - 20, 20, Map.cubicmap.Width * 4,
            Map.cubicmap.Height * 4, Color.Green);

        // Draw player position radar
        Vector2 playerPos = new(GameLoop.camera.Position.X, GameLoop.camera.Position.Z);

        var playerCellX = (int)(playerPos.X - Map.mapPosition.X + 0.5f);
        var playerCellY = (int)(playerPos.Y - Map.mapPosition.Z + 0.5f);

        DrawRectangle(GetScreenWidth() - Map.cubicmap.Width * 4 - 20 + playerCellX * 4, 20 + playerCellY * 4, 4, 4,
            Color.Red);
    }

    private static void VieAndBullet()
    {
        var screenWidth = GetScreenWidth();
        var screenHeight = GetScreenHeight();

        DrawRectangleRounded(healthBar, 0.3f, 2, new Color(100, 100, 100, 100));
        DrawText("Vie", 40, screenHeight - 50, 20, Color.Yellow);
        DrawText($"{Player.Health}", 90, screenHeight - 70, 50, Color.Yellow);

        DrawRectangle(screenWidth - 220, screenHeight - 60, 200, 40, Color.Gray);
        DrawText("Balles: " + Player.Bullet, screenWidth - 215, screenHeight - 55, 20, Color.White);
    }

    private static void ShowTab()
    {
    }
}