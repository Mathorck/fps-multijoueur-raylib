using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using static Raylib_cs.Raylib;
using DeadOpsArcade3D.Launcher;

namespace DeadOpsArcade3D.Launcher.LauncherElement
{
    public class Boutton
    {
        public Rectangle rectangle;
        public string text;
        public Color btnColor;
        public Color fontColor;
        public Color btnColorActive;
        public Color fontColorActive;
        public LauncherPage onClickPage;

        private bool isActive = false;

        public Boutton(Rectangle rectangle, string text, Color btnColor, Color fontColor, Color btnColorActive, Color fontColorActive, LauncherPage onClickPage)
        {
            this.rectangle = rectangle;
            this.text = text;
            this.btnColor = btnColor;
            this.fontColor = fontColor;
            this.btnColorActive = btnColorActive;
            this.fontColorActive = fontColorActive;
            this.onClickPage = onClickPage;
        }

        public Boutton(Rectangle rectangle, string text, Color btnColor, Color fontColor, Color btnColorActive, Color fontColorActive)
        {
            this.rectangle = rectangle;
            this.text = text;
            this.btnColor = btnColor;
            this.fontColor = fontColor;
            this.btnColorActive = btnColorActive;
            this.fontColorActive = fontColorActive;
        }

        /// <summary>
        /// Checker la collision avec le bouton
        /// </summary>
        /// <param name="mousePosition">Position de la souris</param>
        /// <returns></returns>
        public bool CheckCollision(Vector2 mousePosition)
        {
            if (CheckCollisionPointRec(mousePosition, rectangle))
            {
                isActive = true;
                return true;
            }
            else
            {
                isActive = false;
                return false;
            }
        }

        /// <summary>
        /// Changer de page
        /// </summary>
        /// <param name="currentPage">Page Actif</param>
        public void OnClick(ref LauncherPage currentPage)
        {
            currentPage = onClickPage;
        }

        /// <summary>
        /// Dessiner le bouton
        /// </summary>
        public void Draw()
        {
            if (!isActive)
            {
                DrawRectangleRec(rectangle, btnColor);
                DrawText(text, (int)(rectangle.X + rectangle.Width / 2 - MeasureText(text, Launcher.FONT_SIZE) / 2), (int)(rectangle.Y + rectangle.Height / 2 - Launcher.FONT_SIZE / 2), Launcher.FONT_SIZE, fontColor);
            }
            else
            {
                DrawRectangleRec(rectangle, btnColorActive);
                DrawText(text, (int)(rectangle.X + rectangle.Width / 2 - MeasureText(text, Launcher.FONT_SIZE) / 2), (int)(rectangle.Y + rectangle.Height / 2 - Launcher.FONT_SIZE / 2), Launcher.FONT_SIZE, fontColorActive);
            }
        }

        /// <summary>
        /// Dessiner les contours
        /// </summary>
        /// <param name="color">Couleur des contours</param>
        /// <param name="Thick">Epessaire des contours</param>
        public void DrawContour(Color color, float Thick)
        {
            DrawRectangleLinesEx(rectangle, Thick, color);
        }
    }
}
