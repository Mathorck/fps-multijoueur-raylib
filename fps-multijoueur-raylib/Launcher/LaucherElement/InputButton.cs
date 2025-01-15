using System.Numerics;
using static Raylib_cs.Raylib;

namespace DeadOpsArcade3D.Launcher.LauncherElement;

/// <summary>
///     Représente un bouton d'entrée utilisateur pour saisir du texte.
/// </summary>
public class InputButton
{
    private readonly bool isSecret;
    public Color borderColor;
    public Color borderColorActive;
    public Color btnColor;
    public Color btnColorActive;
    public Color fontColor;
    public Color fontColorActive;

    public string inputText;
    private bool isActive;
    public int maxInputChar;
    public Rectangle rectangle;
    public string text;

    /// <summary>
    ///     Constructeur pour créer un bouton d'entrée utilisateur.
    /// </summary>
    /// <param name="rectangle">La zone de saisie du texte.</param>
    /// <param name="text">Le texte affiché sur le bouton.</param>
    /// <param name="maxInputChar">Le nombre maximal de caractères que l'utilisateur peut saisir.</param>
    /// <param name="backColor">Couleur du fond du bouton.</param>
    /// <param name="fontColor">Couleur du texte du bouton.</param>
    /// <param name="borderColor">Couleur de la bordure du bouton.</param>
    /// <param name="backColorActive">Couleur du fond du bouton lorsqu'il est actif.</param>
    /// <param name="fontColorActive">Couleur du texte du bouton lorsqu'il est actif.</param>
    /// <param name="borderColorActive">Couleur de la bordure du bouton lorsqu'il est actif.</param>
    /// <param name="isSecret">Indique si le texte doit être masqué (mot de passe).</param>
    public InputButton(Rectangle rectangle, string text, int maxInputChar, Color backColor, Color fontColor,
        Color borderColor, Color backColorActive, Color fontColorActive, Color borderColorActive, bool isSecret)
    {
        this.rectangle = rectangle;
        btnColor = backColor;
        this.fontColor = fontColor;
        this.borderColor = borderColor;
        this.text = text;
        inputText = string.Empty;
        this.maxInputChar = maxInputChar;
        btnColorActive = backColorActive;
        this.fontColorActive = fontColorActive;
        this.borderColorActive = borderColorActive;
        this.isSecret = isSecret;
    }

    /// <summary>
    ///     Vérifie si la position de la souris est dans la zone du bouton.
    /// </summary>
    /// <param name="mousePosition">Position actuelle de la souris.</param>
    /// <returns>Vrai si la souris est dans la zone du bouton, sinon faux.</returns>
    public bool CheckCollision(Vector2 mousePosition)
    {
        isActive = CheckCollisionPointRec(mousePosition, rectangle);
        return isActive;
    }

    /// <summary>
    ///     Ajoute un caractère à l'input texte si la limite n'est pas atteinte.
    /// </summary>
    /// <param name="caractere">Le caractère à ajouter.</param>
    public void AddChar(char caractere)
    {
        if (inputText.Length < maxInputChar)
            inputText += caractere;
    }

    /// <summary>
    ///     Supprime un caractère du texte d'entrée.
    /// </summary>
    public void RemoveChar()
    {
        if (inputText.Length > 0 && isActive)
            inputText = inputText.Substring(0, inputText.Length - 1);
    }

    /// <summary>
    ///     Dessine le bouton et le texte sur l'écran en fonction de son état (actif ou non).
    /// </summary>
    public void Draw()
    {
        // Dessiner en fonction de l'état actif du bouton
        if (!isSecret)
        {
            DrawButton();
            DrawText(inputText, (int)(rectangle.X + Launcher.FONT_SIZE),
                (int)(rectangle.Y + rectangle.Height / 2 - Launcher.FONT_SIZE / 2), Launcher.FONT_SIZE,
                isActive ? fontColorActive : fontColor);
        }
        else
        {
            DrawButton();
            string? maskedInput = new('*', inputText.Length);
            DrawText(maskedInput, (int)(rectangle.X + Launcher.FONT_SIZE),
                (int)(rectangle.Y + rectangle.Height / 2 - Launcher.FONT_SIZE / 2), Launcher.FONT_SIZE,
                isActive ? fontColorActive : fontColor);
        }

        // Afficher la limite de caractères
        DrawText($"{inputText.Length}/{maxInputChar}",
            (int)(rectangle.X + rectangle.Width + Launcher.FONT_SIZE),
            (int)(rectangle.Y + rectangle.Height / 2 - Launcher.FONT_SIZE / 2),
            Launcher.FONT_SIZE,
            Color.DarkGray
        );
    }

    /// <summary>
    ///     Dessine les bordures du bouton.
    /// </summary>
    /// <param name="Thick">L'épaisseur des bordures du bouton.</param>
    public void DrawBorder(float Thick)
    {
        Color borderColorToUse = isActive ? borderColorActive : borderColor;
        DrawRectangleLinesEx(rectangle, Thick, borderColorToUse);
    }

    /// <summary>
    ///     Dessine le fond du bouton avec le texte associé.
    /// </summary>
    private void DrawButton()
    {
        Color backgroundColor = isActive ? btnColorActive : btnColor;
        Color textColor = isActive ? fontColorActive : fontColor;

        DrawRectangleRec(rectangle, backgroundColor);
        DrawText(text, (int)(rectangle.X - MeasureText(text, Launcher.FONT_SIZE) - Launcher.FONT_SIZE),
            (int)(rectangle.Y + rectangle.Height / 2 - Launcher.FONT_SIZE / 2), Launcher.FONT_SIZE, textColor);
    }
}