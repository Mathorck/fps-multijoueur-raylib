using DeadOpsArcade3D.Multiplayer;

namespace DeadOpsArcade3D
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("1. Lancer le serveur\n2. Lancer le client");
            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Port du serveur (appuyez sur Entrée pour utiliser le port par défaut 3855): ");
                string? portInput = Console.ReadLine();
                int port = string.IsNullOrWhiteSpace(portInput) ? 3855 : int.Parse(portInput);
                Server.StartServer(port);
            }
            else if (choice == "2")
            {
                Console.Write("Adresse du serveur: ");
                string? hostInput = Console.ReadLine();
                string host = string.IsNullOrWhiteSpace(hostInput) ? "127.0.0.1" : hostInput;
                Console.Write("Port (appuyez sur Entrée pour utiliser le port par défaut 3855): ");
                string? portInput = Console.ReadLine();
                int port = string.IsNullOrWhiteSpace(portInput) ? 3855 : int.Parse(portInput);
                Client.StartClient(host, port);
            }
        }
    }

}
