using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Monster : Creature
    {
        public Monster(string name = "Monster", string symbol = "M") : base(name, symbol) { }
    }
}
