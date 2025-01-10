using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadOpsArcade3D.Launcher.LauncherElement
{
    public class User
    {
        public int Id;
        public string Pseudo;

        public User(int id, string pseudo)
        {
            Id = id;
            Pseudo = pseudo;
        }
    }
}
