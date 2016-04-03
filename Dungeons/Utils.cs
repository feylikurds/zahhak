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
