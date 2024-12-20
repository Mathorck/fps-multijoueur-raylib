using System.Numerics;
using static Raylib_cs.Raylib;
using Raylib_cs;

namespace DeadOpsArcade3D.Game;

public static class Map
{
    public static void Render()
    {
        ClearBackground(Color.White);
        DrawPlane(new Vector3(0.0f, 0.0f, 0.0f), new Vector2(64.0f, 64.0f), Color.LightGray);

        // Dessiner des cubes comme obstacles
        Raylib.DrawCube(new Vector3(2.0f, 1.0f, 2.0f), 2.0f, 2.0f, 2.0f, Color.Blue); // Premier cube
        Raylib.DrawCube(new Vector3(-5.0f, 1.0f, 3.0f), 1.5f, 1.5f, 1.5f, Color.Green); // Deuxième cube
        Raylib.DrawCube(new Vector3(4.0f, 1.0f, -4.0f), 1.0f, 1.0f, 1.0f, Color.Red);  // Troisième cube
        Raylib.DrawCube(new Vector3(-12.0f, 4.0f, -8.0f), 7.0f, 7.0f, 2.0f, Color.Yellow); // Quatrième cube
                                                                                           //Raylib.DrawBoundingBox new BoundingBox(new Vector3(-12.0f, 4.0f, -8.0f), 7.0f, 7.0f, 2.0f, Color.Yellow);

        // Mur avant
        Raylib.DrawCube(new Vector3(0.0f, 1.0f, -32.0f), 64.0f, 10.0f, 1.0f, Color.DarkGray);
        // Mur arrière
        Raylib.DrawCube(new Vector3(0.0f, 1.0f, 32.0f), 64.0f, 10.0f, 1.0f, Color.DarkGray);
        // Mur gauche
        Raylib.DrawCube(new Vector3(-32.0f, 1.0f, 0.0f), 1.0f, 10.0f, 64.0f, Color.DarkGray);
        // Mur droit
        Raylib.DrawCube(new Vector3(32.0f, 1.0f, 0.0f), 1.0f, 10.0f, 64.0f, Color.DarkGray);

    }
}