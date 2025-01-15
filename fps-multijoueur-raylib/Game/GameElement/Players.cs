using System.Numerics;
using DeadOpsArcade3D.Multiplayer;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Game.GameElement;

public class Player
{
    public const float JUMP_HEIGHT = 4f;
    public const float JUMP_SPEED = 0.3f;
    public const float PLAYER_SPEED = 0.05f;
    public const float SPRINT_SPEED = 0.09f;

    /// <summary>Liste des qui contient tous les joueurs </summary>
    public static List<Player> PlayerList = new();

    public static Model DefaultModel;

    private static bool canJump = true;
    private static bool canFall = true;
    private static bool isJumping;
    private static float jump;
    public static float rotation;
    public static float Health = 100;
    public static float Bullet = 30;
    public static string Nom = "Player";

    private int animCurrentFrame;
    private int animIndex;
    public BoundingBox HitBox;
    private float life;

    public Vector3 Position;
    private string pseudo;
    private readonly Texture2D pseudoTexture;
    public Vector3 Rotation;

    public Vector3 Size;
    public float Speed;

    public Player(float x, float y, float z, float xRot, float yRot, float zRot, string pseudo)
    {
        Position = new Vector3(x, y, z);
        Size = GetModelBoundingBox(DefaultModel).Max * 0.3f;
        Speed = 1f;
        life = 100f;
        HitBox = new BoundingBox(Position, Size);
        Rotation = new Vector3(xRot, yRot, zRot);
        this.pseudo = pseudo;

        rotation = float.Atan2(Rotation.X - Position.X, Rotation.Z - Position.Z) * (180 / float.Pi);

        int Textsize = 20;
        Image img = GenImageText(MeasureText(this.pseudo, Textsize), Textsize, this.pseudo);
        pseudoTexture = LoadTextureFromImage(img);
    }

    public Player(Player player)
    {
        Position = player.Position;
        Size = player.Size;
        Speed = player.Speed;
        life = player.life;
        HitBox = player.HitBox;
        Rotation = player.Rotation;
        pseudo = player.pseudo;
    }

    /// <summary>
    ///     Cela dessine tous les joueurs
    /// </summary>
    /// <param name="playerList"></param>
    public static void DrawAll(List<Player> playerList)
    {
        if (playerList == null || playerList.Count == 0)
            //Client.ConsoleError("La liste des joueurs est vide ou null.");
            return;

        try
        {
            for (int i = 0; i < playerList.Count; i++) playerList[i].Draw();
            //playerList[i].Animation();
        }
        catch (Exception e)
        {
            Client.ConsoleError($"Error: {e.Message}");
        }
    }

    /// <summary>
    ///     Permet l'affichage de tous les Joueurs
    /// </summary>
    /// <param name="playerList">Liste des Joueurs</param>
    public void Draw()
    {
        DrawBillboard(GameLoop.camera, pseudoTexture, Position, 0.5f, Color.White);


        DrawModelEx(
            DefaultModel, // Model to draw
            Position - new Vector3(0, 0.39f, 0), // Position in 3D space
            new Vector3(0, 1, 0), // Rotation axis (Y-axis for character rotation)
            float.Atan2(Rotation.X - Position.X, Rotation.Z - Position.Z) *
            (180 / float.Pi), // Rotation angle (in degrees)
            new Vector3(0.12f, 0.12f, 0.12f), // Scale (matching the 0.3f from your bounding box)
            Color.Blue // Tint color
        );
    }

