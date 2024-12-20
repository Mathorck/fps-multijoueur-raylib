using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Game;

public static class Gui
{
    /// <summary>
    /// Affiche le GUI
    /// </summary>
    public static void Render()
    {
        Crossair();
    }

    /// <summary>
    /// Affiche le crossair
    /// </summary>
    private static void Crossair()
    {
        Color color = new Color(70,70,70,255);
        int crossWidth = 20;
        int crossHeight = 20;
        int crossWeight = 2; 

        int width = GetScreenWidth();
        int height = GetScreenHeight();
        
        DrawRectangle(width / 2 - crossWidth / 2, height / 2 - crossWeight / 2, crossWidth, crossWeight, color);
        
        DrawRectangle(width / 2 - crossWeight / 2 , height / 2 - crossHeight / 2, crossWeight, crossHeight, color);
    }


}