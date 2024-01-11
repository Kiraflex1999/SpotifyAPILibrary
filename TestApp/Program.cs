using SpotifyAPILibrary;

namespace TestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AccessToken.RequestUserToken("user-read-email user-read-private playlist-read-private");
            Console.ReadLine();
        }
    }
}
