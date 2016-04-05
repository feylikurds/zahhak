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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Collections.Specialized;

namespace Zahhak
{
    class Program
    {
        static private readonly int difficulty = 30;
        static private readonly int worldWidth = 20;
        static private readonly int worldHeight = 20;
        static private readonly int numMonsters = 10;
        static private readonly int numHealth = 10;
        static private readonly int numStrength = 10;
        static private readonly int numTreasure = 10;
        static private readonly int capacity = 2;
        static private readonly int keyTimerInterval = 100;
        static private readonly int gameTimerInterval = 100;
        static private readonly int threadSleep = 100;

        static private OrderedDictionary options = new OrderedDictionary();

        static private Game game;
        static private Tuple<bool, bool> result;
        static private ConsoleKeyInfo key;

        static private bool play = true;
        static private bool quit = false;
        static private bool won = false;

        static void Main(string[] args)
        {
            if (!cmd(args))
            {
                help();

                return;
            }

            Console.Clear();
            Console.ResetColor();

            game = new Game((int)options["difficulty"], (int)options["worldWidth"], (int)options["worldHeight"], (int)options["numMonsters"], (int)options["numHealth"], (int)options["numStrength"], (int)options["numTreasure"], (int)options["capacity"]);

            game.Start();

            System.Timers.Timer keyTimer = new System.Timers.Timer();
            keyTimer.Elapsed += new ElapsedEventHandler(keyOnTimedEvent);
            keyTimer.Interval = (int)options["keyTimerInterval"];
            keyTimer.Enabled = true;

            System.Timers.Timer gameTimer = new System.Timers.Timer();
            gameTimer.Elapsed += new ElapsedEventHandler(playOnTimedEvent);
            gameTimer.Interval = (int)options["gameTimerInterval"];
            gameTimer.Enabled = true;

            while (play && !quit && !won)
            {
                Thread.Sleep((int)options["threadSleep"]);
            }

            end();
        }

        private static bool cmd(string[] args)
        {
            options.Add("difficulty", difficulty);
            options.Add("worldWidth", worldWidth);
            options.Add("worldHeight", worldHeight);
            options.Add("numMonsters", numMonsters);
            options.Add("numHealth", numHealth);
            options.Add("numStrength", numStrength);
            options.Add("numTreasure", numTreasure);
            options.Add("capacity", capacity);
            options.Add("keyTimerInterval", keyTimerInterval);
            options.Add("gameTimerInterval", gameTimerInterval);
            options.Add("threadSleep", threadSleep);

            if (args.Length == 1 && args[0] == "help")
            {
                return false;
            }

            string[] keys = new string[options.Count];
            options.Keys.CopyTo(keys, 0);

            for (int i = 0; i < args.Length && i < options.Count; i++)
            {
                string arg = args[i];
                int val = 0;
                bool valid = int.TryParse(arg, out val);

                if (valid)
                    options[keys[i]] = val;
                else
                    return false;
            }

            return true;
        }

        static void help()
        {
            Console.WriteLine();

            Console.WriteLine("Zahhak by Aryo Pehlewan feylikurds@gmail.com Copyright 2016 License GPLv3");

            Console.WriteLine(@"
zahhak [difficulty] [worldWidth] [worldHeight] [numMonsters] [numHealth] [numStrength] [numTreasure] [capacity] [keyTimer] [gameTimer] [threadSleep]

 difficulty = (default 30)  The level of difficulty from 1 to 100.
 worldWidth = (default 20)  How wide to make the world.
worldHeight = (default 20)  How high to make the world.
numMonsters = (default 10)  How many monsters to make.
  numHealth = (default 10)  How many healths to make.
numStrength = (default 10)  How many strengths to make.
numTreasure = (default 10)  How many treasures to make.
   capacity = (default 2)   How many objects can a room contain.
   keyTimer = (default 100) The interval in milliseconds to capture a key.
  gameTimer = (default 100) The interval in milliseconds to play the game.
threadSleep = (default 100) The interval in milliseconds to check the program's status.

type 'zahhak help' to come back to this screen.
");
        }

        private static void keyOnTimedEvent(object source, ElapsedEventArgs e)
        {
            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Q)
                quit = true;
        }

        private static void playOnTimedEvent(object source, ElapsedEventArgs e)
        {
            result = game.Play(key);
            play = result.Item1;
            won = result.Item2;
        }

        private static void end()
        {
            Console.Clear();
            Console.ResetColor();

            Console.WriteLine(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Zahhak by Aryo Pehlewan feylikurds@gmail.com Copyright 2016 License GPLv3");
            Console.WriteLine(Environment.NewLine);

            if (won)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"
 __    __                                                  __     
/\ \  /\ \                                                /\ \    
\ `\`\\/'/ ___   __  __      __  __  __    ___     ___    \ \ \   
 `\ `\ /' / __`\/\ \/\ \    /\ \/\ \/\ \  / __`\ /' _ `\   \ \ \  
   `\ \ \/\ \L\ \ \ \_\ \   \ \ \_/ \_/ \/\ \L\ \/\ \/\ \   \ \_\ 
     \ \_\ \____/\ \____/    \ \___x___/'\ \____/\ \_\ \_\   \/\_\
      \/_/\/___/  \/___/      \/__//__/   \/___/  \/_/\/_/    \/_/
");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"
 ____                                     _____                          
/\  _`\                                  /\  __`\                        
\ \ \L\_\     __      ___ ___      __    \ \ \/\ \  __  __    __   _ __  
 \ \ \L_L   /'__`\  /' __` __`\  /'__`\   \ \ \ \ \/\ \/\ \ /'__`\/\`'__\
  \ \ \/, \/\ \L\.\_/\ \/\ \/\ \/\  __/    \ \ \_\ \ \ \_/ /\  __/\ \ \/ 
   \ \____/\ \__/.\_\ \_\ \_\ \_\ \____\    \ \_____\ \___/\ \____\\ \_\ 
    \/___/  \/__/\/_/\/_/\/_/\/_/\/____/     \/_____/\/__/  \/____/ \/_/ 
");
            }

            Console.ResetColor();
            Console.WriteLine(Environment.NewLine);
        }
    }
}
