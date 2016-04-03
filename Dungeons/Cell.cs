using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Cell
    {
        public Pixel[] Pixels;
        
        public Cell(int capacity)
        {
            Pixels = new Pixel[capacity];
        }
    }
}
