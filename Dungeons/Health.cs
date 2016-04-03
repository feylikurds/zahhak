using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Health : Item
    {
        protected const string NAME = "Health";
        protected const string SYMBOL = "H";

        public Health(string name = NAME, string symbol = SYMBOL) : base(name, symbol) { }
    }
}
