using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Game
    {
        public void Start()
        {
            // Initialize players (hum-hum;hum-ai;ai-ai)
            Player player1 = new Player("Human 1", Color.White);
            Player player2 = new Player("Human 2", Color.Black);

            // Initialize and set up game board
            GameBoard board = new GameBoard();
            board.SetUpBoard();

            // First move should be done by player with white pawns
            PlayerBase currentPlayer = player1;
            bool whiteTurn = true;
            bool continueGame = true;

            Console.WriteLine($"Start game\n".PadLeft(15).ToUpper() + board.ToString());
            
            while(continueGame)
            {
                Move move;
                GameCondition condition;
                var possibleMoves = board.GetPossibleMoves(currentPlayer.Side);
                
                // if there is no move - game ended
                if (possibleMoves.Count == 0)
                {
                    if (whiteTurn)
                        Console.WriteLine($"Player {player2.Name} win. (Black side)");
                    else Console.WriteLine($"Player {player1.Name} win. (White side)");
                    break;
                }

                if (currentPlayer is AIPlayer)
                {
                    //minimax -> move
                    //board.AIMove(move);
                    condition = GameCondition.NextMove;
                }
                else
                {
                    Console.WriteLine($"{currentPlayer.Name}'s turn:");
                    var cr = Console.ReadLine();
                    while (cr == "moves")
                    {
                        PrintPossibleMoves(possibleMoves, currentPlayer.Side);
                        cr = Console.ReadLine();
                    }

                    var text = cr.Split(' ');
                    move = new Move(int.Parse(text[0]), int.Parse(text[1]), int.Parse(text[2]), int.Parse(text[3]));
                    Console.WriteLine(move);
                    condition = board.Move(move, currentPlayer.Side);
                }

                // check game condition
                switch (condition)
                {
                    case GameCondition.InvalidMove:
                        Console.WriteLine("Invalid move entered.");
                        break;
                    case GameCondition.AdditionalMove:
                        Console.WriteLine($"Additional {currentPlayer.Name} move:");
                        break;
                    case GameCondition.NextMove:
                        // change currentPlayer
                        if (whiteTurn)
                        {
                            currentPlayer = player2;
                            whiteTurn = false;
                        }
                        else
                        {
                            currentPlayer = player1;
                            whiteTurn = true;
                        }
                        Console.WriteLine(board.ToString());
                        break;
                    case GameCondition.Draw:
                        Console.WriteLine("The game ended in a draw");
                        continueGame = false;
                        break;
                    default:
                        Console.WriteLine("The game ended");
                        if (condition == GameCondition.BlackWin)
                            Console.WriteLine($"Player {currentPlayer.Name} win. (Black side)");
                        else Console.WriteLine($"Player {currentPlayer.Name} win. (White side)");
                        continueGame = false;
                        break;
                }
                
            }
        }

        private void PrintPossibleMoves(List<Move> moves, Color side)
        {
            foreach (var m in moves)
            {
                Console.WriteLine(m);
            }
        }
    }
}
