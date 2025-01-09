using static Raylib_cs.Raylib;
using System.Numerics;
using Raylib_cs;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;
using System.Text;
using static System.Formats.Asn1.AsnWriter;
using System.ComponentModel;


namespace DeadOpsArcade3D
{
    class Program
    {
        static unsafe void Main(string[] args)
        {

            InitWindow(GetScreenWidth(), GetScreenHeight(), "Dead Ops Arcade");
            ToggleFullscreen();
            SetTargetFPS(60);

            Camera3D camera = new Camera3D();
            camera.Position = new Vector3(0.0f, 2.0f, 4.0f);
            camera.Target = new Vector3(0.0f, 2.0f, 0.0f);
            camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.FovY = 60.0f;
            camera.Projection = CameraProjection.Perspective;

            

            #region character avec animations
            // Load gltf model animations
            // Load gltf model

            //RoboCharacter
            Model character = LoadModel("ressources/model3d/character/robot.glb");
            Vector3 characterPositioin = new Vector3(0, 0, 5f);
            string fileName = "ressources/model3d/character/robot.glb";
            int animCount = 0;
            int animIndex = 0;
            int animCurrentFrame = 0;
            sbyte[] fileNameBytes = new sbyte[fileName.Length + 1];
            for (int i = 0; i < fileName.Length; i++)
            {
                fileNameBytes[i] = (sbyte)fileName[i];
            }
            fileNameBytes[fileName.Length] = 0;

            //GreenMan Character
            Model greenMan = LoadModel("ressources/model3d/character/greenman.glb");
            Vector3 greenManPositioin = new Vector3(5f, 0, 5f);
            string greenManfileName = "ressources/model3d/character/greenman.glb";
            int greenMananimCount = 0;
            int greenMananimIndex = 0;
            int greenMananimCurrentFrame = 0;
            sbyte[] greenManfileNameBytes = new sbyte[greenManfileName.Length + 1];
            for (int i = 0; i < greenManfileName.Length; i++)
            {
                greenManfileNameBytes[i] = (sbyte)greenManfileName[i];
            }
            greenManfileNameBytes[greenManfileName.Length] = 0;
            #endregion

            DisableCursor();

            while (!WindowShouldClose())
            {
                UpdateCameraPro(ref camera,
                    new Vector3(
                        IsKeyDown(KeyboardKey.W) * 0.1f - IsKeyDown(KeyboardKey.S) * 0.1f,
                        IsKeyDown(KeyboardKey.D) * 0.1f - IsKeyDown(KeyboardKey.A) * 0.1f,
                        0.0f),
                    new Vector3(
                        GetMouseDelta().X * 0.05f,
                        GetMouseDelta().Y * 0.05f,
                        0.0f),
                    0f);



                #region Animations

                unsafe
                {
                    fixed (sbyte* pFileName = fileNameBytes)
                    //fixed (int* pAnimCount = &animCount)
                    {
                        ModelAnimation* characterAnimations = LoadModelAnimations(pFileName, &animCount);

                        Console.WriteLine("Nombre d'animations chargées : " + animCount);

                        // Select current animation
                        if (IsMouseButtonPressed(MouseButton.Right)) animIndex = (animIndex + 1) % animCount;
                        else if (IsMouseButtonPressed(MouseButton.Left)) animIndex = (animIndex + animCount - 1) % animCount;

                        // Update model animation
                        ModelAnimation anim = characterAnimations[animIndex];
                        animCurrentFrame = (animCurrentFrame + 1) % anim.FrameCount;
                        UpdateModelAnimation(character, anim, animCurrentFrame);
                    }
                }

                unsafe
                {
                    fixed (sbyte* pFileName = greenManfileNameBytes)
                    //fixed (int* pAnimCount = &animCount)
                    {
                        ModelAnimation* greenManAnimations = LoadModelAnimations(pFileName, &greenMananimCount);

                        Console.WriteLine("Nombre d'animations chargées : " + greenMananimCount);

                        // Select current animation
                        if (IsMouseButtonPressed(MouseButton.Right)) greenMananimIndex = (greenMananimIndex + 1) % greenMananimCount;
                        else if (IsMouseButtonPressed(MouseButton.Left)) greenMananimIndex = (greenMananimIndex + greenMananimCount - 1) % greenMananimCount;

                        // Update model animation
                        ModelAnimation greenMananim = greenManAnimations[greenMananimIndex];
                        greenMananimCurrentFrame = (greenMananimCurrentFrame + 1) % greenMananim.FrameCount;
                        UpdateModelAnimation(greenMan, greenMananim, greenMananimCurrentFrame);
                    }
                }

                #endregion


                BeginDrawing();

                BeginMode3D(camera);

                //Map
                ClearBackground(Color.White);
                DrawPlane(new Vector3(0.0f, 0.0f, 0.0f), new Vector2(32.0f, 32.0f), Color.LightGray);

                #region draw model

                //Dessiner les moodel 3d sur la map          
                DrawModel(character, characterPositioin, 1f, Color.DarkGray);
                DrawModel(greenMan, greenManPositioin, 1f, Color.DarkGray);
                #endregion


                //Dessiner le sol
                DrawGrid(20, 10.0f);        

                EndMode3D();

                EndDrawing();
            }
            #region unload model et texture
            //
            //Unload
            //
            UnloadModel(character);
            UnloadModel(greenMan);
            #endregion
            CloseWindow();

        }
    }

}