using DeadOpsArcade3D.Multiplayer;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Game;

public class Launcher
{
    public const int MAX_INPUT_CHARS = 13;
    private static LauncherPage currentPage = LauncherPage.Main;
    
    private const int BTH_WIDTH = 425, BTN_HEIGHT = 75, FONT_SIZE = 25;
    const string JOIN_TEXT = "Rejoindre un Serveur", CREATE_JOIN_TEXT = "Créer un Serveur et rejoindre", CREATE_TEXT = "Créer un Serveur", NOM_DU_JEU = "Bienvenue !";
    
    private static Rectangle join = new(GetScreenWidth() * 0.5f - BTH_WIDTH * 0.5f, (GetScreenHeight() - 500 - 3 * BTN_HEIGHT) * 0.25f + 300, BTH_WIDTH, BTN_HEIGHT);
    private static Rectangle createJoin = new(GetScreenWidth() * 0.5f - BTH_WIDTH * 0.5f, 2 * (GetScreenHeight() - 500 - 3 * BTN_HEIGHT) * 0.25f + BTN_HEIGHT + 300, BTH_WIDTH, BTN_HEIGHT);
    private static Rectangle create = new(GetScreenWidth() * 0.5f - BTH_WIDTH * 0.5f, 3 * (GetScreenHeight() - 500 - 3 * BTN_HEIGHT) * 0.25f + 2 * BTN_HEIGHT + 300, BTH_WIDTH, BTN_HEIGHT);
    
    /// <summary>
    /// Création de la fenêtre de choix pour jouer
    /// </summary>
    public static void Init()
    {
        InitWindow(GetScreenWidth(), GetScreenHeight(), "Launcher");
        SetTargetFPS(60);
        
        join = new(GetScreenWidth() * 0.5f - BTH_WIDTH * 0.5f, (GetScreenHeight() - 500 - 3 * BTN_HEIGHT) * 0.25f + 300, BTH_WIDTH, BTN_HEIGHT);
        createJoin = new(GetScreenWidth() * 0.5f - BTH_WIDTH * 0.5f, 2 * (GetScreenHeight() - 500 - 3 * BTN_HEIGHT) * 0.25f + BTN_HEIGHT + 300, BTH_WIDTH, BTN_HEIGHT); 
        create = new(GetScreenWidth() * 0.5f - BTH_WIDTH * 0.5f, 3 * (GetScreenHeight() - 500 - 3 * BTN_HEIGHT) * 0.25f + 2 * BTN_HEIGHT + 300, BTH_WIDTH, BTN_HEIGHT);

        while (!WindowShouldClose())
        {
            ClearBackground(Color.RayWhite);
            switch (currentPage)
            {
                case LauncherPage.Main:
                    DrawMainWindow();
                    break;
                
                case LauncherPage.Join:
                    JoinWindow();
                    break;
                
                case LauncherPage.Create:
                    StartCreate();
                    break;
                
                case LauncherPage.CreateJoin:
                    StartCreateJoin();
                    break;
                
                default:
                    CloseWindow();
                    break;
            }
        }
    }
    
    /// <summary>
    /// Affichage de la fenêtre
    /// </summary>
    private static void DrawMainWindow()
    {
        CheckButton();
        
        BeginDrawing();
        
        DrawRectangleRec(join, Color.DarkGray);
        DrawRectangleRec(createJoin, Color.DarkGray);
        DrawRectangleRec(create, Color.DarkGray);

        // Dessiner les Texts
        DrawText(NOM_DU_JEU, GetScreenWidth() / 2 - MeasureText(NOM_DU_JEU, FONT_SIZE * 8) / 2, 100, FONT_SIZE * 8, Color.Black);
        DrawText(JOIN_TEXT, Convert.ToInt32(join.X + join.Width/2 - MeasureText(JOIN_TEXT, FONT_SIZE)* 0.5f), Convert.ToInt32(join.Y + join.Height * 0.5f - FONT_SIZE * 0.5f) ,FONT_SIZE, Color.White);
        DrawText(CREATE_JOIN_TEXT, Convert.ToInt32(createJoin.X + createJoin.Width * 0.5f - MeasureText(CREATE_JOIN_TEXT, FONT_SIZE) * 0.5f), Convert.ToInt32(createJoin.Y + createJoin.Height * 0.5f - FONT_SIZE * 0.5f), FONT_SIZE, Color.White);
        DrawText(CREATE_TEXT, Convert.ToInt32(create.X + create.Width * 0.5f - MeasureText(CREATE_TEXT, FONT_SIZE) * 0.5f), Convert.ToInt32(create.Y + create.Height * 0.5f - FONT_SIZE * 0.5f), FONT_SIZE, Color.White);
        
        EndDrawing();
    }

