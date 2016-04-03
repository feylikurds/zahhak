using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Player : Creature
    {
        protected const string NAME = "Player";
        protected const string SYMBOL = "P";

        public Player(string name = NAME, string symbol = SYMBOL) : base(name, symbol) { }
    }
}