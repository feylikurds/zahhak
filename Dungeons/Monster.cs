using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Monster : Creature
    {
        protected const string NAME = "Monster";
        protected const string SYMBOL = "M";
        protected const ConsoleColor COLOR = ConsoleColor.Red;

        public Monster(string name = NAME, string symbol = SYMBOL, ConsoleColor color = COLOR) : base(name, symbol, color) { }
    }
}
