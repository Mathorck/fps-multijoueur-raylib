using System.Net;
using System.Net.Sockets;
using DeadOpsArcade3D.Launcher.LauncherElement;
using DeadOpsArcade3D.Multiplayer;
using MySql.Data.MySqlClient;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Launcher;

public class Launcher
{
    #region Boutons

    private static Rectangle recBack = new(0, 0, GetScreenWidth() / 15, GetScreenHeight() / 15);

    #endregion

    /// <summary>
    ///     Chaîne de connexion au serveur MySQL
    /// </summary>
    public static string connectionString = "Server=127.0.0.1;Database=raylib;Uid=root;Pwd=;";

    private static LauncherPage currentPage = LauncherPage.Play;

    public static int FONT_SIZE = GetScreenWidth() / 50, NB_SERVER_AFFICHER = 10;

    private static readonly float BTN_HEBERGER_WIDTH = CalculateButtonWidth(TEXT_HEBERGER);

    private static float BTN_HEBERGER_HEIGHT = (int)(FONT_SIZE * 1.5),
        BTN_PLAY_WIDTH = CalculateButtonWidth(TEXT_PLAY);

    private static readonly float BTN_PLAY_HEIGHT = FONT_SIZE * 2;

    public static int TopMenuHeight = TopButtons(TEXT_PLAY);
    public static int btnServerWidth = GetScreenWidth() / 2, btnServerHeight = (int)(FONT_SIZE * 1.5);
    public static int spaceBetweenServer = GetScreenHeight() / 75;

    /// <summary>
    ///     Définitions des boutons pour l'interface utilisateur
    /// </summary>
    private static readonly Rectangle btnPlay = CreateButton(TEXT_PLAY);

    private static readonly Rectangle btnHeberger = CreateButton(TEXT_HEBERGER, btnPlay.X - BTN_HEBERGER_WIDTH);
    private static List<ServerObject> servers = new();

    /// <summary>
    ///     Initialise le launcher
    /// </summary>
    public static void Init()
    {
        InitWindow(GetScreenWidth(), GetScreenHeight(), "Launcher");
        SetTargetFPS(60);

        bool close = false;
        bool joinServer = false;
        string joinIp = "127.0.0.1";
        string ip = "";
        servers = new List<ServerObject>();

        Task.Run(GetServers);

        while (!WindowShouldClose() && !close)
        {
            ClearBackground(Color.RayWhite);
            switch (currentPage)
            {
                case LauncherPage.Heberger:
                    Heberger(ref ip, ref close, ref joinServer);
                    break;

                case LauncherPage.Play:
                    Play(ref joinIp, ref close, ref joinServer);
                    break;

                default:
                    close = true;
                    break;
            }
        }

        CloseWindow();

        if (joinServer)
            Client.StartClient(joinIp, Program.DEFAULT_PORT);
    }

    /// <summary>
    ///     Dessine les boutons du haut en fonction de la page active
    /// </summary>
    /// <param name="BtnActive">Texte du bouton actif</param>
    /// <returns>Hauteur du menu supérieur</returns>
    private static int TopButtons(string BtnActive)
    {
        if (BtnActive == TEXT_PLAY)
        {
            DrawButton(btnPlay, Color.Red, TEXT_PLAY);
            DrawButton(btnHeberger, Color.Gray, TEXT_HEBERGER);
        }
        else if (BtnActive == TEXT_HEBERGER)
        {
            DrawButton(btnPlay, Color.Gray, TEXT_PLAY);
            DrawButton(btnHeberger, Color.Red, TEXT_HEBERGER);
        }

        if (CheckCollisionPointRec(GetMousePosition(), btnPlay) ||
            CheckCollisionPointRec(GetMousePosition(), btnHeberger))
        {
            SetMouseCursor(MouseCursor.PointingHand);
            if (CheckCollisionPointRec(GetMousePosition(), btnPlay) && IsMouseButtonDown(MouseButton.Left))
            {
                currentPage = LauncherPage.Play;
                Task.Run(GetServers);
            }
            else if (CheckCollisionPointRec(GetMousePosition(), btnHeberger) && IsMouseButtonDown(MouseButton.Left))
            {
                currentPage = LauncherPage.Heberger;
            }
        }
        else
        {
            SetMouseCursor(MouseCursor.Default);
        }

        return (int)BTN_PLAY_HEIGHT;
    }

