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

        public Player() 
        { 
            Position = new Vector3(0f, 0f, 0f);
            Size = new Vector3(1f, 5f, 1f);
            speed = 1f;
            life = 100f;
        }

        public void Update(Vector3 playerPos)
        {
            
        }
    }
}