    /// <summary>
    ///     Gère le mouvement du joueur principal
    /// </summary>
    /// <param name="camera"></param>
    public static unsafe void Movement(ref Camera3D camera)
    {
        canJump = false;
        canFall = true;

        Gui.DebugContent.Add(
            $"Position: [{float.Round(camera.Position.X, 2)} | {float.Round(camera.Position.Y, 2)} | {float.Round(camera.Position.Z, 2)}]");

        Vector3 oldCamPos = camera.Position;
        Vector3 initialTarget = camera.Target - camera.Position;
        Vector3 deplacement = new();

        float speed = IsKeyDown(KeyboardKey.LeftShift) ? SPRINT_SPEED : PLAYER_SPEED;

        deplacement.X = IsKeyDown(KeyboardKey.W) * speed - IsKeyDown(KeyboardKey.S) * PLAYER_SPEED;
        deplacement.Y = IsKeyDown(KeyboardKey.D) * PLAYER_SPEED - IsKeyDown(KeyboardKey.A) * PLAYER_SPEED;

        camera.Up = new Vector3(0, 1, 0);


        //Jumping(ref camera);

        // Mise à jour de la caméra (Déplacement)
        UpdateCameraPro(ref camera,
            deplacement,
            new Vector3(
                GetMouseDelta().X * GameLoop.sensibilité,
                GetMouseDelta().Y * GameLoop.sensibilité,
                0.0f),
            0f
        );
        Vector2 playerPos = new(camera.Position.X, camera.Position.Z);

        int playerCellX = (int)(playerPos.X - Map.MapPosition.X + 0.5f);
        int playerCellY = (int)(playerPos.Y - Map.MapPosition.Z + 0.5f);

        if (playerCellX < 0)
            playerCellX = 0;
        else if (playerCellX >= Map.Cubicmap.Width) playerCellX = Map.Cubicmap.Width - 1;

        if (playerCellY < 0)
            playerCellY = 0;
        else if (playerCellY >= Map.Cubicmap.Height) playerCellY = Map.Cubicmap.Height - 1;

        for (int y = 0; y < Map.Cubicmap.Height; y++)
        for (int x = 0; x < Map.Cubicmap.Width; x++)
        {
            Color* mapPixelsData = Map.MapPixels;

            // Collision: Color.white pixel, only check R channel
            Rectangle rec = new(
                Map.MapPosition.X - 0.5f + x * 1.0f,
                Map.MapPosition.Z - 0.5f + y * 1.0f,
                1.0f,
                1.0f
            );

            bool colisionX = false, colisionZ = false;

            const float bSize = 0.2f, sSize = 0.1f;

            Rectangle recx = new(camera.Position.X - bSize / 2, camera.Position.Z - sSize / 2,
                new Vector2(bSize, sSize));
            Rectangle recz = new(camera.Position.X - sSize / 2, camera.Position.Z - bSize / 2,
                new Vector2(sSize, bSize));

            DrawRectangleRec(recx, Color.Blue);
            DrawRectangleRec(recz, Color.Black);

            colisionX = CheckCollisionRecs(recx, rec);
            colisionZ = CheckCollisionRecs(recz, rec);


            if (mapPixelsData[y * Map.Cubicmap.Width + x].R == 255)
            {
                if (colisionX)
                {
                    camera.Position.X = oldCamPos.X;
                    camera.Target = camera.Position + initialTarget;
                }

                if (colisionZ)
                {
                    camera.Position.Z = oldCamPos.Z;
                    camera.Target = camera.Position + initialTarget;
                }
            }
        }
    }


    public void Animation()
    {
        //Default Character
        string fileName = "ressources/model3d/character/robot.glb";
        int animCount = 0;
        sbyte[] fileNameBytes = new sbyte[fileName.Length + 1];
        for (int i = 0; i < fileName.Length; i++) fileNameBytes[i] = (sbyte)fileName[i];
        fileNameBytes[fileName.Length] = 0;
        unsafe
        {
            fixed (sbyte* pFileName = fileNameBytes)
            {
                ModelAnimation* characterAnimations = LoadModelAnimations(pFileName, &animCount);

                Console.WriteLine("Nombre d'animations chargées : " + animCount);

                // Select current animation

                // Faire défiler l'animation avec les cliques de la souris
                /*if (IsMouseButtonPressed(MouseButton.Right)) animIndex = (animIndex + 1) % animCount;
                else if (IsMouseButtonPressed(MouseButton.Left)) animIndex = (animIndex + animCount - 1) % animCount;*/

                /*
                    0 Emote
                    1 Mort
                    2 Idle
                    3 Sauter
                    4 Non
                    5 Taper
                    6 Courir
                    7 S'asseoir
                    8 Se lever
                    9 Pouce
                    10 Marcher
                    11 Sauter comme mario
                    12 Salut
                    13 Oui
                */
                //Control Animations                
                if (IsKeyDown(KeyboardKey.W)) animIndex = 10;
                if (IsKeyPressed(KeyboardKey.Space)) animIndex = 3;
                //Mettre un timer puis mettre l'animation idle
                if (IsMouseButtonPressed(MouseButton.Left)) animIndex = 5;
                if (IsKeyPressed(KeyboardKey.LeftControl) && IsKeyPressed(KeyboardKey.One))
                {
                    animIndex = 0;
                    DrawText("Vous faites une emote!", GetScreenWidth() / 2, GetScreenHeight() / 3, 10,
                        Color.DarkPurple);
                }

                if (IsKeyPressed(KeyboardKey.LeftControl) && IsKeyPressed(KeyboardKey.Two)) animIndex = 9;
                if (IsKeyPressed(KeyboardKey.LeftControl) && IsKeyPressed(KeyboardKey.Three)) animIndex = 11;
                if (IsKeyPressed(KeyboardKey.LeftControl) && IsKeyPressed(KeyboardKey.Four)) animIndex = 12;
                if (IsKeyPressed(KeyboardKey.LeftControl) && IsKeyPressed(KeyboardKey.Five)) animIndex = 4;
                if (IsKeyPressed(KeyboardKey.LeftControl) && IsKeyPressed(KeyboardKey.Six)) animIndex = 13;

                // Update model animation
                ModelAnimation anim = characterAnimations[animIndex];
                animCurrentFrame = (animCurrentFrame + 1) % anim.FrameCount;
                UpdateModelAnimation(DefaultModel, anim, animCurrentFrame);
            }
        }
    }
}