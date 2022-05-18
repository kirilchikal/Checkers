using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class AIPlayer : PlayerBase
    {
        public int Level;

        public List<int> Nodes;
        public List<long> SearchTime;
        public System.Diagnostics.Stopwatch watch;

        private int NodesNum;

        public AIPlayer(string name, Color side, int level) : base(name, side)
        {
            this.Level = level;
            Nodes = new List<int>();
            SearchTime = new List<long>();
            watch = new System.Diagnostics.Stopwatch();
        }

        public Move GetMove(GameBoard board)
        {
            // start whatch
            watch.Start();

            Move foundMove = MiniMax(board);

            watch.Stop();
            SearchTime.Add(watch.ElapsedMilliseconds);
            Nodes.Add(NodesNum);
            NodesNum = 0;

            return foundMove;
        }

        public Move MiniMax(GameBoard board)
        {
            double alfa = double.MinValue;
            double beta = double.MaxValue;

            List<Move> moves = board.GetPossibleMoves(this.Side);
            List<double> estimations = new List<double>();
            GameBoard clone;

            for (int i = 0; i < moves.Count; i++)
            {
                clone = board.CloneBoard();
                clone.AIMove(moves[i], this.Side);
                //estimations.Add(MiniMax(clone, ChangeSide(this.Side), Level - 1, false));
                estimations.Add(MiniMaxAlphaBeta(clone, ChangeSide(this.Side), Level - 1, alfa, beta, false));
            }

            double maxValue = estimations.Max();
            //in case there are multipal max select random
            List<int> maxIndexes = new List<int>();
            for (int i = 0; i < estimations.Count; i++)
            {
                if (estimations[i] == maxValue)
                    maxIndexes.Add(i);
            }

            int index = maxIndexes.OrderBy(i => Guid.NewGuid()).ToArray()[0];

            return moves[index];
        }


        // Simple minimax with depth 
        public double MiniMax(GameBoard board, Color side, int level, bool isMax)
        {
            NodesNum++;
            if (level == 0)
            {
                return board.EstimateBoard(this.Side);
            }

            List<Move> moves = board.GetPossibleMoves(side);
            if (moves.Count == 0)
                return board.EstimateBoard(this.Side);

            GameBoard clone;

            if (isMax)
            {
                double max = double.MinValue;
                for (int i = 0; i < moves.Count; i++)
                {
                    clone = board.CloneBoard();
                    clone.AIMove(moves[i], side);
                    max = Math.Max(max, MiniMax(clone, ChangeSide(side), level - 1, false));
                }
                return max;
            }
            else
            {
                double min = double.MaxValue;
                for (int i = 0; i < moves.Count; i++)
                {
                    clone = board.CloneBoard();
                    clone.AIMove(moves[i], side);
                    min = Math.Min(min, MiniMax(clone, ChangeSide(side), level - 1, true));
                }
                return min;
            }
        }

        // mini-max algorithm with alpha and beta cuts operation
        public double MiniMaxAlphaBeta(GameBoard board, Color side, int level, double alfa, double beta, bool isMax)
        {
            NodesNum++;
            if (level == 0)
                return board.EstimateBoard(this.Side);


            List<Move> moves = board.GetPossibleMoves(side);
            if (moves.Count == 0)
                return board.EstimateBoard(this.Side);

            GameBoard clone = board.CloneBoard();

            if (isMax)
            {
                double value = double.MinValue;
                for (int i = 0; i < moves.Count; i++)
                {
                    clone.AIMove(moves[i], side);
                    value = Math.Max(value, MiniMaxAlphaBeta(clone, ChangeSide(side), level - 1, alfa, beta, false));
                    if (value >= beta)
                        break;
                    alfa = Math.Max(alfa, value);
                }
                return value;
            }
            else
            {
                double value = double.MaxValue;
                for (int i = 0; i < moves.Count; i++)
                {
                    clone.AIMove(moves[i], side);
                    value = Math.Min(value, MiniMaxAlphaBeta(clone, ChangeSide(side), level - 1, alfa, beta, true));
                    if (value <= alfa)
                        break;
                    beta = Math.Min(beta, value);
                }
                return value;
            }
        }

        public Color ChangeSide(Color side)
        {
            return side == Color.Black ? Color.White : Color.Black;
        }
    }
}
