using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raylib_cs.Raylib;
using System.Numerics;
using Raylib_cs;

namespace Classes
{
    public class Player
    {
        public Vector3 Position;
        public Vector3 Size;
        public float speed;
        public float life;
        public BoundingBox BoundingBox;

        public Player(Model Model) 
        { 
            Position = new Vector3(0f, 0f, 0f);
            Size = GetModelBoundingBox(Model).Max * 0.3f;
            speed = 1f;
            life = 100f;
            BoundingBox = new BoundingBox(Position, Size);
        }

        public void Update(Vector3 playerPos)
        {
            
        }
    }
}
