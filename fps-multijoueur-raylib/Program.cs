using DeadOpsArcade3D.Multiplayer;

using static Raylib_cs.Raylib;
using Raylib_cs;
using DeadOpsArcade3D.Launcher;

namespace DeadOpsArcade3D
{
    class Program
    {
        public const int DEFAULT_PORT = 3855;
        
        static void Main(string[] args)
        {
            //Launcher.Init();
            Login.Start();

#region Commentaires qui font peur a moi /!\ Danger ne pas ouvrir

            

            
            //const int MAX_INPUT_CHARS = 9;

            //// Initialization
            ////--------------------------------------------------------------------------------------
            //const int screenWidth = 800;
            //const int screenHeight = 450;

            //InitWindow(screenWidth, screenHeight, "raylib [text] example - input box");

            //char name[MAX_INPUT_CHARS + 1] = "\0";      // NOTE: One extra space required for null terminator char '\0'
            //int letterCount = 0;

            //Rectangle textBox = (GetScreenWidth() / 2.0f - 100, 180, 225, 50);
            //bool mouseOnText = false;

            //int framesCounter = 0;

            //SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //                                //--------------------------------------------------------------------------------------

            //// Main game loop
            //while (!WindowShouldClose())    // Detect window close button or ESC key
            //{
            //    // Update
            //    //----------------------------------------------------------------------------------
            //    if (CheckCollisionPointRec(GetMousePosition(), textBox)) mouseOnText = true;
            //    else mouseOnText = false;

            //    if (mouseOnText)
            //    {
            //        // Set the window's cursor to the I-Beam
            //        SetMouseCursor(MOUSE_CURSOR_IBEAM);

            //        // Get char pressed (unicode character) on the queue
            //        int key = GetCharPressed();

            //        // Check if more characters have been pressed on the same frame
            //        while (key > 0)
            //        {
            //            // NOTE: Only allow keys in range [32..125]
            //            if ((key >= 32) && (key <= 125) && (letterCount < MAX_INPUT_CHARS))
            //            {
            //                name[letterCount] = (char)key;
            //                name[letterCount + 1] = '\0'; // Add null terminator at the end of the string.
            //                letterCount++;
            //            }

            //            key = GetCharPressed();  // Check next character in the queue
            //        }

            //        if (IsKeyPressed(KEY_BACKSPACE))
            //        {
            //            letterCount--;
            //            if (letterCount < 0) letterCount = 0;
            //            name[letterCount] = '\0';
            //        }
            //    }
            //    else SetMouseCursor(MOUSE_CURSOR_DEFAULT);

            //    if (mouseOnText) framesCounter++;
            //    else framesCounter = 0;
            //    //----------------------------------------------------------------------------------

            //    // Draw
            //    //----------------------------------------------------------------------------------
            //    BeginDrawing();

            //    ClearBackground(RAYWHITE);

            //    DrawText("PLACE MOUSE OVER INPUT BOX!", 240, 140, 20, GRAY);

            //    DrawRectangleRec(textBox, LIGHTGRAY);
            //    if (mouseOnText) DrawRectangleLines((int)textBox.x, (int)textBox.y, (int)textBox.width, (int)textBox.height, RED);
            //    else DrawRectangleLines((int)textBox.x, (int)textBox.y, (int)textBox.width, (int)textBox.height, DARKGRAY);

            //    DrawText(name, (int)textBox.x + 5, (int)textBox.y + 8, 40, MAROON);

            //    DrawText(TextFormat("INPUT CHARS: %i/%i", letterCount, MAX_INPUT_CHARS), 315, 250, 20, DARKGRAY);

            //    if (mouseOnText)
            //    {
            //        if (letterCount < MAX_INPUT_CHARS)
            //        {
            //            // Draw blinking underscore char
            //            if (((framesCounter / 20) % 2) == 0) DrawText("_", (int)textBox.x + 8 + MeasureText(name, 40), (int)textBox.y + 12, 40, MAROON);
            //        }
            //        else DrawText("Press BACKSPACE to delete chars...", 230, 300, 20, GRAY);
            //    }

            //    EndDrawing();
            //    //----------------------------------------------------------------------------------
            //}

            //// De-Initialization
            ////--------------------------------------------------------------------------------------
            //CloseWindow();        // Close window and OpenGL context
            //                      //--------------------------------------------------------------------------------------

            //return 0;


            //// Check if any key is pressed
            //// NOTE: We limit keys check to keys between 32 (KEY_SPACE) and 126
            //bool IsAnyKeyPressed()
            //{
            //    bool keyPressed = false;
            //    int key = GetKeyPressed();

            //    if ((key >= 32) && (key <= 126)) keyPressed = true;

            //    return keyPressed;
            // }





            ///ChatGpt 2
            //    // Déclaration des variables pour l'interface
            //    int port = 3855;
            //    string host = "127.0.0.1";
            //    bool isServer = false; // True si l'option "serveur" est choisie, False pour "client"


            //    // Initialisation de la fenêtre Raylib
            //    Raylib.InitWindow(800, 600, "Serveur/Client Interface");
            //    Raylib.SetTargetFPS(60);

            //    // Variables pour gérer les éléments de l'interface
            //    string portInput = port.ToString();
            //    string hostInput = host;
            //    bool showMessage = false;

            //    // Variables pour gérer le curseur de texte
            //    string currentInput = "";
            //    bool isHostInputActive = true; // Flag pour savoir si l'on édite l'adresse ou le port

            //    // Boucle principale
            //    while (!Raylib.WindowShouldClose())
            //    {
            //        // Détection des événements
            //        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
            //        {
            //            // Si le bouton "Lancer le serveur" est cliqué
            //            if (isServer)
            //            {
            //                int parsedPort = int.TryParse(portInput, out int result) ? result : 3855;
            //                Server.StartServer(parsedPort); // Appelez ici votre méthode de démarrage du serveur
            //                showMessage = true;
            //            }
            //            // Si le bouton "Lancer le client" est cliqué
            //            else
            //            {
            //                string parsedHost = string.IsNullOrWhiteSpace(hostInput) ? "127.0.0.1" : hostInput;
            //                int parsedPort = int.TryParse(portInput, out int result) ? result : 3855;
            //                Client.StartClient(parsedHost, parsedPort); // Appelez ici votre méthode de démarrage du client
            //                showMessage = true;
            //            }
            //        }

            //        // Détection du clic pour sélectionner "Serveur" ou "Client"
            //        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            //        {
            //            Rectangle serverButton = new Rectangle(300, 100, 200, 50);
            //            Rectangle clientButton = new Rectangle(300, 200, 200, 50);

            //            if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), serverButton))
            //            {
            //                isServer = true;
            //                showMessage = false; // Reset the message when switching modes
            //                isHostInputActive = true; // Revenir sur l'input pour l'adresse
            //            }
            //            else if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), clientButton))
            //            {
            //                isServer = false;
            //                showMessage = false; // Reset the message when switching modes
            //                isHostInputActive = true; // Revenir sur l'input pour l'adresse
            //            }
            //        }

            //        // Gestion de l'input clavier pour les champs de texte
            //        if (isHostInputActive)
            //        {
            //            // Saisie de l'adresse du serveur
            //            currentInput = HandleTextInput(currentInput);
            //            hostInput = currentInput;
            //        }
            //        else
            //        {
            //            // Saisie du port
            //            currentInput = HandleTextInput(currentInput);
            //            portInput = currentInput;
            //        }

            //        // Commencez à dessiner
            //        Raylib.BeginDrawing();
            //        Raylib.ClearBackground(Color.DarkGray);

            //        // Affichage des options Serveur / Client
            //        Raylib.DrawText("Choisir une option:", 320, 50, 20, Color.White);
            //        if (isServer)
            //            Raylib.DrawText("Serveur sélectionné", 320, 70, 20, Color.Green);
            //        else
            //            Raylib.DrawText("Client sélectionné", 320, 70, 20, Color.Red);

            //        // Dessiner les boutons
            //        Rectangle serverButtonRect = new Rectangle(300, 100, 200, 50);
            //        Rectangle clientButtonRect = new Rectangle(300, 200, 200, 50);
            //        Raylib.DrawRectangleRec(serverButtonRect, Color.DarkBlue);
            //        Raylib.DrawRectangleRec(clientButtonRect, Color.DarkBlue);
            //        Raylib.DrawText("Lancer le serveur", 340, 115, 20, Color.White);
            //        Raylib.DrawText("Lancer le client", 340, 215, 20, Color.White);

            //        // Affichage des entrées utilisateur
            //        Raylib.DrawText("Adresse du serveur:", 320, 300, 20, Color.White);
            //        Raylib.DrawText(hostInput, 320, 330, 20, Color.LightGray);

            //        Raylib.DrawText("Port:", 320, 370, 20, Color.White);
            //        Raylib.DrawText(portInput, 320, 400, 20, Color.LightGray);

            //        // Afficher un message de succès
            //        if (showMessage)
            //        {
            //            Raylib.DrawText("Serveur/Client démarré!", 320, 460, 20, Color.Yellow);
            //        }

            //        // Fin de dessin
            //        Raylib.EndDrawing();
            //    }

            //    // Fermeture de la fenêtre Raylib
            //    Raylib.CloseWindow();



            //// Fonction pour gérer l'input de texte (ajoute et supprime des caractères)
            //static string HandleTextInput(string currentInput)
            //{
            //    if (Raylib.IsKeyPressed(KeyboardKey.Backspace) && currentInput.Length > 0)
            //    {
            //        currentInput = currentInput.Substring(0, currentInput.Length - 1);
            //    }
            //    else
            //    {
            //        // Ajouter un caractère lorsque l'utilisateur appuie sur une touche
            //        for (int key = (int)KeyboardKey.A; key <= (int)KeyboardKey.Z; key++)
            //        {
            //            if (Raylib.IsKeyPressed((KeyboardKey)key))
            //            {
            //                currentInput += (char)key;
            //            }
            //        }
            //        for (int key =  Convert.ToInt32(KeyboardKey.Kp0); key <= Convert.ToInt32(KeyboardKey.Kp9); key++)
            //        {
            //            if (Raylib.IsKeyPressed((KeyboardKey)key))
            //            {
            //                currentInput += (char)key;
            //            }
            //        }
            //    }

            //    return currentInput;
            //}
            
            //1
            //while (!WindowShouldClose())
            //{
            //    Rectangle ServerBtn = new Rectangle(0.0f, 50.0f, 500.0f, 150.0f);
            //    Rectangle JoinBtn = new Rectangle(0.0f, 200.0f, 500.0f, 150.0f);
            //    Rectangle ServerJoinBtn = new Rectangle(0.0f, 350.0f, 500.0f, 150.0f);

            //    BeginDrawing();
            //    ClearBackground(Color.White);
            //    DrawRectangleRounded(ServerBtn, 0.5f, 2, Color.Black);
            //    DrawRectangleRounded(JoinBtn, 0.5f, 2, Color.Black);
            //    DrawRectangleRounded(ServerJoinBtn, 0.5f, 2, Color.Black);
            //    EndDrawing();
            //}





            //ChatGpt1

            //static void Main()
            //{
            //    // Déclaration des variables pour l'interface
            //    int port = 3855;
            //    string host = "127.0.0.1";
            //    bool isServer = false; // True si l'option "serveur" est choisie, False pour "client"



            //    // Initialisation de la fenêtre Raylib
            //    Raylib.InitWindow(800, 600, "Serveur/Client Interface");
            //    Raylib.SetTargetFPS(60);

            //    // Variables pour gérer les éléments de l'interface
            //    string portInput = port.ToString();
            //    string hostInput = host;
            //    bool showMessage = false;

            //    // Boucle principale
            //    while (!Raylib.WindowShouldClose())
            //    {
            //        // Détection des événements
            //        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
            //        {
            //            // Si le bouton "Lancer le serveur" est cliqué
            //            if (isServer)
            //            {
            //                int parsedPort = int.TryParse(portInput, out int result) ? result : 3855;
            //                Server.StartServer(parsedPort); // Appelez ici votre méthode de démarrage du serveur
            //                showMessage = true;
            //            }
            //            // Si le bouton "Lancer le client" est cliqué
            //            else
            //            {
            //                string parsedHost = string.IsNullOrWhiteSpace(hostInput) ? "127.0.0.1" : hostInput;
            //                int parsedPort = int.TryParse(portInput, out int result) ? result : 3855;
            //                Client.StartClient(parsedHost, parsedPort); // Appelez ici votre méthode de démarrage du client
            //                showMessage = true;
            //            }
            //        }

            //        // Détection du clic pour sélectionner "Serveur" ou "Client"
            //        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            //        {
            //            Rectangle serverButton = new Rectangle(300, 100, 200, 50);
            //            Rectangle clientButton = new Rectangle(300, 200, 200, 50);

            //            if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), serverButton))
            //            {
            //                isServer = true;
            //                showMessage = false; // Reset the message when switching modes
            //            }
            //            else if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), clientButton))
            //            {
            //                isServer = false;
            //                showMessage = false; // Reset the message when switching modes
            //            }
            //        }

            //        // Commencez à dessiner
            //        Raylib.BeginDrawing();
            //        Raylib.ClearBackground(Color.DarkGray);

            //        // Affichage des options Serveur / Client
            //        Raylib.DrawText("Choisir une option:", 320, 50, 20, Color.White);
            //        if (isServer)
            //            Raylib.DrawText("Serveur sélectionné", 320, 70, 20, Color.Green);
            //        else
            //            Raylib.DrawText("Client sélectionné", 320, 70, 20, Color.Red);

            //        // Dessiner les boutons
            //        Rectangle serverButtonRect = new Rectangle(300, 100, 200, 50);
            //        Rectangle clientButtonRect = new Rectangle(300, 200, 200, 50);
            //        Raylib.DrawRectangleRec(serverButtonRect, Color.DarkBlue);
            //        Raylib.DrawRectangleRec(clientButtonRect, Color.DarkBlue);
            //        Raylib.DrawText("Lancer le serveur", 340, 115, 20, Color.White);
            //        Raylib.DrawText("Lancer le client", 340, 215, 20, Color.White);

            //        // Affichage des entrées utilisateur
            //        Raylib.DrawText("Adresse du serveur:", 320, 300, 20, Color.White);
            //        hostInput = Raylib.GuiTextBox(new Rectangle(320, 330, 200, 30), hostInput, 255, Color.LightGray);

            //        Raylib.DrawText("Port:", 320, 370, 20, Color.White);
            //        portInput = Raylib.GuiTextBox(new Rectangle(320, 400, 200, 30), portInput, 255, Color.LightGray);

            //        // Afficher un message de succès
            //        if (showMessage)
            //        {
            //            Raylib.DrawText("Serveur/Client démarré!", 320, 460, 20, Color.Yellow);
            //        }

            //        // Fin de dessin
            //        Raylib.EndDrawing();
            //    }

            //    // Fermeture de la fenêtre Raylib
            //    Raylib.CloseWindow();
            //}





            //Default
            //Console.WriteLine("1. Lancer le serveur\n2. Lancer le client");
            //string? choice = Console.ReadLine();

            //if (choice == "1")
            //{
            //    Console.Write("Port du serveur (appuyez sur Entrée pour utiliser le port par défaut 3855): ");
            //    string? portInput = Console.ReadLine();
            //    int port = string.IsNullOrWhiteSpace(portInput) ? 3855 : int.Parse(portInput);
            //    Server.StartServer(port);
            //}
            //else if (choice == "2")
            //{
            //    Console.Write("Adresse du serveur: ");
            //    string? hostInput = Console.ReadLine();
            //    string host = string.IsNullOrWhiteSpace(hostInput) ? "127.0.0.1" : hostInput;
            //    Console.Write("Port (appuyez sur Entrée pour utiliser le port par défaut 3855): ");
            //    string? portInput = Console.ReadLine();
            //    int port = string.IsNullOrWhiteSpace(portInput) ? 3855 : int.Parse(portInput);
            //    Client.StartClient(host, port);
            //}
            //else
            //{
            //    int port = 3855;
            //    string host = "127.0.0.1";
            //    Server.StartServer(port);
            //    Client.StartClient(host, port);
            //}
#endregion 
        }
    }

}
