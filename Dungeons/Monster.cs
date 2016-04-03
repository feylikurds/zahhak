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

        public Monster(string name = NAME, string symbol = SYMBOL) : base(name, symbol) { }
    }
}
