using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Game;

public static class Gui
{
    public static List<string> DebugContent = new();
    /// <summary>
    /// Affiche le GUI
    /// </summary>
    public static void Render()
    {
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
    }
}