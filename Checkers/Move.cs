using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Move
    {
        public int startRow;
        public int startCol;
        public int toRow;
        public int toCol;

        public bool IsJump = false;

        public Move(int fromRow, int fromCol, int toRow, int toCol)
        {
            startRow = fromRow;
            startCol = fromCol;
            this.toRow = toRow;
            this.toCol = toCol;
        }

        public override string ToString()
        {
            return $"From: [{startRow},{startCol}] To: [{toRow},{toCol}]";
        }

        public override bool Equals(object obj)
        {
            Move m = (Move) obj;
            return startRow == m.startRow && startCol == m.startCol && toRow == m.toRow && toCol == m.toCol;
        }
    }
}
