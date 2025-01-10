using System.Numerics;
using DeadOpsArcade3D.GameElement;
using static Raylib_cs.Raylib;
using Raylib_cs;

namespace DeadOpsArcade3D.Game;

public static class Map
{
    private static Image map;
    private static Model model;
    public static unsafe Color* MapPixels;
    public static Texture2D cubicmap;

    public static Vector3 mapPosition = new (-16.0f, 0.0f, -8.0f);



    //private static Texture2D ground;

    /*
    public static readonly List<BoundingBox> Obstacles = new List<BoundingBox>
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
        
        new BoundingBox(new Vector3(-32.0f, -1f, -32.0f), new Vector3(32.0f, 0f, 32.0f)),
        new BoundingBox(new Vector3(-32.0f, 10f, -32.0f), new Vector3(32.0f, 11f, 32.0f))
    };
    */
    public static unsafe void Init()
    {
        //ground = LoadTexture("./ressources/textures/ground.png");
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
        /*
        ClearBackground(Color.White);
        DrawBBT(Obstacles[8],ground);

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


        foreach (BoundingBox bb in Obstacles)
        {
            Raylib.DrawBoundingBox(bb,Color.Red);
        }

        */
        
        DrawModel(model, mapPosition, 1f, Color.White);


    }
    /*
    /// <summary>
    /// Dessine un cube avec une texture.
    /// </summary>
    /// <param name="texture">Texture à appliquer au cube.</param>
    /// <param name="position">Position centrale du cube.</param>
    /// <param name="size">Taille du cube (largeur, hauteur et profondeur).</param>
    public static unsafe void DrawBBT(BoundingBox boundingBox, Texture2D texture)
    {
        float width = boundingBox.Max.X - boundingBox.Min.X;
        float height = boundingBox.Max.Y - boundingBox.Min.Y;
        float depth = boundingBox.Max.Z - boundingBox.Min.Z;

        Vector3 position = new Vector3(
            (boundingBox.Min.X + boundingBox.Max.X) / 2,
            (boundingBox.Min.Y + boundingBox.Max.Y) / 2,
            (boundingBox.Min.Z + boundingBox.Max.Z) / 2
        );

        Material material = Raylib.LoadMaterialDefault();
        Raylib.SetMaterialTexture(ref material, MaterialMapIndex.Albedo, texture);

        // Générer le maillage du cube
        Mesh mesh = Raylib.GenMeshCube(width, height, depth);

        // Calculer les facteurs de répétition pour chaque face
        float repeatFactorX = width / texture.Width;
        float repeatFactorY = height / texture.Height;

        // Ajuster les coordonnées de texture pour répéter la texture
        for (int i = 0; i < mesh.VertexCount; i++)
        {
            // Chaque sommet a deux valeurs de coordonnées de texture : X et Y
            int texcoordIndexX = i * 2;     // Coordonnée X de texture (indice pair)
            int texcoordIndexY = i * 2 + 1; // Coordonnée Y de texture (indice impair)

            // Appliquer la répétition de la texture
            mesh.TexCoords[texcoordIndexX] = mesh.TexCoords[texcoordIndexX] * repeatFactorX;
            mesh.TexCoords[texcoordIndexY] = mesh.TexCoords[texcoordIndexY] * repeatFactorY;
        }

        // Créer le modèle avec le maillage modifié
        Model model = Raylib.LoadModelFromMesh(mesh);
        model.Materials[0] = material;

        // Dessiner le modèle
        Raylib.DrawModel(model, position, 1.0f, Color.White);

        // Libérer la mémoire
        Raylib.UnloadModel(model);
    }
    */





}