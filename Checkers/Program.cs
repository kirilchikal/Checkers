using System;

namespace Checkers
{
    class Program
    {
        static void Main(string[] args)
        {
            Game checkers = new Game();
            checkers.Start();



            //for (int i = 0; i < 5; i++)
            //{
            //    checkers.Start();
            //}

            //Console.WriteLine("b: " + checkers.b + "   w: " + checkers.w + "   rem: "+ checkers.rem);


            //GameBoard board = new GameBoard();
            //board.SetUpBoard();

            //board.Board[3, 4] = Cell.WhiteDisc;
            //board.Board[5, 6] = Cell.BlackKing;
            //board.Board[2, 5] = Cell.Empty;
            //board.Board[2, 3] = Cell.Empty;
            //board.Board[3, 7] = Cell.Empty;
            //Console.WriteLine(" \n" + board);

            //GameBoard clone = board.CloneBoard();

            //Move mo = new Move(6, 5, 4, 7);
            //Console.WriteLine(clone.Move(mo, Color.White));
            //Console.WriteLine("\n clone board:\n" + clone);
            //Console.WriteLine(" \n" + board);



            //foreach (var m in board.GetPossibleMoves(Color.White))
            //{
            //    Console.WriteLine(m);
            //}
        }
    }
}
