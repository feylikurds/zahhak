using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    abstract class Item : GameObject
    {
        public Item(string name, string symbol) : base(name, symbol) {}

        public abstract void PickUp(Player player);
    }
}