    /// <summary>
    /// Regarde si les boutton sont pressé et fait l'action si c'est le cas
    /// </summary>
    private static void CheckButton()
    {
        if (CheckCollisionPointRec(GetMousePosition(), join) || CheckCollisionPointRec(GetMousePosition(), createJoin) || CheckCollisionPointRec(GetMousePosition(), create))
        {
            SetMouseCursor(MouseCursor.PointingHand);
            
            if (CheckCollisionPointRec(GetMousePosition(), join) && IsMouseButtonPressed(MouseButton.Left))
                currentPage = LauncherPage.Join;
            
            else if (CheckCollisionPointRec(GetMousePosition(), createJoin) && IsMouseButtonPressed(MouseButton.Left))
                currentPage = LauncherPage.CreateJoin;
            
            else if (CheckCollisionPointRec(GetMousePosition(), create) && IsMouseButtonPressed(MouseButton.Left))
                currentPage = LauncherPage.Create;
        }
        else
            SetMouseCursor(MouseCursor.Default);
    }

    /// <summary>
    /// Affiche la fenêtre pour rejoindre
    /// </summary>
    private static void JoinWindow()
    {
        string ip = "";
        int letterCount = 0;

        Rectangle textBox = new Rectangle(GetScreenWidth() * 0.5f - 500, GetScreenHeight() * 0.5f - 25, 1000, 50);

        int framesCounter = 0;

        while (!IsKeyPressed(KeyboardKey.Enter) && !WindowShouldClose())
        {
            int key = GetCharPressed();
            
            while (key > 0 && !WindowShouldClose())
            {
                // Seuls les caractères dans la plage [32..125] sont acceptés
                if (key >= 32 && key <= 125 && letterCount < MAX_INPUT_CHARS)
                {
                    ip += (char)key; // Ajouter le caractère à la chaîne
                    letterCount++;
                }

                key = GetCharPressed();  // Vérifie le prochain caractère dans la file d'attente
            }
            // Si la touche BACKSPACE est pressée
            if (IsKeyPressed(KeyboardKey.Backspace))
            {
                if (letterCount > 0)
                {
                    letterCount--;
                    ip = ip.Substring(0, letterCount);  // Supprimer le dernier caractère
                }
            }
            else if (CheckCollisionPointRec(GetMousePosition(), textBox))
                SetMouseCursor(MouseCursor.IBeam);
            else
                SetMouseCursor(MouseCursor.Default);


            framesCounter++;
            
            BeginDrawing();
            ClearBackground(Color.RayWhite);
            DrawText(
                "ENTREZ L'ADRESSE DU SERVEUR", 
                GetScreenWidth() / 2 - MeasureText("ENTREZ L'ADRESSE DU SERVEUR",
                    20) / 2, 
                GetScreenHeight() / 2 - 100 /2,
                20, 
                Color.Gray
            );

            DrawRectangleRec(textBox, Color.LightGray);

            DrawRectangleLines((int)textBox.X, (int)textBox.Y, (int)textBox.Width, (int)textBox.Height, Color.Red);


            DrawText(ip, (int)textBox.X + 5, (int)textBox.Y + 8, 40, Color.Maroon);

            DrawText(
                $"INPUT CHARS: {letterCount}/{MAX_INPUT_CHARS}", 
                GetScreenWidth() / 2 - MeasureText($"INPUT CHARS: {letterCount}/{MAX_INPUT_CHARS}", 20) / 2, 
                GetScreenHeight() / 2 - 20 / 2 + 50, 
                20, 
                Color.DarkGray
            );


            if (letterCount < MAX_INPUT_CHARS)
            {
                // Affiche un soulignement clignotant lorsque le texte peut encore être saisi
                if ((framesCounter / 20) % 2 == 0)
                    DrawText("_", (int)textBox.X + 8 + MeasureText(ip, 40), (int)textBox.Y + 12, 40, Color.Maroon);
            }
            else
            {
                DrawText("Press BACKSPACE to delete chars...", GetScreenWidth() / 2 - MeasureText("Press BACKSPACE to delete chars...", 20) / 2, GetScreenHeight()/2 - 500, 20, Color.Gray);
            }


            EndDrawing();
            if (IsKeyPressed(KeyboardKey.Enter))
            {
                StartJoin(ip, Program.DEFAULT_PORT);
            }

        }
    }

    /// <summary>
    /// Démarre le client
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    private static void StartJoin(string ip, int port)
    {
        try
        {
            Client.StartClient(ip, Program.DEFAULT_PORT);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    /// <summary>
    /// Démarre le serveur
    /// </summary>
    private static void StartCreate()
    {
        Server.StartServer(Program.DEFAULT_PORT);
        CloseWindow();
    }
    
    /// <summary>
    /// Démarre le client et le serveur
    /// </summary>
    private static void StartCreateJoin()
    {
        Server.StartServer(Program.DEFAULT_PORT);
        Client.StartClient("127.0.0.1", Program.DEFAULT_PORT);
    }
}