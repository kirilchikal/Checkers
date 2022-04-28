using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public abstract class PlayerBase
    {
        public String Name;
        public Color Side;

        public PlayerBase(String name, Color color)
        {
            this.Name = name;
            this.Side = color;
        }

        public override string ToString()
        {
            return this.Name;
        }

        // virtual methods
    }

    public enum Color
    {
        Black,
        White
    }
}
