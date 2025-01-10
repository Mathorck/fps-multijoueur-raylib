using System.Numerics;
using System.Runtime.CompilerServices;
using DeadOpsArcade3D.Game;
using DeadOpsArcade3D.Multiplayer;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Game.GameElement
{
    public class Player
    {
        public const float JUMP_HEIGHT = 4f;
        public const float JUMP_SPEED = 0.3f;
        public const float PLAYER_SPEED = 0.05f;
        public const float SPRINT_SPEED = 0.09f;

        /// <summary>Liste des qui contient tous les joueurs </summary>
        public static List<Player> PlayerList = new List<Player>();

        public static Model DefaultModel;

        public int animIndex;
        public int animCurrentFrame;

        private static bool canJump = true;
        private static bool canFall = true;
        private static bool isJumping = false;
        private static float jump = 0;
        public static float rotation = 0;


        public Vector3 Position;
        public Vector3 Rotation;

        public Vector3 Size;
        public float Speed;
        public float Life;
        public BoundingBox HitBox;

        public Player(float x, float y, float z, float xRot, float yRot, float zRot)
        {
            Position = new Vector3(x, y, z);
            Size = GetModelBoundingBox(DefaultModel).Max * 0.3f;
            Speed = 1f;
            Life = 100f;
            HitBox = new BoundingBox(Position, Size);
            Rotation = new Vector3(xRot, yRot, zRot);

            rotation = float.Atan2(Rotation.X - Position.X, Rotation.Z - Position.Z) * (180 / float.Pi);
        }

        public Player(Player player)
        {
            Position = player.Position;
            Size = player.Size;
            Speed = player.Speed;
            Life = player.Life;
            HitBox = player.HitBox;
            Rotation = player.Rotation;
        }

        /// <summary>
        /// Cela dessine tout les joueurs
        /// </summary>
        /// <param name="playerList"></param>
        public static void DrawAll(List<Player> playerList)
        {
            if (playerList == null || playerList.Count == 0)
            {
                Client.ConsoleError("La liste des joueurs est vide ou null.");
                return;
            }
            try
            {
                for (int i = 0; i < playerList.Count; i++)
                {
                    playerList[i].Draw();
                }
            }
            catch (Exception e)
            {
                Client.ConsoleError($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Permet l'affichage de tous les Joueurs
        /// </summary>
        /// <param name="playerList">Liste des Joueurs</param>
        public void Draw()
        {
            DrawModelEx(
                DefaultModel,                   // Model to draw
                Position - new Vector3(0, 0.39f, 0),// Position in 3D space
                new Vector3(0, 1, 0),           // Rotation axis (Y-axis for character rotation)
                float.Atan2(Rotation.X - Position.X, Rotation.Z - Position.Z) * (180 / float.Pi), // Rotation angle (in degrees)
                new Vector3(0.12f, 0.12f, 0.12f),  // Scale (matching the 0.3f from your bounding box)
                Color.Blue                      // Tint color
            );
        }

        /// <summary>
        /// Gère le mouvement du joueur principal
        /// </summary>
        /// <param name="camera"></param>
        public static unsafe void Movement(ref Camera3D camera)
        {
            canJump = false;
            canFall = true;

            Gui.DebugContent.Add($"Position: [{float.Round(camera.Position.X, 2)} | {float.Round(camera.Position.Y, 2)} | {float.Round(camera.Position.Z, 2)}]");

            Vector3 oldCamPos = camera.Position;
            Vector3 initialTarget = camera.Target - camera.Position;
            Vector3 deplacement = new Vector3();

            float speed = IsKeyDown(KeyboardKey.LeftShift) ? SPRINT_SPEED : PLAYER_SPEED;

            deplacement.X = IsKeyDown(KeyboardKey.W) * speed - IsKeyDown(KeyboardKey.S) * PLAYER_SPEED;
            deplacement.Y = IsKeyDown(KeyboardKey.D) * PLAYER_SPEED - IsKeyDown(KeyboardKey.A) * PLAYER_SPEED;

            camera.Up = new Vector3(0, 1, 0);


            //Jumping(ref camera);

            // Mise a jour de la caméra (Déplacement)
            UpdateCameraPro(ref camera,
                deplacement,
                new Vector3(
                    GetMouseDelta().X * GameLoop.sensibilité,
                    GetMouseDelta().Y * GameLoop.sensibilité,
                    0.0f),
                0f
            );
            Vector2 playerPos = new(camera.Position.X, camera.Position.Z);

            int playerCellX = (int)(playerPos.X - Map.mapPosition.X + 0.5f);
            int playerCellY = (int)(playerPos.Y - Map.mapPosition.Z + 0.5f);

            if (playerCellX < 0)
            {
                playerCellX = 0;
            }
            else if (playerCellX >= Map.cubicmap.Width)
            {
                playerCellX = Map.cubicmap.Width - 1;
            }

            if (playerCellY < 0)
            {
                playerCellY = 0;
            }
            else if (playerCellY >= Map.cubicmap.Height)
            {
                playerCellY = Map.cubicmap.Height - 1;
            }

            for (int y = 0; y < Map.cubicmap.Height; y++)
            {
                for (int x = 0; x < Map.cubicmap.Width; x++)
                {
                    Color* mapPixelsData = Map.MapPixels;

                    // Collision: Color.white pixel, only check R channel
                    Rectangle rec = new(
                        Map.mapPosition.X - 0.5f + x * 1.0f,
                        Map.mapPosition.Z - 0.5f + y * 1.0f,
                        1.0f,
                        1.0f
                    );

                    bool colisionX = false, colisionZ = false;

                    //if (camera.Position.X - oldCamPos.X > 0)
                    //    colisionX = CheckCollisionCircleRec(new(camera.Position.X, camera.Position.Z), 0.1f, rec);
                    //else
                    //    colisionX = CheckCollisionCircleRec(new(camera.Position.X, camera.Position.Z), 0.1f, rec);
                    //if (camera.Position.Z - oldCamPos.Z > 0)
                    //    colisionZ = CheckCollisionCircleRec(new(camera.Position.Z, camera.Position.Z), 0.1f, rec);
                    //else
                    //    colisionZ = CheckCollisionCircleRec(new(camera.Position.Z, camera.Position.Z), 0.1f, rec);

                    //colisionX = CheckCollisionCircleRec(new(camera.Position.X, 0), 0.1f, rec);
                    //colisionZ = CheckCollisionCircleRec(new(0, camera.Position.Z), 0.1f, rec);


                    const float bSize = 0.2f, sSize = 0.1f;

                    Rectangle recx = new(camera.Position.X - bSize / 2, camera.Position.Z - sSize / 2, new(bSize, sSize));
                    Rectangle recz = new(camera.Position.X - sSize / 2, camera.Position.Z - bSize / 2, new(sSize, bSize));

                    DrawRectangleRec(recx, Color.Blue);
                    DrawRectangleRec(recz, Color.Black);

                    colisionX = CheckCollisionRecs(recx, rec);
                    colisionZ = CheckCollisionRecs(recz, rec);



                    if (mapPixelsData[y * Map.cubicmap.Width + x].R == 255)
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


                    //bool collision = CheckCollisionCircleRec(new Vector2(camera.Position.X, camera.Position.Z), 0.1f, rec);
                    //if ((mapPixelsData[y * Map.cubicmap.Width + x].R == 255) && collision)
                    //{
                    //    // Collision detected, reset camera position
                    //    camera.Position = oldCamPos;
                    //    camera.Target = camera.Position + initialTarget;
                    //}
                }
            }

        }


        public void Animation()
        {
            //Default Character
            string fileName = "ressources/model3d/character/robot.glb";
            int animCount = 0;
            sbyte[] fileNameBytes = new sbyte[fileName.Length + 1];
            for (int i = 0; i < fileName.Length; i++)
            {
                fileNameBytes[i] = (sbyte)fileName[i];
            }
            fileNameBytes[fileName.Length] = 0;
            unsafe
            {
                fixed (sbyte* pFileName = fileNameBytes)
                {
                    ModelAnimation* characterAnimations = LoadModelAnimations(pFileName, &animCount);

                    Console.WriteLine("Nombre d'animations chargées : " + animCount);

                    // Select current animation

                    // Faire défiler les animation avec les cliques de la souris            
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
                    if (IsKeyPressed(KeyboardKey.Space))
                    {
                        animIndex = 3;
                        //Mettre un timer puis mettre l'animation idle
                    }
                    if (IsMouseButtonPressed(MouseButton.Left)) animIndex = 5;
                    if (IsKeyPressed(KeyboardKey.LeftControl) && IsKeyPressed(KeyboardKey.One))
                    {
                        animIndex = 0;
                        DrawText("Vous faites une emote!", GetScreenWidth() / 2, GetScreenHeight() / 3, 10, Color.DarkPurple);
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

        /// <summary>
        /// Méthode qui gère le saut et la chute du joueur
        /// </summary>
        /// <param name="camera"></param>
        private static void Jumping(ref Camera3D camera)
        {
            if (canJump && !isJumping && IsKeyDown(KeyboardKey.Space))
                isJumping = true;

            if (isJumping)
            {
                float jumpProgress = jump / JUMP_HEIGHT;
                float jumpVelocity = JUMP_SPEED * (1f - jumpProgress + 0.1f);
                camera.Position.Y += jumpVelocity;
                jump += jumpVelocity;
                if (jump >= JUMP_HEIGHT)
                {
                    isJumping = false;
                    jump = 0;
                }
            }
            else if (canFall && !isJumping)
            {
                float fallProgress = jump / JUMP_HEIGHT;
                float fallVelocity = JUMP_SPEED * (fallProgress + 0.1f);
                camera.Position.Y -= fallVelocity;
                jump += fallVelocity;
            }
        }
    }
}