    /// <summary>
    ///     Affiche la page "Jouer"
    /// </summary>
    /// <param name="close">Indicateur pour fermer la fenêtre</param>
    private static void Play(ref string joinIp, ref bool close, ref bool joinServer)
    {
        BeginDrawing();
        TopButtons(TEXT_PLAY);

        MouseCursor mouseCursor = MouseCursor.Default;

        for (int i = 0; i < servers.Count - 1; i++)
        {
            ServerObject? s = servers[i];
            s.Draw();

            if (s.CheckCollision(GetMousePosition()))
            {
                mouseCursor = MouseCursor.PointingHand;
                if (IsMouseButtonDown(MouseButton.Left))
                {
                    close = true;
                    joinServer = true;
                    joinIp = s.ip;
                }
            }
        }

        SetMouseCursor(mouseCursor);
        EndDrawing();
    }

    /// <summary>
    ///     Affiche la page "Heberger"
    /// </summary>
    /// <param name="ip">Entrée de l'adresse IP du serveur</param>
    private static void Heberger(ref string ip, ref bool close, ref bool joinServer)
    {
        Rectangle textBox = new(GetScreenWidth() * 0.5f - 500, GetScreenHeight() * 0.5f - 25, 1000, 50);
        int framesCounter = 0;
        int key = GetCharPressed();

        if (key >= 32 && key <= 125 && ip.Count() < MAX_INPUT_CHARS_NAME)
            ip += (char)key; // Ajouter un caractère à la chaîne

        if (IsKeyPressed(KeyboardKey.Backspace))
            if (ip.Count() > 0)
                ip = ip.Substring(0, ip.Count() - 1); // Supprimer le dernier caractère

        framesCounter++;

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        TopButtons(TEXT_HEBERGER);

        DrawText(
            "ENTREZ LE NOM DU SERVEUR",
            GetScreenWidth() / 2 - MeasureText("ENTREZ LE NOM DU SERVEUR", 20) / 2,
            GetScreenHeight() / 2 - 100 / 2,
            20,
            Color.Gray
        );

        DrawRectangleRec(textBox, Color.LightGray);
        DrawRectangleLines((int)textBox.X, (int)textBox.Y, (int)textBox.Width, (int)textBox.Height, Color.Red);

        DrawText(ip, (int)textBox.X + 5, (int)textBox.Y + 8, 40, Color.Maroon);

        DrawText(
            $"CARACTÈRES ENTRÉS : {ip.Count()}/{MAX_INPUT_CHARS_NAME}",
            GetScreenWidth() / 2 - MeasureText($"CARACTÈRES ENTRÉS : {ip.Count()}/{MAX_INPUT_CHARS_NAME}", 20) / 2,
            GetScreenHeight() / 2 - 20 / 2 + 50,
            20,
            Color.DarkGray
        );

        if (ip.Count() < MAX_INPUT_CHARS_NAME)
        {
            if (framesCounter / 20 % 2 == 0)
                DrawText("_", (int)textBox.X + 8 + MeasureText(ip, 40), (int)textBox.Y + 12, 40, Color.Maroon);
        }
        else
        {
            DrawText("Appuyez sur RETOUR pour supprimer des caractères...",
                GetScreenWidth() / 2 - MeasureText("Appuyez sur RETOUR pour supprimer des caractères...", 20) / 2,
                GetScreenHeight() / 2 - 300, 20, Color.Gray);
        }

        EndDrawing();

        if (IsKeyPressed(KeyboardKey.Enter))
        {
            close = true;
            SaveServer(ip);
            Server.StartServer(Program.DEFAULT_PORT);
            joinServer = true;
        }
    }

