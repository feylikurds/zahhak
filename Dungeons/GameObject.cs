/*
Dungeons, a C# 6.0 coding example in form of a console game.
Copyright (C) 2016 Aryo Pehlewan feylikurds@gmail.com

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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
