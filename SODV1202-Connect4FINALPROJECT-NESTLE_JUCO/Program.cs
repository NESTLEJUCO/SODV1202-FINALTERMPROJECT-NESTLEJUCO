using System;
using System.Collections.Generic;

namespace SODV1202FinalProject
{
    // Interface
    interface IConnectFourGame
    {
        void Play();
    }

    // Abstract Player Class
    public abstract class Player
    {
        public string Name { get; }
        public char Symbol { get; }

        public Player(string name, char symbol)
        {
            Name = name;
            Symbol = symbol;
        }
    }

    // Inheritance HumanPlayer Class
    public class HumanPlayer : Player
    {
        public HumanPlayer(string name, char symbol) : base(name, symbol)
        {
        }
    }

    // Inheritance AIPlayer Class
    public class AIPlayer : Player
    {
        public AIPlayer(string name, char symbol) : base(name, symbol)
        {
        }

        public int GetMove(GameBoard gameBoard)
        {
            //AI logic to choose move
            Random random = new Random();
            int column;
            do
            {
                column = random.Next(GameBoard.Columns);
            } while (!gameBoard.IsValidMove(column));
            return column;
        }
    }


    // Game Board Class
    public class GameBoard
    {
        public const int Rows = 6;
        public const int Columns = 7;
        public char[,] board;
        private bool isPlayerOnesTurn;
        private HumanPlayer player1;
        private Player player2;

        public GameBoard()
        {
            board = new char[Rows, Columns];
            InitializeBoard();
        }

