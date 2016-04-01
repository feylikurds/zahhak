using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    abstract class GameObject
    {
        public string Name { get; set; }
        public string Symbol { get; set; }

        public GameObject(string name, string symbol)
        {
            Name = name;
            Symbol = symbol;
        }
    }
}
