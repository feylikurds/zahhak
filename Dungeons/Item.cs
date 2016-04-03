using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal abstract class Item : GameObject
    {
        const int AMOUNT = 10;

        public readonly int Amount;

        public Item(string name, string symbol, int amount = AMOUNT) : base(name, symbol)
        {
            Amount = amount;
        }
    }
}
