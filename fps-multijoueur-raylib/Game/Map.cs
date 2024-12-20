using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Game;

public static class Map
{
    public static List<BoundingBox> Obstacles = new List<BoundingBox>
    {
        new BoundingBox(new Vector3(1.0f, 0.0f, 1.0f), new Vector3(3.0f, 2.0f, 3.0f)), // Premier cube
        new BoundingBox(new Vector3(-6.0f, 0.0f, 2.0f), new Vector3(-4.0f, 2.0f, 4.0f)), // Deuxième cube
        new BoundingBox(new Vector3(3.5f, 0.0f, -4.5f), new Vector3(4.5f, 2.0f, -3.5f)), // Troisième cube
        new BoundingBox(new Vector3(-15.5f, 0.0f, -9.0f), new Vector3(-8.5f, 7.0f, -7.0f)), // Quatrième cube
        // Ajoutez d'autres obstacles ici si nécessaire
    };

    public static void Render()
    {
        ClearBackground(Color.White);
        DrawPlane(new Vector3(0.0f, 0.0f, 0.0f), new Vector2(64.0f, 64.0f), Color.LightGray);

        // Dessiner des cubes colorés
        DrawCube(new Vector3(2.0f, 1.0f, 2.0f), 2.0f, 2.0f, 2.0f, Color.Blue);
        DrawCube(new Vector3(-5.0f, 1.0f, 3.0f), 1.5f, 1.5f, 1.5f, Color.Green);
        DrawCube(new Vector3(4.0f, 1.0f, -4.0f), 1.0f, 1.0f, 1.0f, Color.Red);
        DrawCube(new Vector3(-12.0f, 4.0f, -8.0f), 7.0f, 7.0f, 2.0f, Color.Yellow);

        // Dessiner les murs
        DrawCube(new Vector3(0.0f, 1.0f, -32.0f), 64.0f, 10.0f, 1.0f, Color.DarkGray);
        DrawCube(new Vector3(0.0f, 1.0f, 32.0f), 64.0f, 10.0f, 1.0f, Color.DarkGray);
        DrawCube(new Vector3(-32.0f, 1.0f, 0.0f), 1.0f, 10.0f, 64.0f, Color.DarkGray);
        DrawCube(new Vector3(32.0f, 1.0f, 0.0f), 1.0f, 10.0f, 64.0f, Color.DarkGray);

        // Debug : Afficher les BoundingBox
        foreach (var obstacle in Obstacles)
        {
            DrawBoundingBox(obstacle, Color.Red);
        }
    }

    public static bool CheckCollisionWithObstacles(Vector3 newPosition, Vector3 size, float x, float y)
    {
        // Crée une BoundingBox représentant la position de la caméra avec la taille
        BoundingBox playerBox = new BoundingBox(newPosition - size / 2, newPosition + size + new Vector3(x , y, 0));

        // Parcourt la liste des obstacles pour vérifier les collisions
        foreach (var obstacle in Obstacles)
        {
            if (CheckCollisionBoxes(playerBox, obstacle))
            {
                return true; // Si une collision est détectée
            }
        }

        return false; // Aucun obstacle en collision
    }
}

