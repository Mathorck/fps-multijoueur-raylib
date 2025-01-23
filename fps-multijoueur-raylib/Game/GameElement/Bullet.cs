using Raylib_cs;
using System;
using System.Numerics;
using System.Collections.Generic;
using DeadOpsArcade3D.Multiplayer;


namespace DeadOpsArcade3D.Game.GameElement;

public class Bullet
{
    public static List<Bullet> BulletsList = new();
    private static Model bulletModel;
    public static Sound bulletSound;
    public BoundingBox BoundingBox;
    public Vector3 Direction;
    public Vector3 PlayerPosition;

    public Vector3 Position;
    public float Rotation;
    public Vector3 Size;
    public float Speed;
    public Vector3 Target;
    public Weapon Weapon;
    public Player Sender;
    
    private static Random random = new();

    public Bullet(Vector3 playerPos, Vector3 direction, Weapon w, Player sender)
    {
        Position = new Vector3(playerPos.X, playerPos.Y - 0.05f, playerPos.Z);
        Target = new(direction.X, direction.Y - 0.05f, direction.Z);
        Size = new Vector3(0.1f, 0.1f, 0.1f);
        this.Direction = Vector3.Normalize(Target - Position);
        Speed = 25.0f;
        Weapon = w;
        BoundingBox = new BoundingBox(Position - Size / 2, Position + Size / 2);
        PlayerPosition = Position;
        Rotation = float.Atan2(Direction.X - PlayerPosition.X, Direction.Z - PlayerPosition.Z) * (180 / float.Pi);
        Sender = sender;
        
        
        
        float distance = Vector3.Distance(GameLoop.camera.Position, PlayerPosition);
        float volume = Math.Clamp(1.0f / (distance + 1.0f), 0.1f, 1.0f);
        float randomPitch = 0.8f + (float)random.NextDouble() * 0.4f;
        Raylib.SetSoundPitch(bulletSound, randomPitch);
        Raylib.SetSoundVolume(bulletSound, volume);
        Raylib.PlaySound(bulletSound);
    }


    public static void Unload()
    {
        Raylib.UnloadModel(bulletModel);
    }

    /// <summary>
    /// Met à jour la position de la balle et vérifie les collisions
    /// </summary>
    /// <returns>True si la balle doit être supprimée</returns>
    public bool Update()
    {
        Position += Direction * Speed * Raylib.GetFrameTime();
        BoundingBox.Min = Position + new Vector3(-0.01f,-0.01f,-0.01f);
        BoundingBox.Max = Position - new Vector3(0.1f,0.1f,0.1f);
        //BoundingBox = new BoundingBox(Position - Size / 2, Position + Size / 2);

        // Vérification des collisions avec les murs
        if (CheckCollisionWithWalls())
            return true; // Supprime la balle en cas de collision

        if (CheckCollisionWithPlr())
            return true;

        return false; // Sinon, la balle continue
    }

    /// <summary>
    /// Vérifie si la balle entre en collision avec un mur
    /// </summary>
    /// <returns>True si une collision est détectée</returns>
    private unsafe bool CheckCollisionWithWalls()
    {
        Color* mapPixelsData = Map.MapPixels;

        for (int y = 0; y < Map.Cubicmap.Height; y++)
        {
            for (int x = 0; x < Map.Cubicmap.Width; x++)
            {
                // Vérifie si le pixel correspondant est un mur (R == 255)
                if (mapPixelsData[y * Map.Cubicmap.Width + x].R == 255)
                {
                    // Calcul des coordonnées du rectangle de collision pour ce mur
                    Rectangle wallRect = new Rectangle(
                        Map.MapPosition.X - 0.5f + x,
                        Map.MapPosition.Z - 0.5f + y,
                        1,
                        1
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


    private unsafe bool CheckCollisionWithPlr()
    {
        try
        {
            foreach (Bullet bullet in BulletsList)
            {
                if (Raylib.CheckCollisionBoxes(bullet.BoundingBox, Player.BB) && bullet.Sender != null)
                {
                    Player.Health -= bullet.Weapon.damage;
                    return true;
                }

            }

            if(Player.Health <= 0)
            {
                Player.Health = 100;
                GameLoop.camera = new()
                {
                    Position = GameLoop.ListSpawn[GameLoop.random.Next(0, 7)],
                    Target = new Vector3(0.0f, 1.0f, 0.0f),
                    Up = new Vector3(0.0f, 1.0f, 0.0f),
                    FovY = 60.0f,
                    Projection = CameraProjection.Perspective
                };
            }
        }
        catch (Exception e)
        {
            Client.ConsoleError($"Erreur de Neuille avec la liste des bullets {e}");
        }
        return false;
    }



    /// <summary>
    /// Dessine toutes les balles de la liste
    /// </summary>
    /// <param name="bullets">Liste des balles</param>
    public static void Draw(List<Bullet> bullets)
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            //Raylib.DrawBoundingBox(bullets[i].BoundingBox, Color.Blue);
            Raylib.DrawModelEx(
                bulletModel,                    // Le modèle de la balle
                bullets[i].Position,            // Position de la balle
                new System.Numerics.Vector3(0, 1, 0), // Axe de rotation (axe Y)
                float.Atan2(bullets[i].Target.X - bullets[i].PlayerPosition.X, bullets[i].Target.Z - bullets[i].PlayerPosition.Z) * (180 / float.Pi),                          // L'angle en radians
                new System.Numerics.Vector3(0.01f, 0.01f, 0.01f), // Échelle du modèle
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
        bulletSound = Raylib.LoadSound("./ressources/sound/bullet.wav");
        bulletModel = Raylib.LoadModel("./ressources/model3d/ammo/bullet.glb");
    }
}


