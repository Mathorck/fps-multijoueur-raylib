using System.Numerics;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Game;

public static class Map
{
    private static Image map;
    private static Model model;
    public static unsafe Color* MapPixels;
    public static Texture2D Cubicmap;
    public static Vector3 MapPosition = new(-16.0f, 0.0f, -8.0f);

    public static unsafe void Init()
    {
        map = LoadImage("./ressources/textures/Map.png");
        Cubicmap = LoadTextureFromImage(map);
        Mesh mesh = GenMeshCubicmap(map, new Vector3(1.0f, 1.0f, 1.0f));
        model = LoadModelFromMesh(mesh);

        Texture2D texture = LoadTexture("./ressources/textures/Under.png");

        SetMaterialTexture(ref model, 0, MaterialMapIndex.Albedo, ref texture);

        MapPixels = LoadImageColors(map);
        UnloadImage(map);
    }

    public static void Unload()
    {
        UnloadImage(map);
        UnloadModel(model);
        UnloadTexture(Cubicmap);
    }

    public static void Render()
    {
        DrawModel(model, MapPosition, 1f, Color.White);
    }
}