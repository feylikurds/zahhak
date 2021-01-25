/*
Zahhak, a C# 6.0 coding example in form of a console game.
Copyright (C) 2016 Aryo Pehlewan aryopehlewan@hotmail.com

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

namespace Zahhak
{
    internal class Strength : Item
    {
        protected const int POINTS = 1;

        private const string NAME = "Strength";
        private const string SYMBOL = "S";
        private const ConsoleColor COLOR = ConsoleColor.Cyan;

        public Strength(string name = NAME, string symbol = SYMBOL, ConsoleColor color = COLOR, int points = POINTS) : base(name, symbol, color, points) { }
    }
}