    /// <summary>
    ///     Sauvegarde un serveur dans la base de données
    /// </summary>
    /// <param name="ip">Adresse IP du serveur</param>
    private static void SaveServer(string ip)
    {
        using (MySqlConnection? conn = new(connectionString))
        {
            conn.Open();

            IPHostEntry? host = Dns.GetHostEntry(Dns.GetHostName());
            string? localIp = "0.0.0.0";

            foreach (IPAddress? ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ipAddress))
                {
                    localIp = ipAddress.ToString();
                    break;
                }
            }

            MySqlCommand? cmd = new($"INSERT INTO serveur (Ip, Nom) VALUES ('{localIp}', '{ip}')", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }


    /// <summary>
    ///     Récupère la liste des serveurs depuis la base de données
    /// </summary>
    /// <returns>Liste des serveurs</returns>
    private static async Task GetServers()
    {
        servers.Clear();

        using (MySqlConnection conn = new(connectionString))
        {
            conn.Open();
            MySqlCommand requete = new("SELECT Id, Ip, Nom, Nombre_Joueur FROM serveur", conn);

            using (MySqlDataReader? reader = requete.ExecuteReader())
            {
                int nbPassage = 0;

                while (reader.Read() && nbPassage < NB_SERVER_AFFICHER)
                {
                    servers.Add(new ServerObject(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                        reader.GetInt32(3), nbPassage));
                    nbPassage++;
                }
            }

            conn.Close();
        }

        CleanupServers();
    }

    /// <summary>
    ///     Valide et nettoie les serveurs inactifs
    /// </summary>
    private static void CleanupServers()
    {
        for (int i = 0; i < servers.Count; i++)
        {
            try
            {
                TcpClient? client = new();
                client.Connect(servers[i].ip, Program.DEFAULT_PORT + 1);
                client.Close();
            }
            catch
            {
                using (MySqlConnection conn = new(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new($"DELETE FROM serveur WHERE id = {servers[i].id};", conn);
                    cmd.ExecuteNonQuery();
                    servers.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    /// <summary>
    ///     Dessine un bouton avec un texte
    /// </summary>
    /// <param name="button">Rectangle du bouton</param>
    /// <param name="color">Couleur du bouton</param>
    /// <param name="text">Texte du bouton</param>
    private static void DrawButton(Rectangle button, Color color, string text)
    {
        DrawRectangleRec(button, color);
        DrawText(
            text,
            (int)(button.X + button.Width / 2 - MeasureText(text, FONT_SIZE) / 2),
            (int)(button.Y + button.Height / 2 - FONT_SIZE / 2),
            FONT_SIZE,
            Color.Black
        );
    }

    /// <summary>
    ///     Calcule la largeur du bouton en fonction du texte
    /// </summary>
    /// <param name="text">Texte du bouton</param>
    /// <returns>Largeur du bouton</returns>
    private static float CalculateButtonWidth(string text)
    {
        return MeasureText(text, FONT_SIZE) + 2 * FONT_SIZE;
    }

    /// <summary>
    ///     Crée un rectangle pour un bouton
    /// </summary>
    /// <param name="text">Texte du bouton</param>
    /// <param name="xPosition">Position X optionnelle</param>
    /// <returns>Rectangle pour le bouton</returns>
    private static Rectangle CreateButton(string text, float xPosition = -1)
    {
        return new Rectangle(
            xPosition == -1 ? GetScreenWidth() / 2 - CalculateButtonWidth(text) / 2 : xPosition,
            0,
            CalculateButtonWidth(text),
            BTN_PLAY_HEIGHT
        );
    }

    private static void LoadConnexionString()
    {
        using (StreamReader sr = new("ressources/connexion.txt"))
        {
            connectionString = $"Server={sr.ReadLine()};Database=raylib;Uid=root;Pwd=;";
        }
    }

    #region Constantes

    /// <summary>
    ///     Textes des boutons et autres constantes
    /// </summary>
    private const string TEXT_HEBERGER = "Heberger", TEXT_PLAY = "Jouer"; // ↩


    private const int MAX_INPUT_CHARS_NAME = 20;

    #endregion
}