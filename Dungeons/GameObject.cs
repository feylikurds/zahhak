using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal abstract class GameObject
    {
        public string Name;
        public string Symbol;
        public int X;
        public int Y;
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
