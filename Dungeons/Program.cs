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
using System.Threading;
using System.Timers;

namespace Dungeons
{
    class Program
    {
        static Game game;
        static ConsoleKeyInfo key;

        static bool play = true;
        static bool quit = false;

        static void Main(string[] args)
        {
            game = new Game(20, 20, 2, 10, 10, 10);

            game.Start();

            System.Timers.Timer keyTimer = new System.Timers.Timer();
            keyTimer.Elapsed += new ElapsedEventHandler(keyOnTimedEvent);
            keyTimer.Interval = 100;
            keyTimer.Enabled = true;

            System.Timers.Timer gameTimer = new System.Timers.Timer();
            gameTimer.Elapsed += new ElapsedEventHandler(playOnTimedEvent);
            gameTimer.Interval = 100;
            gameTimer.Enabled = true;

            while (play && !quit)
            {
                Thread.Sleep(100);
            }

            Console.Clear();

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Dungeons by Aryo Pehlewan feylikurds@gmail.com Copyright 2016 License GPLv3");

            Console.WriteLine(Environment.NewLine);

            Console.WriteLine(@"
 ____                                     _____                          
/\  _`\                                  /\  __`\                        
\ \ \L\_\     __      ___ ___      __    \ \ \/\ \  __  __    __   _ __  
 \ \ \L_L   /'__`\  /' __` __`\  /'__`\   \ \ \ \ \/\ \/\ \ /'__`\/\`'__\
  \ \ \/, \/\ \L\.\_/\ \/\ \/\ \/\  __/    \ \ \_\ \ \ \_/ /\  __/\ \ \/ 
   \ \____/\ \__/.\_\ \_\ \_\ \_\ \____\    \ \_____\ \___/\ \____\\ \_\ 
    \/___/  \/__/\/_/\/_/\/_/\/_/\/____/     \/_____/\/__/  \/____/ \/_/ ");

            Console.WriteLine(Environment.NewLine);
        }

        private static void keyOnTimedEvent(object source, ElapsedEventArgs e)
        {
            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Q)
                quit = true;
        }

        private static void playOnTimedEvent(object source, ElapsedEventArgs e)
        {
            play = game.Play(key);
        }
    }
}
