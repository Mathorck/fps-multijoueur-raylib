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
        TEXT_SINGUP = "Créer",
        TEXT_TOP = "Bienvenue !",
        TEXT_REMEMBER_ME = "Remember me";

    private const int MAX_INPUT_CHARS_USERNAME = 25, MAX_INPUT_CHARS_PASSWORD = 25;
    public static User? Player;
    public static bool IsLoggedIn;

    /// <summary>
    ///     Démarre l'interface de connexion et d'inscription.
    /// </summary>
    public static void Start()
    {
        InitWindow(GetScreenWidth(), GetScreenHeight(), "Login");
        SetTargetFPS(60);

        // Définition des constantes et variables

        int INPUT_RECTANGLE_WIDTH = (int)(GetScreenWidth() / 1.5),
            INPUT_RECTANGLE_HEIGHT = GetScreenHeight() / 10,
            SPACE_BETWEEN = GetScreenHeight() / 30,
            FONT_SIZE = GetScreenHeight() / 30;

        string username = "", password = "";
        bool close = false, isRememberMe = false;

        // Définition des rectangles pour les champs de saisie
        Rectangle recTextBoxUsername = new(GetScreenWidth() * 0.5f - INPUT_RECTANGLE_WIDTH * 0.5f,
            GetScreenHeight() / 3,
            INPUT_RECTANGLE_WIDTH, INPUT_RECTANGLE_HEIGHT);
        Rectangle recTextBoxPassword = new(GetScreenWidth() * 0.5f - INPUT_RECTANGLE_WIDTH * 0.5f,
            recTextBoxUsername.Y + INPUT_RECTANGLE_HEIGHT + FONT_SIZE, INPUT_RECTANGLE_WIDTH, INPUT_RECTANGLE_HEIGHT);
        Rectangle recSignin = new(GetScreenWidth() * 0.5f - INPUT_RECTANGLE_WIDTH * 0.5f,
            (int)(recTextBoxPassword.Y + INPUT_RECTANGLE_HEIGHT + FONT_SIZE), INPUT_RECTANGLE_WIDTH,
            INPUT_RECTANGLE_HEIGHT);
        Rectangle recSingup = new(GetScreenWidth() * 0.5f - INPUT_RECTANGLE_WIDTH * 0.5f,
            (int)(recSignin.Y + INPUT_RECTANGLE_HEIGHT + FONT_SIZE), INPUT_RECTANGLE_WIDTH, INPUT_RECTANGLE_HEIGHT);
        Rectangle recRememberMe = new(GetScreenWidth() * 0.5f - INPUT_RECTANGLE_WIDTH * 0.5f,
            (int)(recSingup.Y + INPUT_RECTANGLE_HEIGHT + FONT_SIZE), INPUT_RECTANGLE_HEIGHT, INPUT_RECTANGLE_HEIGHT);

        // Initialisation des champs de saisie et des boutons
        InputButton textBoxUsername = new(recTextBoxUsername, TEXT_USERNAME, MAX_INPUT_CHARS_USERNAME, Color.LightGray,
            Color.Black, Color.Black, Color.LightGray, Color.Black, Color.Black, false);
        InputButton textBoxPassword = new(recTextBoxPassword, TEXT_PASSWORD, MAX_INPUT_CHARS_PASSWORD, Color.LightGray,
            Color.Black, Color.Black, Color.LightGray, Color.Black, Color.Black, true);
        List<InputButton> inputButtons = new() { textBoxUsername, textBoxPassword };

        Boutton Signin = new(recSignin, TEXT_SIGNIN, Color.Blue, Color.White, Color.DarkBlue, Color.White);
        Boutton Singup = new(recSingup, TEXT_SINGUP, Color.Blue, Color.White, Color.DarkBlue, Color.White);
        List<Boutton> Bouttons = new() { Signin, Singup };

        int framesCounter = 0;
        string erreur = "";

        // Charger les informations de connexion si elles existent
        LoadRememberedLoginInfo(ref textBoxUsername, ref textBoxPassword, ref isRememberMe);

        while (!IsKeyPressed(KeyboardKey.Enter) && !WindowShouldClose() && !close && !IsLoggedIn)
        {
            int key = GetCharPressed();
            bool isHoverInput = false, isHoverButton = false;

            // Vérification des clics sur les boutons et champs de saisie
            HandleUserInput(ref erreur, ref isHoverButton, ref isHoverInput, key, textBoxUsername, textBoxPassword,
                Signin, Singup, recRememberMe, ref isRememberMe);

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // Dessiner les éléments de l'interface
            DrawUIElements(inputButtons, Bouttons, recRememberMe, TEXT_REMEMBER_ME, erreur, isRememberMe, FONT_SIZE);

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
    private static void HandleUserInput(ref string erreur, ref bool isHoverButton, ref bool isHoverInput, int key,
        InputButton textBoxUsername, InputButton textBoxPassword, Boutton Signin, Boutton Singup,
        Rectangle recRememberMe, ref bool isRememberMe)
    {
        if (Signin.CheckCollision(GetMousePosition()))
        {
            isHoverButton = true;
            if (IsMouseButtonReleased(MouseButton.Left))
            {
                erreur = verifyInputConnect(textBoxUsername.inputText, textBoxPassword.inputText);
                if (erreur == "" && Connect(textBoxUsername.inputText, textBoxPassword.inputText, isRememberMe))
                    CloseWindow();
            }
        }
        else if (Singup.CheckCollision(GetMousePosition()))
        {
            isHoverButton = true;
            if (IsMouseButtonReleased(MouseButton.Left))
            {
                erreur = verifyInputCreate(textBoxUsername.inputText, textBoxPassword.inputText);
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
    private static void DrawUIElements(List<InputButton> inputButtons, List<Boutton> Bouttons, Rectangle recRememberMe,
        string TEXT_REMEMBER_ME, string erreur, bool isRememberMe, int FONT_SIZE)
    {
        DrawText(TEXT_TOP, (GetScreenWidth() - MeasureText(TEXT_TOP, 200)) / 2, 100, 200, Color.Black);

        foreach (Boutton b in Bouttons) b.Draw();

        if (isRememberMe)
            DrawRectangleRec(recRememberMe, Color.Blue);
        DrawRectangleLinesEx(recRememberMe, 1, Color.Black);

        DrawText(TEXT_REMEMBER_ME, (int)(recRememberMe.X + recRememberMe.Width + FONT_SIZE),
            (int)(recRememberMe.Y + recRememberMe.Height / 2 - FONT_SIZE / 2), FONT_SIZE, Color.Black);
        DrawText(erreur,
            (int)(recRememberMe.X + recRememberMe.Width + MeasureText(TEXT_REMEMBER_ME, FONT_SIZE) + 2 * FONT_SIZE),
            (int)(recRememberMe.Y + recRememberMe.Width / 2 - FONT_SIZE / 2), FONT_SIZE, Color.Red);
    }

    /// <summary>
    ///     Vérifie les entrées pour l'inscription (pseudo et mot de passe).
    /// </summary>
    private static string verifyInputCreate(string username, string password)
    {
        string s = verifyInput(username, password);
        if (!string.IsNullOrEmpty(s))
            return s;

        Game.GameElement.Player.Nom = username;

        using (MySqlConnection conn = new(Launcher.connectionString))
        {
            conn.Open();
            MySqlCommand cmd = new($"SELECT Pseudo FROM user WHERE Pseudo = \"{username}\"", conn);
            using (MySqlDataReader? reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                    return "Ce pseudo est déjà utilisé par un autre joueur";
                return "";
            }
        }
    }

    /// <summary>
    ///     Vérifie les entrées pour la connexion.
    /// </summary>
    private static string verifyInputConnect(string username, string password)
    {
        string s = verifyInput(username, password);
        if (!string.IsNullOrEmpty(s))
            return s;

        Game.GameElement.Player.Nom = username;

        using (MySqlConnection conn = new(Launcher.connectionString))
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
    }

    /// <summary>
    ///     Vérifie si les entrées (pseudo et mot de passe) sont valides.
    /// </summary>
    private static string verifyInput(string username, string password)
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
    private static bool Create(string Username, string Password, bool rememberme)
    {
        try
        {
            using (MySqlConnection conn = new(Launcher.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new($"INSERT INTO user (Pseudo, MotDePasse) VALUE (\"{Username}\", \"{Password}\")",
                    conn);
                cmd.ExecuteNonQuery();
                return Connect(Username, Password, rememberme);
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
    private static bool Connect(string Username, string Password, bool rememberme)
    {
        try
        {
            using (MySqlConnection conn = new(Launcher.connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new(
                    $"SELECT Id, Pseudo, MotDePasse FROM user WHERE Pseudo = \"{Username}\" AND MotDePasse = \"{Password}\";",
                    conn
                );
                using (MySqlDataAdapter adapter = new(cmd))
                {
                    DataTable dt = new();
                    adapter.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        StreamWriter sw = new("LoginInfo/LoginInfo.txt");
                        if (rememberme)
                        {
                            sw.WriteLine(Username);
                            sw.WriteLine(Password);
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