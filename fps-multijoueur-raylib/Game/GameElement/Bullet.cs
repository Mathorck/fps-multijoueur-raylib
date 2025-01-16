using Raylib_cs;
using System;
using System.Numerics;
using System.Collections.Generic;

namespace DeadOpsArcade3D.Game.GameElement
{
    public class Bullet
    {
        public static List<Bullet> BulletsList = new List<Bullet>();
        private static Model bulletModel;

        public Vector3 Position;
        public Vector3 PlayerPosition;
        public Vector3 Target;
        public Vector3 Size;
        public Vector3 Direction;
        public float Speed;
        public Weapon Weapon;
        public BoundingBox BoundingBox;
        public float Rotation;

        public Bullet(Vector3 playerPos, Vector3 direction, Weapon w)
        {
            Position = new Vector3(playerPos.X, playerPos.Y - 0.05f, playerPos.Z);
            Target = new(direction.X, direction.Y - 0.05f, direction.Z);
            Size = new Vector3(0.1f, 0.1f, 0.1f);
            this.Direction = Vector3.Normalize(Target - Position);
            Speed = 75.0f;
            Weapon = w;
            BoundingBox = new BoundingBox(Position - Size / 2, Position + Size / 2);
            PlayerPosition = Position;
            Rotation = float.Atan2(Direction.X - PlayerPosition.X, Direction.Z - PlayerPosition.Z) * (180 / float.Pi);
        }

        /// <summary>
        /// Met à jour la position de la balle et vérifie les collisions
        /// </summary>
        /// <returns>True si la balle doit être supprimée</returns>
        public bool Update()
        {
            Position += Direction * Speed * Raylib.GetFrameTime();
            BoundingBox.Min = Position;
            //BoundingBox = new BoundingBox(Position - Size / 2, Position + Size / 2);

            // Vérification des collisions avec les murs
            if (CheckCollisionWithWalls())
            {
                return true; // Supprime la balle en cas de collision
            }

            return false; // Sinon, la balle continue
        }

        /// <summary>
        /// Vérifie si la balle entre en collision avec un mur
        /// </summary>
        /// <returns>True si une collision est détectée</returns>
        private unsafe bool CheckCollisionWithWalls()
        {
            Color* mapPixelsData = Map.MapPixels;

            for (int y = 0; y < Map.cubicmap.Height; y++)
            {
                for (int x = 0; x < Map.cubicmap.Width; x++)
                {
                    // Vérifie si le pixel correspondant est un mur (R == 255)
                    if (mapPixelsData[y * Map.cubicmap.Width + x].R == 255)
                    {
                        // Calcul des coordonnées du rectangle de collision pour ce mur
                        Rectangle wallRect = new Rectangle(
                            Map.mapPosition.X - 0.5f + x,
                            Map.mapPosition.Z - 0.5f + y,
                            0.98f,
                            0.98f
                        );

                        // Rectangle de la balle pour la collision
                        Rectangle bulletRect = new Rectangle(
                            Position.X - Size.X / 2,
                            Position.Z - Size.Z / 2,
                            Size.X,
                            Size.Z
                        );

                        // Vérifie la collision
                        if (Raylib.CheckCollisionRecs(wallRect, bulletRect))
                        {
                            return true; // Collision détectée
                        }
                    }
                }
            }

            return false; // Pas de collision
        }

        /// <summary>
        /// Dessine toutes les balles de la liste
        /// </summary>
        /// <param name="bullets">Liste des balles</param>
        public static void Draw(List<Bullet> bullets)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                Raylib.DrawModelEx(
                    bulletModel,                    // Le modèle de la balle
                    bullets[i].Position,            // Position de la balle
                    new System.Numerics.Vector3(0, 1, 0), // Axe de rotation (axe Y)
                    float.Atan2(bullets[i].Target.X - bullets[i].PlayerPosition.X, bullets[i].Target.Z - bullets[i].PlayerPosition.Z) * (180 / float.Pi),                          // L'angle en radians
                    new System.Numerics.Vector3(0.025f, 0.025f, 0.025f), // Échelle du modèle
                    Color.White                     // Couleur du modèle
                );
                // Supprime la balle si elle doit l'être
                if (bullets[i].Update())
                {
                    bullets.RemoveAt(i);
                    i--; // Ajuste l'indice pour éviter de sauter une balle
                }
            }
        }

        /// <summary>
        /// Initialise le modèle de balle
        /// </summary>
        public static void Init()
        {
            bulletModel = Raylib.LoadModel("./ressources/model3d/ammo/bullet.glb");
        }
    }
}
