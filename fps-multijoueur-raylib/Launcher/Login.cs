using System.Data;
using DeadOpsArcade3D.Launcher.LauncherElement;
using DeadOpsArcade3D.Multiplayer;
using MySql.Data.MySqlClient;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Launcher;

/// <summary>
///     Classe gérant la connexion et l'inscription de l'utilisateur dans le jeu.
/// </summary>
public class Login
{
    private const string TEXT_USERNAME = "Pseudo",
        TEXT_PASSWORD = "Mot de passe",
        TEXT_SIGNIN = "CONNECTER",
        TEXT_SINGUP = "CRÉER",
        TEXT_TOP = "Bienvenue !",
        TEXT_REMEMBER_ME = "Remember me";

    private const int MAX_INPUT_CHARS_USERNAME = 25, MAX_INPUT_CHARS_PASSWORD = 25;
    public static User? Player;
    public static bool IsLoggedIn;
    
    
    private static string erreur = "";

    /// <summary>
    ///     Démarre l'interface de connexion et d'inscription.
    /// </summary>
    public static void Start()
    {
        InitWindow(GetScreenWidth(), GetScreenHeight(), "Login");
        SetTargetFPS(60);

        Launcher.LoadConnexionString();

        // Définition des constantes et variables

        int rectangleWidth = (int)(GetScreenWidth() / 1.5),
            rectangleHeight = GetScreenHeight() / 10,
            fontSize = GetScreenHeight() / 30;

        bool close = false, isRememberMe = false;

        // Définition des rectangles pour les champs de saisie
        Rectangle recTextBoxUsername = new(GetScreenWidth() * 0.5f - rectangleWidth * 0.5f,
            GetScreenHeight() * 0.3f,
            rectangleWidth, rectangleHeight);
        Rectangle recTextBoxPassword = new(GetScreenWidth() * 0.5f - rectangleWidth * 0.5f,
            recTextBoxUsername.Y + rectangleHeight + fontSize, rectangleWidth, rectangleHeight);
        Rectangle recSignin = new(GetScreenWidth() * 0.5f - rectangleWidth * 0.5f,
            (int)(recTextBoxPassword.Y + rectangleHeight + fontSize), rectangleWidth,
            rectangleHeight);
        Rectangle recSingup = new(GetScreenWidth() * 0.5f - rectangleWidth * 0.5f,
            (int)(recSignin.Y + rectangleHeight + fontSize), rectangleWidth, rectangleHeight);
        Rectangle recRememberMe = new(GetScreenWidth() * 0.5f - rectangleWidth * 0.5f,
            (int)(recSingup.Y + rectangleHeight + fontSize), rectangleHeight, rectangleHeight);

        // Initialisation des champs de saisie et des boutons
        InputButton textBoxUsername = new(recTextBoxUsername, TEXT_USERNAME, MAX_INPUT_CHARS_USERNAME, Color.LightGray,
            Color.Black, Color.Black, Color.LightGray, Color.Black, Color.Black, false);
        InputButton textBoxPassword = new(recTextBoxPassword, TEXT_PASSWORD, MAX_INPUT_CHARS_PASSWORD, Color.LightGray,
            Color.Black, Color.Black, Color.LightGray, Color.Black, Color.Black, true);
        List<InputButton> inputButtons = new() { textBoxUsername, textBoxPassword };

        Boutton signin = new(recSignin, TEXT_SIGNIN, Color.Blue, Color.White, Color.DarkBlue, Color.White);
        Boutton singup = new(recSingup, TEXT_SINGUP, Color.Blue, Color.White, Color.DarkBlue, Color.White);
        List<Boutton> bouttons = new() { signin, singup };

        

        // Charger les informations de connexion si elles existent
        LoadRememberedLoginInfo(ref textBoxUsername, ref textBoxPassword, ref isRememberMe);

        while (!IsKeyPressed(KeyboardKey.Enter) && !WindowShouldClose() && !close && !IsLoggedIn)
        {
            int key = GetCharPressed();
            bool isHoverInput = false, isHoverButton = false;

            // Vérification des clics sur les boutons et champs de saisie
            HandleUserInput(ref isHoverButton, ref isHoverInput, key, textBoxUsername, textBoxPassword,
                signin, singup, recRememberMe, ref isRememberMe);

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // Dessiner les éléments de l'interface
            DrawUiElements(inputButtons, bouttons, recRememberMe, TEXT_REMEMBER_ME, isRememberMe, fontSize);

            EndDrawing();
        }
    }

