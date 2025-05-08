using golf_try2;

namespace kg_lab2
{
    internal static class Program
    {

        static void Main(string[] args)
        {
            using (Game game = new Game(400, 400))
            {
                game.Run();
            }

        }
    }
}