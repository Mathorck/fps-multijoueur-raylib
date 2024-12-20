using static Raylib_cs.Raylib;
using System.Numerics;
using DeadOpsArcade3D.Game;
using DeadOpsArcade3D.Multiplayer;
using Raylib_cs;

namespace DeadOpsArcade3D.GameElement
{
    public class Player
    {
        const float JUMP_HEIGHT = 5f;
        const float JUMP_SPEED = 15f;
        const float PLAYER_SPEED = 0.1f;

        /// <summary>Liste des qui contient tous les joueurs </summary>
        public static List<Player> PlayerList = new List<Player>();

        public static Model DefaultModel;
        public static bool isJumping = false;
        public static bool isGoingUp = false;
        public static float initpos = 0;


        public Vector3 Position;
        public Vector3 Rotation;

        public Vector3 Size;
        public float Speed;
        public float Life;
        public BoundingBox BoundingBox;

        public Player(float x,float y, float z, float xRot, float yRot, float zRot)
        {
            Position = new Vector3(x, y, z);
            Size = GetModelBoundingBox(DefaultModel).Max * 0.3f;
            Speed = 1f;
            Life = 100f;
            BoundingBox = new BoundingBox(Position, Size);
            Rotation = new Vector3(xRot, yRot, zRot);
        }

        public static void DrawAll(List<Player> playerList)
        {
            if (playerList == null || playerList.Count == 0)
            {
                Client.ConsoleError("La liste des joueurs est vide ou null.");
                return;
            }
            try
            {
                for (int i = 0; i < playerList.Count; i++)
                {
                    playerList[i].Draw();
                }
            }
            catch (Exception e)
            {
                Client.ConsoleError($"Error: {e.Message}");
            }

        }

        /// <summary>
        /// Permet l'affichage de tous les Joueurs
        /// </summary>
        /// <param name="playerList">Liste des Joueurs</param>
        public void Draw()
        {
            // TODO: Gèrer l'orientation
            DrawModel(
                DefaultModel, 
                Position - new Vector3(0,2,0), 
                0.1f, 
                Color.DarkGray
            );
            
            /* C'est pas au bon endroit En plus il me fait crash
            for (int b = 0; b < BulletsList.Count; b++)
            {
                if (CheckCollisionBoxes(BulletsList[b].BoundingBox, playerList[p].BoundingBox))
                {
                    playerList[p].Life -= BulletsList[b].Weapon.damage;
                    BulletsList.RemoveAt(b);
                    b--;
                    if (playerList[p].Life <= 0)
                    {
                        ////tuer le player
                    }
                }
            }
            */
            
        }

