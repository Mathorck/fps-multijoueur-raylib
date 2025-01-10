using System.Numerics;
using DeadOpsArcade3D.Game.GameElement;
using static Raylib_cs.Raylib;
using Raylib_cs;

namespace DeadOpsArcade3D.Game;

public static class Map
{
    private static Image map;
    public static Model model;
    public static unsafe Color* MapPixels;
    public static Texture2D cubicmap;
    public static Vector3 mapPosition = new(-16.0f, 0.0f, -8.0f);
    
    public static unsafe void Init()
    {
        map = LoadImage("./ressources/textures/Map.png");
        cubicmap = LoadTextureFromImage(map);
        Mesh mesh = Raylib.GenMeshCubicmap(map, new Vector3(1.0f, 1.0f, 1.0f));
        model = LoadModelFromMesh(mesh);

        Texture2D texture = LoadTexture("./ressources/textures/Under.png");

        Raylib.SetMaterialTexture(ref model, 0, MaterialMapIndex.Albedo, ref texture);

        MapPixels = LoadImageColors(map);
        UnloadImage(map);
    }

    public static void Render()
    {
        DrawModel(model, mapPosition, 1f, Color.White);
    }
}