using DeadOpsArcade3D.GameElement;
using Raylib_cs;
using System.Numerics;
using System.Xml.Serialization;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Game;

public static class Gui
{
    public static List<string> DebugContent = new();
    private static Texture2D wepon;

    public static void Init()
    {
        wepon = LoadTexture("./ressources/textures/ShootGun.png");
    }

    /// <summary>
    /// Affiche le GUI
    /// </summary>
    public static void Render()
    {
        Weapon();
        Crossair();
        Debug();
    }

    /// <summary>
    /// Affiche le crossair
    /// </summary>
    private static void Crossair()
    {
        Color color = new Color(0,0,0,50);
        int crossWidth = 20;
        int crossHeight = 20;
        int crossWeight = 5; 

        int width = GetScreenWidth();
        int height = GetScreenHeight();
        
        DrawRectangle(width / 2 - crossWidth / 2, height / 2 - crossWeight / 2, crossWidth, crossWeight, Color.Black);
        
        DrawRectangle(width / 2 - crossWeight / 2 , height / 2 - crossHeight / 2, crossWeight, crossHeight, Color.Black);
    }

    private static void Debug()
    {
        Console.WriteLine();

        string output = "";
        foreach (string text in DebugContent)
        {
            output += text + "\n";
        }
        DrawText(output, 20, 20,20, Color.Black);
        DebugContent.Clear();


        DrawTextureEx(Map.cubicmap, new Vector2(GetScreenWidth() - Map.cubicmap.Width * 4 - 20, 20), 0.0f, 4.0f, Color.White);
        DrawRectangleLines(GetScreenWidth() - Map.cubicmap.Width * 4 - 20, 20, Map.cubicmap.Width * 4, Map.cubicmap.Height * 4, Color.Green);

        // Draw player position radar
        Vector2 playerPos = new(GameLoop.camera.Position.X, GameLoop.camera.Position.Z);

        int playerCellX = (int)(playerPos.X - Map.mapPosition.X + 0.5f);
        int playerCellY = (int)(playerPos.Y - Map.mapPosition.Z + 0.5f);

        DrawRectangle(GetScreenWidth() - Map.cubicmap.Width * 4 - 20 + playerCellX * 4, 20 + playerCellY * 4, 4, 4, Color.Red);

        DrawFPS(10, 10);
    }

    private static void Weapon()
    {
        Raylib.DrawTexture(wepon, (GetScreenWidth() - wepon.Width) / 2 +100, GetScreenHeight() - wepon.Height, Color.White);
    }
}