        /// <summary>
        /// Gère le mouvement du joueur principal
        /// </summary>
        /// <param name="camera"></param>
        public static void Movement(ref Camera3D camera)
        {
            // Saut
            if (IsKeyPressed(KeyboardKey.Space) && !isJumping)
            {
                isJumping = true;
                isGoingUp = true;
                initpos = camera.Position.Y;
            }

            if (isJumping)
            {
                // Logique du saut (comme dans le code précédent)
                if (camera.Position.Y < initpos + JUMP_HEIGHT && isGoingUp)
                {
                    camera.Position.Y += JUMP_SPEED * GetFrameTime();
                    camera.Target.Y += JUMP_SPEED * GetFrameTime();
                }
                else if (camera.Position.Y > initpos)
                {
                    if (camera.Position.Y - JUMP_SPEED * GetFrameTime() > initpos)
                    {
                        camera.Position.Y -= JUMP_SPEED * GetFrameTime();
                        camera.Target.Y -= JUMP_SPEED * GetFrameTime();
                    }
                    else
                    {
                        camera.Position.Y = initpos;
                        camera.Target.Y = initpos;
                        isJumping = false;
                        isGoingUp = false;
                    }
                }
            }

            // Déplacement (gestion des touches W, A, S, D)
            /*
            Vector3 movement = Vector3.Zero;
            if (IsKeyDown(KeyboardKey.W)) movement.Z -= PLAYER_SPEED;
            if (IsKeyDown(KeyboardKey.S)) movement.Z += PLAYER_SPEED;
            if (IsKeyDown(KeyboardKey.A)) movement.X -= PLAYER_SPEED;
            if (IsKeyDown(KeyboardKey.D)) movement.X += PLAYER_SPEED;

            // Calculer la nouvelle position de la caméra en fonction du déplacement
            Vector3 newPosition = camera.Position + movement;

            // Vérification des collisions avec les obstacles avant d'appliquer la nouvelle position
            if (!Map.CheckCollisionWithObstacles(newPosition, new Vector3(1.0f, 1.0f, 1.0f)))
            {
                // Si aucune collision n'est détectée, on applique la nouvelle position à la caméra
                camera.Position = newPosition;
                //camera.Target = newPosition + new Vector3(0, 0, -1); // Maintien la direction de la caméra
            }

            // Rotation de la caméra avec la souris
            /*Vector2 mouseDelta = GetMouseDelta();

            // Sensibilité de la rotation
            float rotationSpeed = 0.05f;

            // Limites de l'angle de rotation vertical
            float verticalAngleLimit = 80f;

            // Rotation horizontale (autour de l'axe Y)
            camera.Target = Vector3.Transform(camera.Target - camera.Position, Matrix4x4.CreateFromAxisAngle(new Vector3(0, 1, 0), -mouseDelta.X * rotationSpeed)) + camera.Position;

            // Rotation verticale (autour de l'axe X)
            Vector3 forward = Vector3.Normalize(camera.Target - camera.Position);
            float angleX = (float)Math.Acos(Vector3.Dot(forward, new Vector3(0, 1, 0))); // Angle entre la caméra et l'axe Y
            float deltaAngleX = mouseDelta.Y * rotationSpeed;

            // Empêcher la rotation verticale excessive (pas plus de 80 degrés vers le haut ou vers le bas)
            if (angleX + deltaAngleX < Math.PI / 2 && angleX + deltaAngleX > -Math.PI / 2)
            {
                camera.Target = Vector3.Transform(camera.Target - camera.Position, Matrix4x4.CreateFromAxisAngle(new Vector3(1, 0, 0), deltaAngleX)) + camera.Position;
            }*/

            // Réinitialisation de la position de la caméra à chaque boucle


            float W = IsKeyDown(KeyboardKey.W) * PLAYER_SPEED - IsKeyDown(KeyboardKey.S) * PLAYER_SPEED;
            float D = IsKeyDown(KeyboardKey.D) * PLAYER_SPEED - IsKeyDown(KeyboardKey.A) * PLAYER_SPEED;
            Vector3 Old = camera.Position;

            Vector3 New = new(0, 0, 0);


            Vector3 movement = Vector3.Zero;
            if (IsKeyDown(KeyboardKey.W)) movement.Z -= PLAYER_SPEED;
            if (IsKeyDown(KeyboardKey.S)) movement.Z += PLAYER_SPEED;
            if (IsKeyDown(KeyboardKey.A)) movement.X -= PLAYER_SPEED;
            if (IsKeyDown(KeyboardKey.D)) movement.X += PLAYER_SPEED;

            // Calculer la nouvelle position de la caméra en fonction du déplacement
            Vector3 newPosition = camera.Position + movement;



            float angle = float.Atan2(camera.Target.X - camera.Position.X, camera.Target.Z - camera.Position.Z) * (180 / float.Pi);
            Console.WriteLine(angle);

            if((angle >= 0 && angle <= 90 && IsKeyDown(KeyboardKey.W)) || 
                (angle >= 90 && angle <= 180 && IsKeyDown(KeyboardKey.D)) || 
                (angle >= -180 && angle <= -90 && IsKeyDown(KeyboardKey.S)) || 
                (angle >= -90 && angle <= 0 && IsKeyDown(KeyboardKey.A)))
            {
                Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\nXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\nXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\nXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\nXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\nXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\nXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\nXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\nXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\nXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\nXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\nXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\nXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\nXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX\n");

                if (!Map.CheckCollisionWithObstacles(Old - new Vector3(PLAYER_SPEED * GetFrameTime(), 0, 0), Vector3.One, PLAYER_SPEED*GetFrameTime(), 0))
                    New = new(W, D, 0);
                else
                    New = new(0, 0, 0);
            }
            if ((angle >= 45 && angle <= 135 && IsKeyDown(KeyboardKey.S)) ||
                (((angle >= 135 && angle <= 180) || (angle >= -180 && angle <= -135)) && IsKeyDown(KeyboardKey.A)) ||
                (angle >= -135 && angle <= -45 && IsKeyDown(KeyboardKey.W)) ||
                (angle >= -45 && angle <= 45 && IsKeyDown(KeyboardKey.D)))
            {
                Console.WriteLine("-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X\n-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X\n-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X\n-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X\n-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X\n-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X\n-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X\n-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X\n-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X-X");

                if (!Map.CheckCollisionWithObstacles(Old - new Vector3(PLAYER_SPEED * GetFrameTime(), 0, 0), Vector3.One, -PLAYER_SPEED * GetFrameTime(), 0))
                    New = new(W, D, 0);
                else
                    New = new(0, 0, 0);
            }
            if ((angle >= 45 && angle <= 135 && IsKeyDown(KeyboardKey.D)) ||
                (((angle >= 135 && angle <= 180) || (angle >= -180 && angle <= -135)) && IsKeyDown(KeyboardKey.S)) ||
                (angle >= -135 && angle <= -45 && IsKeyDown(KeyboardKey.A)) ||
                (angle >= -45 && angle <= 45 && IsKeyDown(KeyboardKey.W)))
            {
                Console.WriteLine("YYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY\nYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY\nYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY\nYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY\nYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");

                if (!Map.CheckCollisionWithObstacles(Old - new Vector3(PLAYER_SPEED * GetFrameTime(), 0, 0), Vector3.One, 0, PLAYER_SPEED * GetFrameTime()))
                    New = new(W, D, 0);
                else
                    New = new(0, 0, 0);
            }
            if ((angle >= 45 && angle <= 135 && IsKeyDown(KeyboardKey.A)) ||
                (((angle >= 135 && angle <= 180) || (angle >= -180 && angle <= -135)) && IsKeyDown(KeyboardKey.W)) ||
                (angle >= -135 && angle <= -45 && IsKeyDown(KeyboardKey.D)) ||
                (angle >= -45 && angle <= 45 && IsKeyDown(KeyboardKey.S)))
            {
                Console.WriteLine("-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y\n-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y\n-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y\n-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y\n-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y\n-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y-Y\n");

                if (!Map.CheckCollisionWithObstacles(Old - new Vector3(PLAYER_SPEED * GetFrameTime(), 0, 0), Vector3.One, 0, -PLAYER_SPEED * GetFrameTime()))
                    New = new(W, D, 0);
                else
                    New = new(0, 0, 0);
            }





            //if (!Map.CheckCollisionWithObstacles(Old - new Vector3(PLAYER_SPEED * GetFrameTime(), 0, 0), Vector3.One, PLAYER_SPEED * GetFrameTime()))
            //            New = new(W, D, 0);
            //        else
            //            New = new(0, 0, 0);



                //if (movement.X == PLAYER_SPEED)
                //{
                //    if (!Map.CheckCollisionWithObstacles(Old - new Vector3(PLAYER_SPEED, 0, 0), Vector3.One))
                //        New = new(W, D, 0);
                //    else
                //        New = new(0, 0, 0);
                //}
                //if (movement.X == -PLAYER_SPEED)
                //{
                //    if (!Map.CheckCollisionWithObstacles(Old - new Vector3(-PLAYER_SPEED, 0, 0), Vector3.One))
                //        New = new(W, D, 0);
                //    else
                //        New = new(0, 0, 0);
                //}
                //if (movement.Y == PLAYER_SPEED)
                //{
                //    if (!Map.CheckCollisionWithObstacles(Old - new Vector3(0, PLAYER_SPEED, 0), Vector3.One))
                //        New = new(W, D, 0);
                //    else
                //        New = new(0, 0, 0);
                //}
                //if (movement.Y == -PLAYER_SPEED)
                //{
                //    if (!Map.CheckCollisionWithObstacles(Old - new Vector3(0, -PLAYER_SPEED, 0), Vector3.One))
                //        New = new(W, D, 0);
                //    else
                //        New = new(0, 0, 0);
                //}




                //if (IsKeyDown(KeyboardKey.A))
                //{
                //    if (!Map.CheckCollisionWithObstacles(Old - new Vector3(W, D, 0), Vector3.One))
                //        New = new(W, D, 0);
                //    else
                //        New = new(0, 0, 0);
                //}
                //if (IsKeyDown(KeyboardKey.S))
                //{
                //    if (!Map.CheckCollisionWithObstacles(Old + new Vector3(W, D, 0), Vector3.One))
                //        New = new(W, D, 0);
                //    else
                //        New = new(0, 0, 0);
                //}
                //if (IsKeyDown(KeyboardKey.D))
                //{
                //    if (!Map.CheckCollisionWithObstacles(Old + new Vector3(W, D, 0), Vector3.One))
                //        New = new(W, D, 0);
                //    else
                //        New = new(0, 0, 0);
                //}

                //if (!Map.CheckCollisionWithObstacles(Old + new Vector3(W, D, 0), Vector3.One))
                //    New = new(W, D, 0);
                //else
                //    New = new(0,0,0);


                UpdateCameraPro(
                ref camera,
                New,
                new Vector3(GetMouseDelta().X * GameLoop.sensibilité, GetMouseDelta().Y * GameLoop.sensibilité, 0),
                0
            );
        }

    }
}
