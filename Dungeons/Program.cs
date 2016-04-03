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
        static bool exit;

        static void Main(string[] args)
        {
            game = new Game();

            game.Start();

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 500;
            timer.Enabled = true;

            key = new ConsoleKeyInfo();
            exit = false;

            do
            {
                while (Console.KeyAvailable == false)
                    Thread.Sleep(100);

                key = Console.ReadKey(true);
            } while (!exit && key.Key != ConsoleKey.Q);
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            exit = game.Play(key);
        }
    }
}
