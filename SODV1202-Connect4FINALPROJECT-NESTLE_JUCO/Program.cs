//SODV1202 Connect Four Game Final Term Project
//Submitted By: Nestle Juco
using System;

namespace SODV1202FinalProject
{
    //Interface
    interface IGame
    {
        void Play();
    }

    //Player Abstract Class and Inheritance
    abstract class Player
    {
        public string Name { get; }
        public char Symbol { get; }

        public Player(string name, char symbol)
        {
            Name = name;
            Symbol = symbol;
        }
    }

    class HumanPlayer : Player
    {
        public HumanPlayer(string name, char symbol) : base(name, symbol)
        {
        }
    }

    class Connect4TwoPlayerHuman: IGame
    {
        private const int Rows = 6;
        private const int Columns = 7;
        private char[,] board;
        private bool isPlayerOnesTurn;
        private Player player1;
        private Player player2;

        public Connect4TwoPlayerHuman()
        {
            board = new char[Rows, Columns];
            isPlayerOnesTurn = true;
            InitializeBoard();
            Console.WriteLine("\nConnect 4 Two Player Human vs Human Battle Selected!\n");
            InitializePlayers();
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    board[row, col] = ' ';
                }
            }
        }

        private void InitializePlayers()
        {
            Console.Write("Enter name for Player 1: ");
            string player1Name = Console.ReadLine();
            Console.Write("Enter name for Player 2: ");
            string player2Name = Console.ReadLine();
            player1 = new HumanPlayer(player1Name, 'X');
            player2 = new HumanPlayer(player2Name, 'O');
        }

        public void Play()
        {
            bool isGameOver = false;

            while (!isGameOver)
            {
                Console.Clear();
                PrintBoard();

                int column = GetMove();
                if (IsValidMove(column))
                {
                    int row = DropPiece(column);

                    if (IsWinningMove(row, column))
                    {
                        Console.Clear();
                        PrintBoard();
                        Console.WriteLine($"It's a connect four! Player {(isPlayerOnesTurn ? player1.Name : player2.Name)} wins!");

                        isGameOver = true;
                        if (PlayAgain())
                        {
                            isGameOver = false;
                            InitializeBoard();
                            isPlayerOnesTurn = true;
                        }
                    }
                    else if (IsDraw())
                    {
                        Console.Clear();
                        PrintBoard();
                        Console.WriteLine("It's a draw!");

                        isGameOver = true;
                        if (PlayAgain())
                        {
                            isGameOver = false;
                            InitializeBoard();
                            isPlayerOnesTurn = true;
                        }
                    }
                    else
                    {
                        isPlayerOnesTurn = !isPlayerOnesTurn;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid move. Try again.");
                }
            }
        }

        private bool PlayAgain()
        {
            Console.Write("Do you want to play again? (Y/N): ");
            string input = Console.ReadLine().Trim();
            if (input == "Y" || input == "y")
            {
                return input.Equals("Y", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                Console.WriteLine("Thank you for playing!");
            }
            return false;
        }


        private void PrintBoard()
        {
            Console.WriteLine("Connect Four Game Project\n");
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    Console.Write($"| {board[row, col]} ");
                }
                Console.WriteLine("|");
            }

            Console.WriteLine("-----------------------------");
            Console.WriteLine("  1   2   3   4   5   6   7  ");
            Console.WriteLine("-----------------------------");
        }

        private int GetMove()
        {
            Player currentPlayer = isPlayerOnesTurn ? player1 : player2;
            Console.WriteLine($"Player {currentPlayer.Name}'s turn, enter a column number (1-7):");
            int column = Convert.ToInt32(Console.ReadLine()) - 1;
            return column;
        }

        private bool IsValidMove(int column)
        {
            if (column < 0 || column >= Columns)
            {
                return false;
            }

            return board[0, column] == ' ';
        }

        private int DropPiece(int column)
        {
            int row = -1;
            for (int i = Rows - 1; i >= 0; i--)
            {
                if (board[i, column] == ' ')
                {
                    Player currentPlayer = isPlayerOnesTurn ? player1 : player2;
                    board[i, column] = currentPlayer.Symbol;
                    row = i;
                    break;
                }
            }
            return row;
        }

        private bool IsWinningMove(int row, int col)
        {
            char playerSymbol = isPlayerOnesTurn ? player1.Symbol : player2.Symbol;

            // Check for horizontal win
            int count = 0;
            for (int c = Math.Max(0, col - 3); c <= Math.Min(col + 3, Columns - 1); c++)
            {
                if (board[row, c] == playerSymbol)
                {
                    count++;
                    if (count == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }

            // Check for vertical win
            count = 0;
            for (int r = Math.Max(0, row - 3); r <= Math.Min(row + 3, Rows - 1); r++)
            {
                if (board[r, col] == playerSymbol)
                {
                    count++;
                    if (count == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }

            // Check for diagonal win (top-left to bottom-right)
            count = 0;
            int startRow = row - Math.Min(row, col);
            int startCol = col - Math.Min(row, col);
            for (int i = 0; i < Math.Min(Rows - startRow, Columns - startCol); i++)
            {
                if (board[startRow + i, startCol + i] == playerSymbol)
                {
                    count++;
                    if (count == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }

            // Check for diagonal win (top-right to bottom-left)
            count = 0;
            startRow = row - Math.Min(row, Columns - 1 - col);
            startCol = col + Math.Min(row, Columns - 1 - col);
            for (int i = 0; i < Math.Min(Rows - startRow, startCol + 1); i++)
            {
                if (board[startRow + i, startCol - i] == playerSymbol)
                {
                    count++;
                    if (count == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }

            return false;
        }

        private bool IsDraw()
        {
            for (int col = 0; col < Columns; col++)
            {
                if (board[0, col] == ' ')
                {
                    return false;
                }
            }
            return true;
        }
    }

    /*class Player
    {
        public string Name { get; }
        public char Symbol { get; }

        public Player(string name, char symbol)
        {
            Name = name;
            Symbol = symbol;
        }
    }*/

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
                    game.Play();
                    isValidSelection = true;
                }
                else if (sel == 2)
                {
                    Console.WriteLine("Sorry, this game mode is not yet available. Select other game mode.");
                    //isValidSelection = true;
                }
                else if (sel == 3)
                {
                    Console.WriteLine("Exit Game, Thank you for playing!");
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
