using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal abstract class GameObject
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public GameObject(string name, string symbol)
        {
            Name = name;
            Symbol = symbol;
        }

        public void Move(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
