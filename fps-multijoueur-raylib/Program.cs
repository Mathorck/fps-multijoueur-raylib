using DeadOpsArcade3D.Launcher;

namespace DeadOpsArcade3D;

internal class Program
{
    public const int DEFAULT_PORT = 3855;

    private static void Main(string[] args)
    {
        ArgsVerif(args);
        Login.Start();
    }

    private static void ArgsVerif(string[] args)
    {
         
    }
}