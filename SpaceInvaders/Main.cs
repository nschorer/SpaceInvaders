using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the instance
            SpaceInvaders game = new SpaceInvaders();
            Debug.Assert(game != null);

            // Start the game
            game.Run();
        }
    }
}
