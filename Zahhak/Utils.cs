/*
Zahhak, a C# 6.0 coding example in form of a console game.
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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zahhak
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

        public static Tuple<int, int> MoveToPlayer(Monster monster, Player player)
        {
            var deltaX = monster.X - player.X;
            var deltaY = monster.Y - player.Y;
            var x = 0;
            var y = 0;

            if (deltaX < 0)
                x = 1;
            else if (deltaX > 0)
                x = -1;

            if (deltaY < 0)
                y = 1;
            else if (deltaY > 0)
                y = -1;

            x += monster.X;
            y += monster.Y;

            return Tuple.Create(x, y);
        }

        public static Tuple<int, int> MoveRandomly(Monster monster)
        {
            var x = monster.X;
            var y = monster.Y;

            if (flip())
            {
                if (flip())
                    x += -1;
                else
                    x += 1;
            }
            else
            {
                if (flip())
                    y += -1;
                else
                    y += 1;
            }

            return Tuple.Create(x, y);
        }

        public static bool flip(int bias = 50)
        {
            var n = RandomNumber(1, 100);

            if (n >= bias)
                return true;

            return false;
        }

        public static Tuple<int, int> GetMove(ConsoleKey key)
        {
            var x = 0;
            var y = 0;

            switch (key)
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
