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
    internal class Treasure : Item
    {
        const int POINTS = 1;

        protected const string NAME = "Treasure";
        protected const string SYMBOL = "T";
        protected const ConsoleColor COLOR = ConsoleColor.Blue;

        public Treasure(string name = NAME, string symbol = SYMBOL, ConsoleColor color = COLOR, int points = POINTS) : base(name, symbol, color, points) { }
    }
}
