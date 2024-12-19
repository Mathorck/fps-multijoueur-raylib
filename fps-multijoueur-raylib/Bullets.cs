using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeadOpsArcade3D
{
    public class Bullets
    {
        public Vector3 Position;
        public Vector3 Size;
        public Vector3 Direction;
        public float speed;
        public float FrameTime;
        public Weapon Weapon;

        public Bullets(Vector3 playerPos, Vector3 Direction, float GetFrameTime, Weapon w) 
        { 
            Position = new Vector3(playerPos.X, playerPos.Y, playerPos.Z);
            Size = new Vector3(0.1f, 0.1f, 0.1f);
            this.Direction = Vector3.Normalize(Direction - Position);
            speed = 5.0f;
            FrameTime = GetFrameTime;
            Weapon = w;
        }

        public bool update()
        {
            Position += Direction * speed * FrameTime;
            if(Position.Y <= 0) 
            {
                return true;
            }
            return false;
        }
    }
}
