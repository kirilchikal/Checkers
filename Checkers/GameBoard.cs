using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class GameBoard
    {
        public const int SIZE = 8;
        public Cell[,] Board;

        public int BlackPawns;
        public int BlackKings;
        public int WhitePawns;
        public int WhiteKings;
        public int CountKingMoves;

        public void SetUpBoard()
        {
            Board = new Cell[SIZE, SIZE];

            for (int i = 0; i < SIZE; i++)
            {
                int col = i % 2 == 0 ? 1 : 0;

                Cell disc = Cell.Empty;
                if (i <= 2)
                    disc = Cell.BlackPawn;
                else if (i >= 5)
                    disc = Cell.WhitePawn;

                for (int j = col; j < SIZE; j += 2)
                {
                    Board[i, j] = disc;   
                }
            }

            BlackPawns = 12;
            WhitePawns = 12;
            BlackKings = 0;
            WhiteKings = 0;
            CountKingMoves = 0;
        }

        public (int, int) FindRemoveItem(Move move)
        {
            Cell moved = Board[move.startRow, move.startCol];
            if (moved == Cell.BlackPawn || moved == Cell.WhitePawn)
            {
                return ((move.toRow + move.startRow) / 2, (move.toCol + move.startCol) / 2);
            }

            int colChange = move.startCol > move.toCol ? -1 : 1;
            int rowChange = move.startRow > move.toRow ? -1 : 1;
            int row = move.startRow + rowChange;
            int col = move.startCol + colChange;
            while (row != move.toRow)
            {
                if (Board[row, col] != Cell.Empty)
                    return (row, col);
                row += rowChange;
                col += colChange;
            }

            return (-1, -1);
        }

        // Check if move is valid and then return game condition
        public GameCondition Move(Move move, Color side)
        {
            var possibleMoves = GetPossibleMoves(side);

            if (!possibleMoves.Contains(move))
            {
                return GameCondition.InvalidMove;
            }

            Move m = possibleMoves.Find(x => x.Equals(move));
            Cell cellType = Board[m.startRow, m.startCol];

            Board[m.startRow, m.startCol] = Cell.Empty;
            Board[m.toRow, m.toCol] = cellType;

            if (m.IsJump)
            {
                (int row, int col) = FindRemoveItem(m);

                Cell removeType = Board[row, col];
                Board[row, col] = Cell.Empty;

                switch (removeType)
                {
                    case Cell.BlackPawn:
                        BlackPawns--;
                        break;
                    case Cell.BlackKing:
                        BlackKings--;
                        break;
                    case Cell.WhitePawn:
                        WhitePawns--;
                        break;
                    case Cell.WhiteKing:
                        WhiteKings--;
                        break;
                }

                if (BlackPawns == 0 && BlackKings == 0)
                    return GameCondition.WhiteWin;
                else if (WhitePawns == 0 && WhiteKings == 0)
                    return GameCondition.BlackWin;

                // check if additioanal move nedeed
                possibleMoves = GetPawnsPossibleMoves(m.toRow, m.toCol, side);
                if (possibleMoves.Any(x => x.IsJump))
                    return GameCondition.AdditionalMove;
            }

            // check kings
            if (cellType != Cell.BlackKing && cellType != Cell.WhiteKing)
            {
                if (cellType == Cell.WhitePawn && m.toRow == 0)
                {
                    Board[m.toRow, m.toCol] = Cell.WhiteKing;
                    WhiteKings++;
                    WhitePawns--;
                }
                else if (cellType == Cell.BlackPawn && m.toRow == SIZE - 1)
                {
                    Board[m.toRow, m.toCol] = Cell.BlackKing;
                    BlackKings++;
                    BlackPawns--;
                }
            }

            if (WhitePawns == 0 && BlackPawns == 0)
            {
                if (CountKingMoves == 15)
                    return GameCondition.Draw;
            }

            return GameCondition.NextMove;
        }


        public GameCondition AIMove(Move move, Color side)
        {
            GameCondition condition = Move(move, side);

            while (condition is GameCondition.AdditionalMove)
            {
                condition = Move(GetPossibleMoves(side)[0], side);
            }

            return condition;
        }

        public List<Move> GetPossibleMoves(Color side)
        {
            List<Move> moves = new List<Move>();
            Cell pawnColor = side == Color.Black ? Cell.BlackPawn : Cell.WhitePawn;
            Cell kingColor = side == Color.Black ? Cell.BlackKing : Cell.WhiteKing;

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    Cell currentCell = Board[i, j];
                    if (currentCell == pawnColor)
                        moves.AddRange(GetPawnsPossibleMoves(i, j, side));
                    else if (currentCell == kingColor)
                        moves.AddRange(GetKingsPossibleMoves(i, j, side));
                }
            }

            var x = moves.FindAll(m => m.IsJump);
            return x.Count == 0 ? moves : x;
        }

        public List<Move> GetPawnsPossibleMoves(int row, int col, Color side)
        {
            //Separate empty move VS eating moves and return only the best moves (if eats not empty >> only eats;;)

            List<Move> moves = new List<Move>();
            int change = side == Color.Black ? -1 : 1;

            //look back
            int checkRow = row + change;
            if(checkRow >= 0 && checkRow < SIZE)
            {
                int checkCol = col + 1;
                if (checkCol < SIZE && IsOppositePiece(checkRow, checkCol, side))
                {
                    if (IsEmptyCell(checkRow + change, checkCol + 1))
                    {
                        moves.Add(new Move(row, col, checkRow + change, checkCol + 1) { IsJump = true });
                    }
                }

                checkCol = col - 1;
                if (checkCol >= 0 && IsOppositePiece(checkRow, checkCol, side))
                {
                    if (IsEmptyCell(checkRow + change, checkCol - 1))
                    {
                        moves.Add(new Move(row, col, checkRow + change, checkCol - 1) { IsJump = true });
                    }
                }
            }

            //look ahead
            checkRow = row - change;
            if (checkRow >= 0 && checkRow < SIZE)
            {
                int checkCol = col + 1;
                if (checkCol < SIZE)
                {
                    if (IsOppositePiece(checkRow, checkCol, side) && IsEmptyCell(checkRow - change, checkCol + 1))
                        moves.Add(new Move(row, col, checkRow - change, checkCol + 1) { IsJump = true });
                    else if (IsEmptyCell(checkRow, checkCol))
                        moves.Add(new Move(row, col, checkRow, checkCol));
                }

                checkCol = col - 1;
                if (checkCol >= 0)
                {
                    if (IsOppositePiece(checkRow, checkCol, side) && IsEmptyCell(checkRow - change, checkCol - 1))
                        moves.Add(new Move(row, col, checkRow - change, checkCol - 1) { IsJump = true });
                    else if (IsEmptyCell(checkRow, checkCol))
                        moves.Add(new Move(row, col, checkRow, checkCol));
                }
            }

            return moves;
        }

        public List<Move> GetKingsPossibleMoves(int row, int col, Color side)
        {
            List<Move> moves = new List<Move>();
            moves.AddRange(GetKingsPossibleMoves(row, col, side, true, true));
            moves.AddRange(GetKingsPossibleMoves(row, col, side, true, false));
            moves.AddRange(GetKingsPossibleMoves(row, col, side, false, true));
            moves.AddRange(GetKingsPossibleMoves(row, col, side, false, false));

            return moves;
        }

        public List<Move> GetKingsPossibleMoves(int row, int col, Color side, bool back, bool right)
        {
            List<Move> moves = new List<Move>();
            int change = side == Color.Black ? -1 : 1;
            int checkRow = back ? row + change : row - change;
            int checkCol = right ? col + 1 : col - 1;

            while (checkRow >= 0 && checkRow < SIZE && checkCol < SIZE && checkCol >= 0)
            {
                if (IsEmptyCell(checkRow, checkCol))
                {
                    moves.Add(new Move(row, col, checkRow, checkCol));
                    checkRow = back ? checkRow + change : checkRow - change;
                    checkCol = right ? checkCol + 1 : checkCol - 1;
                }
                else if (IsOppositePiece(checkRow, checkCol, side) && IsEmptyCell(back ? checkRow + change : checkRow - change, right ? checkCol + 1 : checkCol - 1))
                {
                    moves.Add(new Move(row, col, back ? checkRow + change : checkRow - change, right ? checkCol + 1 : checkCol - 1) { IsJump = true });
                    break;
                }
                else break;

            }

            return moves;
        }


        public bool IsOppositePiece(int row, int col, Color side)
        {
            Cell cell = Board[row, col];
            if (cell == Cell.Empty)
                return false;
            
            if (side == Color.Black)
            {
                if (cell == Cell.WhitePawn || cell == Cell.WhiteKing)
                    return true;
            }
            else if (side == Color.White)
            {
                if (cell == Cell.BlackPawn || cell == Cell.BlackKing)
                    return true;
            }

            return false;
        }

        public bool IsEmptyCell(int row, int col)
        {
            if (row >= 0 && row < SIZE && col >= 0 && col< SIZE)
            {
                return Board[row, col] == Cell.Empty;
            }

            return false;
        }


        /* 
             BOARD ESTIMATION HEURISTICS
        */

        // Kings weigth
        public double EstimateBoard2(Color side)
        {
            double weight = 2;

            if (side == Color.White)
            {
                return WhiteKings * weight + WhitePawns - BlackKings * weight - BlackPawns;
            }

            return BlackKings * weight + BlackPawns - WhiteKings * weight - WhitePawns;
        }

        // Board position weigth
        public double EstimateBoard(Color side)
        {
            double WhiteResult = 0;
            double BlackResult = 0;

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    switch(Board[i, j])
                    {
                        case Cell.BlackPawn:
                            BlackResult += i >= 4 ? 7 : 5;  // opponent side 7
                            if(side == Color.Black && (j == 0 ||  j == 7)) BlackResult += 3;    // +3 if on edge
                            break;
                        case Cell.WhitePawn:
                            WhiteResult += i <= 3 ? 7 : 5;
                            if (side == Color.White && (j == 0 || j == 7)) WhiteResult += 3;
                            break;
                        case Cell.BlackKing:
                            BlackResult += 10;
                            break;
                        case Cell.WhiteKing:
                            WhiteResult += 10;
                            break;
                    }
                }
            }

            if (side == Color.White)
            {
                return WhiteResult - BlackResult;
            }

            return BlackResult - WhiteResult;
        }


        public GameBoard CloneBoard()
        {
            Cell[,] clone = (Cell[,]) Board.Clone();

            return new GameBoard() { Board = clone, BlackKings = BlackKings, BlackPawns = BlackPawns, WhiteKings = WhiteKings, WhitePawns = WhitePawns, CountKingMoves = CountKingMoves };
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append("  ");
            for (int i = 0; i < SIZE; i++)
            {
                b.Append(i + " ");
            }
            b.Append("\n");
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = -1; j < SIZE; j++)
                {
                    String row = "";
                    if (j == -1)
                        row = i + "";
                    else if (Board[i, j] == Cell.WhitePawn)
                        row = "w";
                    else if (Board[i, j] == Cell.BlackPawn)
                        row = "b";
                    else if (Board[i, j] == Cell.WhiteKing)
                        row = "W";
                    else if (Board[i, j] == Cell.BlackKing)
                        row = "B";
                    else
                        row = "\u00B7";

                    b.Append(row);
                    b.Append(" ");
                }
                b.Append("\n");
            }
            return b.ToString();
        }
    }
}
