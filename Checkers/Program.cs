using System;

namespace Checkers
{
    class Program
    {
        static void Main(string[] args)
        {
            Game checkers = new Game();
            checkers.Start();



            //GameBoard board = new GameBoard();
            //board.SetUpBoard();

            //board.Board[3, 4] = Cell.WhiteDisc;
            //board.Board[5, 6] = Cell.BlackKing;
            //board.Board[2, 5] = Cell.Empty;
            //board.Board[2, 3] = Cell.Empty;
            //board.Board[3, 7] = Cell.Empty;
            //Console.WriteLine(" \n" + board);

            //Move mo = new Move(3, 4, 2, 5);
            //Console.WriteLine(board.IsValidMove(mo, Color.White));



            //foreach (var m in board.GetPossibleMoves(Color.White))
            //{
            //    Console.WriteLine(m);
            //}
        }
    }
}
