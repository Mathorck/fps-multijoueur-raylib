using System.Numerics;
using DeadOpsArcade3D.Multiplayer;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Launcher.LauncherElement;

/// <summary>
///     Classe représentant un serveur dans la liste des serveurs disponibles.
///     Permet de gérer l'affichage, la détection de collision pour l'interaction et la connexion au serveur.
/// </summary>
public class ServerObject
{
    /// <summary>
    ///     Dimensions et espaces pour l'affichage des serveurs.
    ///     Ces valeurs sont récupérées à partir de la classe Launcher.
    /// </summary>
    private static readonly int recWidth = Launcher.btnServerWidth;

    private static readonly int recHeight = Launcher.btnServerHeight;
    private static readonly int TopMenuHeight = Launcher.TopMenuHeight;
    private static readonly int spaceBetweenServers = Launcher.spaceBetweenServer;

    /// <summary>
    ///     Identifiant unique du serveur.
    /// </summary>
    public int id;

    /// <summary>
    ///     Adresse IP du serveur.
    /// </summary>
    public string ip;

    /// <summary>
    ///     Indicateur pour savoir si le serveur est actif (sélectionné).
    /// </summary>
    public bool isActive;

    /// <summary>
    ///     Nombre de joueurs connectés au serveur.
    /// </summary>
    public int nbJoueur;

    /// <summary>
    ///     Nom du serveur.
    /// </summary>
    public string nom;

    /// <summary>
    ///     Représentation graphique du serveur sous forme de rectangle pour l'affichage.
    /// </summary>
    public Rectangle rec;

    /// <summary>
    ///     Constructeur de la classe Serveurs qui initialise un serveur avec ses propriétés.
    /// </summary>
    /// <param name="id">Identifiant du serveur.</param>
    /// <param name="ip">Adresse IP du serveur.</param>
    /// <param name="nom">Nom du serveur.</param>
    /// <param name="nbJoueur">Nombre de joueurs actuellement sur le serveur.</param>
    /// <param name="i">Index du serveur dans la liste pour positionner correctement le rectangle sur l'écran.</param>
    public ServerObject(int id, string ip, string nom, int nbJoueur, int i)
    {
        this.id = id;
        this.ip = ip;
        this.nom = nom;
        this.nbJoueur = nbJoueur;

        // Positionner le rectangle en fonction de l'écran et de l'index
        rec = new Rectangle(
            GetScreenWidth() / 2 - recWidth / 2,
            TopMenuHeight + (i + 1) * recHeight + (i + 1) * spaceBetweenServers,
            recWidth,
            recHeight
        );
    }

    /// <summary>
    ///     Vérifie si la position de la souris entre en collision avec le rectangle du serveur.
    /// </summary>
    /// <param name="mousePosition">Position actuelle de la souris.</param>
    /// <returns>Vrai si la souris est sur le rectangle, sinon faux.</returns>
    public bool CheckCollision(Vector2 mousePosition)
    {
        // Vérification de la collision avec le rectangle
        isActive = CheckCollisionPointRec(mousePosition, rec);
        return isActive;
    }

    /// <summary>
    ///     Connecte le client au serveur en utilisant l'adresse IP et le port par défaut.
    /// </summary>
    public void join()
    {
        // Démarrer le client avec l'IP et le port par défaut
        Client.StartClient(ip, Program.DEFAULT_PORT);
    }

    /// <summary>
    ///     Dessine le serveur à l'écran, y compris le rectangle de fond et les informations du serveur.
    /// </summary>
    public void Draw()
    {
        // Dessiner le rectangle représentant le serveur
        DrawRectangleRec(rec, Color.DarkGray);

        // Afficher les informations du serveur (ID, nom, IP, nombre de joueurs)
        var serverInfo = $"{id}  {nom} {ip} Joueurs : {nbJoueur}";

        DrawText(
            serverInfo,
            (int)(rec.X + Launcher.FONT_SIZE - 20),
            (int)(rec.Y + rec.Height / 2) - Launcher.FONT_SIZE / 2,
            Launcher.FONT_SIZE,
            Color.White
        );
    }
}