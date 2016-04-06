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
using System.Reactive.Linq;
using System.Diagnostics;

namespace Zahhak
{
    class Program
    {
        static private int keyTimerInterval = 100;
        static private int gameTimerInterval = 200;
        static private int threadSleep = 100;

        static private OrderedDictionary options = new OrderedDictionary();

        static private Game game;
        static private Tuple<bool, bool> result;
        static private ConsoleKey key;

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

            var keyTimer = new System.Timers.Timer();
            keyTimer.Elapsed += new ElapsedEventHandler(keyOnTimedEvent);
            keyTimer.Interval = (int)options["keyTimerInterval"];
            keyTimer.Enabled = true;

			var gameTimer = Observable.Interval(TimeSpan.FromMilliseconds((int)options["gameTimerInterval"]));
			var gameSub = gameTimer.Subscribe(tick => result = game.Play(key));

			while (play && !quit && !won) 
			{
				Thread.Sleep ((int)options ["threadSleep"]);

				if (result == null)
					continue;

				play = result.Item1;
				won = result.Item2;
			}

            keyTimer.Elapsed -= keyOnTimedEvent;
			gameSub.Dispose();

            end();
        }

        private static bool cmd(string[] args)
        {

            options.Add("difficulty", Game.DIFFICULTY);
            options.Add("worldWidth", Game.WIDTH);
            options.Add("worldHeight", Game.HEIGHT);
            options.Add("numMonsters", Game.NUM_MONSTERS);
            options.Add("numHealth", Game.NUM_HEALTH);
            options.Add("numStrength", Game.NUM_STRENGTH);
            options.Add("numTreasure", Game.NUM_TREASURE);
            options.Add("capacity", Game.CAPACITY);
            options.Add("keyTimerInterval", keyTimerInterval);
            options.Add("gameTimerInterval", gameTimerInterval);
            options.Add("threadSleep", threadSleep);

            if (args.Length == 1 && args[0] == "help")
                return false;

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
            Console.WriteLine(Environment.NewLine + "Zahhak by Aryo Pehlewan feylikurds@gmail.com Copyright 2016 License GPLv3");

            Console.WriteLine(@"
Zahhak [difficulty] [worldWidth] [worldHeight] [numMonsters] [numHealth] [numStrength] [numTreasure] [capacity] [keyTimer] [gameTimer] [threadSleep]

 difficulty = (default 30)  The level of difficulty from 1 to 100.
 worldWidth = (default 20)  How wide to make the world.
worldHeight = (default 20)  How high to make the world.
numMonsters = (default 10)  How many monsters to make.
  numHealth = (default 10)  How many healths to make.
numStrength = (default 10)  How many strengths to make.
numTreasure = (default 10)  How many treasures to make.
   capacity = (default 2)   How many objects can a room contain.
   keyTimer = (default 100) The interval in milliseconds to capture a key.
  gameTimer = (default 200) The interval in milliseconds to play the game.
threadSleep = (default 100) The interval in milliseconds to check the program's status.

type 'Zahhak help' to come back to this screen.
");
        }

        private static void keyOnTimedEvent(object source, ElapsedEventArgs e)
        {
			if (!Console.KeyAvailable)
				return;
			
			key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.Q)
                quit = true;
        }

        private static void end()
        {
            Console.Clear();
            Console.ResetColor();

            Console.WriteLine(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine + "Zahhak by Aryo Pehlewan feylikurds@gmail.com Copyright 2016 License GPLv3");
            Console.WriteLine();

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
            Console.WriteLine();
        }
    }
}
