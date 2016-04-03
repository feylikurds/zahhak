using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

namespace Dungeons
{
    class Program
    {
        static Game game;
        static ConsoleKeyInfo key;

        static void Main(string[] args)
        {
            game = new Game();

            game.Start();

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 1000;
            timer.Enabled = true;

            key = Console.ReadKey();
            while (key.KeyChar != 'q')
                key = Console.ReadKey();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            game.Play();
        }
    }
}
