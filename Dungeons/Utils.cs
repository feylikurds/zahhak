/*
Dungeons, a C# 6.0 coding example in form of a console game.
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

namespace Dungeons
{
    internal static class Utils
    {

        private static Random random = new Random();
        private static object syncLock = new object();

        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            {
                return random.Next(min, max);
            }
        }

        public static Tuple<int, int> MoveRandomly(int currentX, int currentY)
        {
            var x = currentX;
            var y = currentY;

            var n = RandomNumber(0, 100);

            if (n > 50)
            {
                n = RandomNumber(0, 100);

                if (n > 50)
                    x += -1;
                else
                    x += 1;
            }
            else
            {
                n = RandomNumber(0, 100);

                if (n > 50)
                    y += -1;
                else
                    y += 1;
            }

            return Tuple.Create(x, y);
        }


        public static Tuple<int, int> GetMove(ConsoleKeyInfo key)
        {
            var x = 0;
            var y = 0;

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    y = -1;
                    break;

                case ConsoleKey.DownArrow:
                    y = 1;
                    break;

                case ConsoleKey.LeftArrow:
                    x = -1;
                    break;

                case ConsoleKey.RightArrow:
                    x = 1;
                    break;

                default:
                    break;
            }

            return Tuple.Create(x, y);
        }
    }
}