    /// <summary>
    ///     Charge les informations de connexion mémorisées dans le fichier LoginInfo.txt.
    /// </summary>
    private static void LoadRememberedLoginInfo(ref InputButton textBoxUsername, ref InputButton textBoxPassword,
        ref bool isRememberMe)
    {
        using StreamReader sr = new("LoginInfo/LoginInfo.txt");
        string? rememberUsername = sr.ReadLine();
        string? rememberPassword = sr.ReadLine();
        if (rememberUsername != null && rememberPassword != null)
        {
            textBoxUsername.inputText = rememberUsername;
            textBoxPassword.inputText = rememberPassword;
            isRememberMe = true;
        }
    }

    /// <summary>
    ///     Gère les entrées de l'utilisateur pour les champs de texte, les boutons et la case "Remember Me".
    /// </summary>
    private static void HandleUserInput(ref bool isHoverButton, ref bool isHoverInput, int key,
        InputButton textBoxUsername, InputButton textBoxPassword, Boutton signin, Boutton singup,
        Rectangle recRememberMe, ref bool isRememberMe)
    {
        if (signin.CheckCollision(GetMousePosition()))
        {
            isHoverButton = true;
            if (IsMouseButtonReleased(MouseButton.Left))
            {
                erreur = VerifyInputConnect(textBoxUsername.inputText, textBoxPassword.inputText);
                if (erreur == "" && Connect(textBoxUsername.inputText, textBoxPassword.inputText, isRememberMe))
                    CloseWindow();
            }
        }
        else if (singup.CheckCollision(GetMousePosition()))
        {
            isHoverButton = true;
            if (IsMouseButtonReleased(MouseButton.Left))
            {
                erreur = VerifyInputCreate(textBoxUsername.inputText, textBoxPassword.inputText);
                if (erreur == "" && Create(textBoxUsername.inputText, textBoxPassword.inputText, isRememberMe))
                    CloseWindow();
            }
        }
        else if (CheckCollisionPointRec(GetMousePosition(), recRememberMe))
        {
            isHoverButton = true;
            if (IsMouseButtonReleased(MouseButton.Left))
                isRememberMe = !isRememberMe;
        }

        if (isHoverInput)
            SetMouseCursor(MouseCursor.IBeam);
        else if (isHoverButton)
            SetMouseCursor(MouseCursor.PointingHand);
        else
            SetMouseCursor(MouseCursor.Default);

        // Saisie du texte dans les champs
        foreach (InputButton b in new[] { textBoxUsername, textBoxPassword })
        {
            if (b.CheckCollision(GetMousePosition()))
            {
                isHoverInput = true;
                if (key > 0)
                    b.AddChar((char)key);
            }

            if (IsKeyPressed(KeyboardKey.Backspace))
                b.RemoveChar();

            b.Draw();
        }
    }

    /// <summary>
    ///     Dessine les éléments de l'interface utilisateur.
    /// </summary>
    private static void DrawUiElements(List<InputButton> inputButtons, List<Boutton> bouttons, Rectangle recRememberMe,
        string rmberMeTxt, bool isRememberMe, int fontSize)
    {
        DrawText(TEXT_TOP, (GetScreenWidth() - MeasureText(TEXT_TOP, 200)) / 2, 100, 200, Color.Black);

        foreach (Boutton b in bouttons) b.Draw();

        if (isRememberMe)
            DrawRectangleRec(recRememberMe, Color.Blue);
        DrawRectangleLinesEx(recRememberMe, 1, Color.Black);

        DrawText(rmberMeTxt, (int)(recRememberMe.X + recRememberMe.Width + fontSize),
            (int)(recRememberMe.Y + recRememberMe.Height * 0.5f - fontSize * 0.5f), fontSize, Color.Black);
        DrawText(erreur,
            (int)(recRememberMe.X + recRememberMe.Width + MeasureText(rmberMeTxt, fontSize) + 2 * fontSize),
            (int)(recRememberMe.Y + recRememberMe.Width * 0.5f - fontSize * 0.5f), fontSize, Color.Red);
    }

