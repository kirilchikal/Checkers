using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public enum GameCondition
    {
        InvalidMove,
        AdditionalMove,
        NextMove,
        Draw,
        WhiteWin,
        BlackWin
    }
}
