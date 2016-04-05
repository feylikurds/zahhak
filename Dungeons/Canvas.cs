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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Canvas
    {
        private readonly int worldHeight;
        private readonly int worldWidth;
        private readonly int menuWidth;
        private readonly int menuHeight;
        private Cell[,] screen;
        private readonly int capacity;

        public Canvas(int worldHeight, int worldWidth, int menuWidth, int menuHeight, int capacity)
        {
            this.worldHeight = worldHeight;
            this.worldWidth = worldWidth;
            this.menuWidth = menuWidth;
            this.menuHeight = menuHeight;
            this.capacity = capacity;

            screen = new Cell[worldWidth + menuWidth, worldHeight + menuHeight];
        }

        public void Draw(Room[,] rooms, Player player, ConcurrentQueue<Pixel> cq, int numTreasures)
        {
            for (var y = 0; y < worldHeight; y++)
                for (var x = 0; x < worldWidth; x++)
                {
                    screen[x, y] = new Cell(capacity);
                    screen[x, y].Pixels = getPixels(rooms, x, y);
                }

            var col = worldWidth + menuWidth - 1;

            entry(screen, col, 0, "Health", player.Health, ConsoleColor.Yellow);
            entry(screen, col, 1, "Strength", player.Strength, ConsoleColor.Yellow);
            entry(screen, col, 2, "Treasure", numTreasures, ConsoleColor.Yellow);
            entry(screen, col, 3, "--------------", ConsoleColor.Yellow);

            var row = 4;

            foreach (var pixel in cq.Reverse())
                entry(screen, col, row++, pixel.Symbol, pixel.Color);

            entry(screen, col, worldHeight - 4, "Commands", ConsoleColor.Yellow);
            entry(screen, col, worldHeight - 3, "--------------", ConsoleColor.Yellow);
            entry(screen, col, worldHeight - 2, "Q: Quit", ConsoleColor.Yellow);
            entry(screen, col, worldHeight - 1, "Arrows: Move", ConsoleColor.Yellow);

            entry(screen, 0, worldHeight + 1, "P: Player", ConsoleColor.Yellow);
            entry(screen, 1, worldHeight + 1, "    ", ConsoleColor.Yellow);
            entry(screen, 2, worldHeight + 1, "H: Health", ConsoleColor.Green);
            entry(screen, 3, worldHeight + 1, "    ", ConsoleColor.Yellow);
            entry(screen, 4, worldHeight + 1, "T: Treasure", ConsoleColor.Blue);

            entry(screen, 0, worldHeight + 2, "M: Monster", ConsoleColor.Red);
            entry(screen, 1, worldHeight + 2, "   ", ConsoleColor.Red);
            entry(screen, 2, worldHeight + 2, "S: Strength", ConsoleColor.Cyan);

            Console.SetCursorPosition(0, 0);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Dungeons by Aryo Pehlewan feylikurds@gmail.com Copyright 2016 License GPLv3");
            Console.ResetColor();

            for (var y = 0; y < worldHeight + menuHeight; y++)
            {
                for (var x = 0; x < worldWidth + menuWidth; x++)
                {
                    var pixels = screen[x, y]?.Pixels;

                    if (pixels == null)
                        continue;

                    for (var i = 0; i < pixels.Length; i++)
                    {
                        Console.ForegroundColor = pixels[i].Color;
                        Console.Write(pixels[i].Symbol);
                        Console.ResetColor();
                    }
                }

                Console.Write(Environment.NewLine);
            }

        }

        private Pixel[] getPixels(Room[,] rooms, int x, int y)
        {
            var gos = rooms[x, y].GetGameObjects();
            var numGos = gos.Count;
            var cell = new List<Pixel>(capacity);

            if (gos.Count == 0)
            {
                cell.Add(new Pixel());

                for (int i = 0; i < capacity - 1; i++)
                    cell.Add(new Pixel { Symbol = " " });
            }
            else
            {
                foreach (var go in gos)
                {
                    var symbol = go.Symbol;
                    var color = go.Color;

                    cell.Add(new Pixel { Symbol = symbol, Color = color });
                }

                for (int i = 0; i < capacity - numGos; i++)
                    cell.Add(new Pixel { Symbol = " " });
            }

            return cell.ToArray();
        }

        private void entry(Cell[,] screen, int x, int y, string name, int value, ConsoleColor color)
        {
            screen[x, y] = new Cell(capacity);

            var text = String.Format("{0, 8}", name) + " = " + String.Format("{0, 3}", value);
            
            screen[x, y].Pixels = status(text, color);
        }

        private void entry(Cell[,] screen, int x, int y, string text, ConsoleColor color)
        {
            screen[x, y] = new Cell(capacity);
            screen[x, y].Pixels = status(text, color);
        }

        private Pixel[] status(string text, ConsoleColor color)
        {
            var cell = new Pixel[]
            {
                new Pixel { Symbol = " " },
                new Pixel { Symbol = text, Color = color }
            };

            return cell;
        }
    }
}