    /// <summary>
    ///     Vérifie les entrées pour l'inscription (pseudo et mot de passe).
    /// </summary>
    private static string VerifyInputCreate(string username, string password)
    {
        string s = VerifyInput(username, password);
        if (!string.IsNullOrEmpty(s))
            return s;

        Game.GameElement.Player.Nom = username;

        using MySqlConnection conn = new(Launcher.connectionString);
        try
        {
            conn.Open();
            MySqlCommand cmd = new($"SELECT Pseudo FROM user WHERE Pseudo = \"{username}\"", conn);
            using (MySqlDataReader? reader = cmd.ExecuteReader())
            {
                return reader.HasRows ? "Ce pseudo est déjà utilisé par un autre joueur" : "";
            }
        }
        catch (MySqlException ex)
        {
            Client.ConsoleError(ex.ToString());
            erreur = "La connexion au serveur n'a pas pu être établie\n\n\nVeuillez vous référencer au readme";
            return erreur;
        }
    }

    /// <summary>
    ///     Vérifie les entrées pour la connexion.
    /// </summary>
    private static string VerifyInputConnect(string username, string password)
    {
        string s = VerifyInput(username, password);
        if (!string.IsNullOrEmpty(s))
            return s;

        Game.GameElement.Player.Nom = username;

        using MySqlConnection conn = new(Launcher.connectionString);
        try
        {
            conn.Open();
            MySqlCommand cmd =
                new($"SELECT pseudo FROM user WHERE pseudo = \"{username}\" AND MotDePasse = \"{password}\"", conn);
            using (MySqlDataReader? reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                    return "";
                return "Cet nom d'utilisateur et ce mot de passe n'existent pas";
            }
        }
        catch (MySqlException ex)
        {
            Client.ConsoleError(ex.ToString());
            erreur = "La connexion au serveur n'a pas pu être établie\n\n\nVeuillez vous référencer au readme";
            return erreur;
        }
    }

    /// <summary>
    ///     Vérifie si les entrées (pseudo et mot de passe) sont valides.
    /// </summary>
    private static string VerifyInput(string username, string password)
    {
        if (string.IsNullOrEmpty(username))
            return "Le champ \"Pseudo\" ne peut pas être vide";
        if (string.IsNullOrEmpty(password))
            return "Le champ \"Mot de passe\" ne peut pas être vide";
        if (username.Count() < 3)
            return "Le pseudo doit contenir au moins 3 caractères";
        if (password.Count() < 3)
            return "Le mot de passe doit contenir au moins 3 caractères";
        return "";
    }

    /// <summary>
    ///     Crée un nouvel utilisateur dans la base de données.
    /// </summary>
    private static bool Create(string username, string password, bool rememberMe)
    {
        try
        {
            using (MySqlConnection conn = new(Launcher.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new($"INSERT INTO user (Pseudo, MotDePasse) VALUE (\"{username}\", \"{password}\")",
                    conn);
                cmd.ExecuteNonQuery();
                return Connect(username, password, rememberMe);
            }
        }
        catch (Exception ex)
        {
            Client.ConsoleError(ex.ToString());
            return false;
        }
    }

    /// <summary>
    ///     Connecte l'utilisateur à l'application après la vérification dans la base de données.
    /// </summary>
    private static bool Connect(string username, string password, bool rememberMe)
    {
        try
        {
            using (MySqlConnection conn = new(Launcher.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new(
                    $"SELECT Id, Pseudo, MotDePasse FROM user WHERE Pseudo = \"{username}\" AND MotDePasse = \"{password}\";",
                    conn
                );
                using (MySqlDataAdapter adapter = new(cmd))
                {
                    DataTable dt = new();
                    adapter.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        StreamWriter sw = new("LoginInfo/LoginInfo.txt");
                        if (rememberMe)
                        {
                            sw.WriteLine(username);
                            sw.WriteLine(password);
                        }
                        else
                        {
                            sw.Write("");
                        }

                        sw.Close();

                        Player = new User((int)dt.Rows[0]["Id"], (string)dt.Rows[0]["Pseudo"]);
                        IsLoggedIn = true;
                        CloseWindow();
                        Launcher.Init();
                        return true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Client.ConsoleError(ex.ToString());
            return false;
        }

        return false;
    }
}