        public void InitializeBoard()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    board[row, col] = ' ';
                }
            }
        }

        public void PrintBoard()
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

        public int DropPiece(int column)

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

        public bool IsWinningMove(int row, int col)
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

        public bool IsDraw()
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

        public int GetMove()
        {
            Player currentPlayer = isPlayerOnesTurn ? player1 : player2;
            Console.WriteLine($"Player {currentPlayer.Name}'s turn, enter a column number (1-7):");
            int column = Convert.ToInt32(Console.ReadLine()) - 1;
            return column;
        }

        public bool IsValidMove(int column)
        {
            if (column < 0 || column >= Columns)
            {
                return false;
            }

            return board[0, column] == ' ';
        }

        public void SetPlayers(HumanPlayer player1, HumanPlayer player2)
        {
            this.player1 = player1;
            this.player2 = player2;
        }

        public void SetPlayerTurn(bool isPlayerOnesTurn)
        {
            this.isPlayerOnesTurn = isPlayerOnesTurn;
        }

        public void SetPlayersAI(HumanPlayer player1, AIPlayer player2)
        {
            this.player1 = player1;
            this.player2 = player2;
        }
    }

    // Two Player Human vs Human Mode Class
    class Connect4TwoPlayerHuman : IConnectFourGame
    {
        private GameBoard gameBoard;
        private bool isPlayerOnesTurn;
        private HumanPlayer player1;
        private HumanPlayer player2;

        public Connect4TwoPlayerHuman()
        {
            gameBoard = new GameBoard();
            isPlayerOnesTurn = true;
            gameBoard.InitializeBoard();
            Console.WriteLine("\nConnect 4 Two Player Human vs Human Battle Selected!\n");
            InitializePlayers();
            gameBoard.SetPlayers(player1, player2);
            gameBoard.SetPlayerTurn(isPlayerOnesTurn);
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
                gameBoard.PrintBoard();

                int column = gameBoard.GetMove();
                if (gameBoard.IsValidMove(column))
                {
                    int row = gameBoard.DropPiece(column);

                    if (gameBoard.IsWinningMove(row, column))
                    {
                        Console.Clear();
                        gameBoard.PrintBoard();
                        Console.WriteLine($"It's a connect four! Player {(isPlayerOnesTurn ? player1.Name : player2.Name)} wins!");

                        isGameOver = true;
                        if (PlayAgain())
                        {
                            isGameOver = false;
                            gameBoard.InitializeBoard();
                            gameBoard.SetPlayerTurn(true);
                        }
                    }
                    else if (gameBoard.IsDraw())
                    {
                        Console.Clear();
                        gameBoard.PrintBoard();
                        Console.WriteLine("It's a draw!");

                        isGameOver = true;
                        if (PlayAgain())
                        {
                            isGameOver = false;
                            gameBoard.InitializeBoard();
                            gameBoard.SetPlayerTurn(true);
                        }
                    }
                    else
                    {
                        isPlayerOnesTurn = !isPlayerOnesTurn;
                        gameBoard.SetPlayerTurn(isPlayerOnesTurn);
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
            if (input.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Thank you for playing!");
            }
            return false;
        }
    }

    // Two Player Human vs AI Mode Class
    class Connect4PlayerVsAI : IConnectFourGame
    {
        private GameBoard gameBoard;
        private bool isPlayerOnesTurn;
        private HumanPlayer player1;
        private AIPlayer player2;

        public Connect4PlayerVsAI()
        {
            gameBoard = new GameBoard();
            isPlayerOnesTurn = true;
            gameBoard.InitializeBoard();
            Console.WriteLine("\nConnect 4 Player vs AI Battle Selected!\n");
            InitializePlayers();
            gameBoard.SetPlayersAI(player1, player2);
            gameBoard.SetPlayerTurn(isPlayerOnesTurn);
        }

        private void InitializePlayers()
        {
            Console.Write("Enter name for Player: ");
            string playerName = Console.ReadLine();
            player1 = new HumanPlayer(playerName, 'X');
            player2 = new AIPlayer("AI", 'O');
        }

        public void Play()
        {
            bool isGameOver = false;

            while (!isGameOver)
            {
                Console.Clear();
                gameBoard.PrintBoard();

                if (isPlayerOnesTurn)
                {
                    int column = gameBoard.GetMove();
                    if (gameBoard.IsValidMove(column))
                    {
                        int row = gameBoard.DropPiece(column);

                        if (gameBoard.IsWinningMove(row, column))
                        {
                            Console.Clear();
                            gameBoard.PrintBoard();
                            Console.WriteLine($"It's a connect four! Player {player1.Name} wins!");

                            isGameOver = true;
                        }
                        else if (gameBoard.IsDraw())
                        {
                            Console.Clear();
                            gameBoard.PrintBoard();
                            Console.WriteLine("It's a draw!");

                            isGameOver = true;
                        }
                        else
                        {
                            isPlayerOnesTurn = !isPlayerOnesTurn;
                            gameBoard.SetPlayerTurn(isPlayerOnesTurn);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid move. Try again.");
                    }
                }
                else
                {
                    int column = player2.GetMove(gameBoard);
                    int row = gameBoard.DropPiece(column);

                    Console.Clear();
                    gameBoard.PrintBoard();

                    if (gameBoard.IsWinningMove(row, column))
                    {
                        Console.WriteLine($"It's a connect four! Player {player2.Name} wins!");

                        isGameOver = true;
                    }
                    else if (gameBoard.IsDraw())
                    {
                        Console.WriteLine("It's a draw!");

                        isGameOver = true;
                    }
                    else
                    {
                        isPlayerOnesTurn = !isPlayerOnesTurn;
                        gameBoard.SetPlayerTurn(isPlayerOnesTurn);
                    }
                }
            }

            if (PlayAgain())
            {
                isGameOver = false;
                gameBoard.InitializeBoard();
                gameBoard.SetPlayerTurn(true);
                Play();
            }
            else
            {
                Console.WriteLine("Thank you for playing!");
            }
        }

        private bool PlayAgain()
        {
            Console.Write("Do you want to play again? (Y/N): ");
            string input = Console.ReadLine().Trim();
            return input.Equals("Y", StringComparison.OrdinalIgnoreCase);
        }
    }


    // Game Menu Launch Class
    class Menu
    {
        public static void GameMenu()
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
                    HumanVsHuman();
                    isValidSelection = true;
                }
                else if (sel == 2)
                {
                    HumanVsAI();
                    isValidSelection = true;
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

        public static void HumanVsHuman()
        {
            Connect4TwoPlayerHuman game = new Connect4TwoPlayerHuman();
            game.Play();
        }

        public static void HumanVsAI()
        {
            Connect4PlayerVsAI game = new Connect4PlayerVsAI();
            game.Play();
        }
    }

    //Main Program Class
    class Program
    {
        static void Main(string[] args)
        {
            Menu.GameMenu();
        }
    }
}
