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
        public static Model DefaultModel;
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

        public void Update()
        {

        }
    }
}
