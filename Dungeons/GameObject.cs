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
        public bool Remove = false;
        public ConsoleColor Color { get; set; }

        public GameObject(string name, string symbol, ConsoleColor color = ConsoleColor.White)
        {
            Name = name;
            Symbol = symbol;
            Color = color;
        }

        public void Move(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Delete(string symbol)
        {
            Remove = true;
            Symbol = symbol;
        }
    }
}
