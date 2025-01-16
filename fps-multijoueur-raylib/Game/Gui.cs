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
        float SHeight = GetScreenHeight();
        float SWidth = GetScreenWidth();

        int tabWidth = 400;
        int tabPaddingR = (int)(SWidth - tabWidth*2);

        DrawRectangle(
            (int)(tabWidth),
            50,
            tabPaddingR,
            (int)(SHeight - 100),
            new Color(0, 0, 0, 100)
        );

        int hgt = 55; 
        
        DrawPlayerElement(Player.Nom, ref hgt, tabWidth, tabPaddingR);
        hgt += 10;

        for (int i = 0; i < Player.PlayerList.Count; i++)
        {
            Player plr = Player.PlayerList[i];
            
            DrawPlayerElement(plr.Pseudo,ref hgt, tabWidth, tabPaddingR);
            
        }
    }

    private static void DrawPlayerElement(string pseudo,ref int hgt,int tabWidth, int tabPaddingR)
    {
        DrawRectangle(
            tabWidth+5,
            hgt,
            tabPaddingR-10,
            hgt+5,
            new Color(0, 0, 0, 100)
        );
            
        DrawText(
            pseudo,
            tabWidth+10,
            hgt+10,
            20,
            Color.White
        );
        hgt += 10;
    }
}