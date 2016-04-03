using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Strength : Item
    {
        protected const string NAME = "Strength";
        protected const string SYMBOL = "S";
        protected const ConsoleColor COLOR = ConsoleColor.Cyan;

        public Strength(string name = NAME, string symbol = SYMBOL, ConsoleColor color = COLOR) : base(name, symbol, color) { }
    }
}
