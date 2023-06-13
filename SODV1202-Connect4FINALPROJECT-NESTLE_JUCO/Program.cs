using System;

namespace SODV1202FinalProject
{
    class Connect4TwoPlayerHuman
    {
        private bool isPlayerOnesTurn;
        private Player player1;
        private Player player2;

        public Connect4TwoPlayerHuman()
        {
            Console.WriteLine("\nConnect 4 Two Player Human vs Human Battle Selected!\n");
            InitializePlayers();
        }

        private void InitializePlayers()
        {
            Console.Write("Enter name for Player 1: ");
            string player1Name = Console.ReadLine();
            Console.Write("Enter name for Player 2: ");
            string player2Name = Console.ReadLine();
            player1 = new Player(player1Name, 'X');
            player2 = new Player(player2Name, 'O');
        }
    }



    class Player
    {
        public string Name { get; }
        public char Symbol { get; }

        public Player(string name, char symbol)
        {
            Name = name;
            Symbol = symbol;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Connect Four Game!\n");
            Console.WriteLine("Please select game mode below:\n");
            Console.WriteLine("1 - Human vs Human");
            Console.WriteLine("2 - Human vs AI Bot");
            Console.WriteLine("3 - Exit\n");

            int sel;
            bool isValidSelection = false;

            while (!isValidSelection)
            {
                Console.Write("Key in Selection (1-3): ");
                string select = Console.ReadLine();
                sel = int.Parse(select);

                if (sel == 1)
                {
                    Connect4TwoPlayerHuman game = new Connect4TwoPlayerHuman();
                    isValidSelection = true;
                }
                else if (sel == 2)
                {
                    Console.WriteLine("Sorry, this game mode is not yet available.");
                    isValidSelection = true;
                }
                else if (sel == 3)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid Selection. Please try again.\n");
                }
            }
        }
    }
}
