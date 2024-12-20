using System.Numerics;
using static Raylib_cs.Raylib;
using Raylib_cs;

namespace DeadOpsArcade3D.Game;

public static class Map
{
    public static List<BoundingBox> Obstacles = new List<BoundingBox>
    {
        // Objets définis avec leurs BoundingBox respectifs
        new BoundingBox(new Vector3(1.0f, 0.0f, 1.0f), new Vector3(3.0f, 2.0f, 3.0f)), // Premier cube
        new BoundingBox(new Vector3(-6.0f, 0.0f, 2.0f), new Vector3(-4.0f, 2.0f, 4.0f)), // Deuxi�me cube
        new BoundingBox(new Vector3(3.5f, 0.0f, -4.5f), new Vector3(4.5f, 2.0f, -3.5f)), // Troisi�me cube
        new BoundingBox(new Vector3(-15.5f, 0.0f, -9.0f), new Vector3(-8.5f, 7.0f, -7.0f)), // Quatri�me cube

        // Murs
        new BoundingBox(new Vector3(-32.0f, -4.0f, -32.5f), new Vector3(32.0f, 6.0f, -31.5f)), // Mur avant
        new BoundingBox(new Vector3(-32.0f, -4.0f, 31.5f), new Vector3(32.0f, 6.0f, 32.5f)),  // Mur arri�re
        new BoundingBox(new Vector3(-32.5f, -4.0f, -32.0f), new Vector3(-31.5f, 6.0f, 32.0f)), // Mur gauche
        new BoundingBox(new Vector3(31.5f, -4.0f, -32.0f), new Vector3(32.5f, 6.0f, 32.0f)),  // Mur droit

        new BoundingBox(new Vector3(-32.0f, 0f, -32.0f), new Vector3(32.0f, 1f, 32.0f))
    };

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
        // Mur arri�re
        Raylib.DrawCube(new Vector3(0.0f, 1.0f, 32.0f), 64.0f, 10.0f, 1.0f, Color.DarkGray);
        // Mur gauche
        Raylib.DrawCube(new Vector3(-32.0f, 1.0f, 0.0f), 1.0f, 10.0f, 64.0f, Color.DarkGray);
        // Mur droit
        Raylib.DrawCube(new Vector3(32.0f, 1.0f, 0.0f), 1.0f, 10.0f, 64.0f, Color.DarkGray);


        foreach(BoundingBox bb in Obstacles)
        {
            Raylib.DrawBoundingBox(bb,Color.Red);
        }
    }
}