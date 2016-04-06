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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zahhak
{
    internal class Canvas
    {
        private readonly int worldHeight;
        private readonly int worldWidth;
        private readonly int menuWidth;
        private readonly int menuHeight;
        private readonly int capacity;

        private Cell[,] screen;

        public Canvas(int worldWidth, int worldHeight, int menuWidth, int menuHeight, int capacity)
        {
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            this.menuWidth = menuWidth;
            this.menuHeight = menuHeight;
            this.capacity = capacity;

            screen = new Cell[worldWidth + menuWidth, worldHeight + menuHeight];
        }

        public void Draw(Room[,] rooms, Player player, ConcurrentQueue<Pixel> statuses, int numTreasures)
        {
            copy(rooms);

            var column = worldWidth + menuWidth - 1;

            entry(column, 0, "Health", player.Health, ConsoleColor.Yellow);
            entry(column, 1, "Strength", player.Strength, ConsoleColor.Yellow);
            entry(column, 2, "Treasure", numTreasures, ConsoleColor.Yellow);
            entry(column, 3, "--------------", ConsoleColor.Yellow);

            var row = 4;

            foreach (var status in statuses.Reverse())
                entry(column, row++, status.Symbol, status.Color);

            entry(column, worldHeight - 4, "Commands", ConsoleColor.Yellow);
            entry(column, worldHeight - 3, "--------------", ConsoleColor.Yellow);
            entry(column, worldHeight - 2, "Q: Quit", ConsoleColor.Yellow);
            entry(column, worldHeight - 1, "Arrows: Move", ConsoleColor.Yellow);

            entry(0, worldHeight + 1, "P: Player", ConsoleColor.Yellow);
            entry(1, worldHeight + 1, " ", ConsoleColor.Yellow);
            entry(2, worldHeight + 1, "H: Health", ConsoleColor.Green);
            entry(3, worldHeight + 1, "  ", ConsoleColor.Green);
            entry(4, worldHeight + 1, "T: Treasure", ConsoleColor.Blue);

            entry(0, worldHeight + 2, "M: Monster", ConsoleColor.Red);
            entry(1, worldHeight + 2, "", ConsoleColor.Red);
            entry(2, worldHeight + 2, "S: Strength", ConsoleColor.Cyan);
            entry(3, worldHeight + 2, "", ConsoleColor.Cyan);
            entry(4, worldHeight + 2, "Type 'Zahhak help' for more options.", ConsoleColor.White);

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Zahhak by Aryo Pehlewan feylikurds@gmail.com Copyright 2016 License GPLv3");
            Console.ResetColor();

            paint();
        }

        private void copy(Room[,] rooms)
        {
            for (var y = 0; y < worldHeight; y++)
                for (var x = 0; x < worldWidth; x++)
                {
                    screen[x, y] = new Cell(capacity);
                    screen[x, y].Pixels = getPixels(rooms[x, y]);
                }
        }

        private void paint()
        {
            for (var y = 0; y < worldHeight + menuHeight; y++)
            {
                for (var x = 0; x < worldWidth + menuWidth; x++)
                {
                    var pixels = screen[x, y]?.Pixels;

                    if (pixels == null)
                        continue;

                    foreach (var pixel in pixels)
                    {
                        Console.ForegroundColor = pixel.Color;
                        Console.Write(pixel.Symbol);
                        Console.ResetColor();
                    }
                }

                Console.Write(Environment.NewLine);
            }
        }

        private Pixel[] getPixels(Room room)
        {
            var gameObjects = room.GetGameObjects();
            var numGgameObjects = gameObjects.Count;
            var cell = new List<Pixel>(capacity);

            if (gameObjects.Count == 0)
            {
                cell.Add(new Pixel { Symbol = "." });

                for (int i = 0; i < capacity - 1; i++)
                    cell.Add(new Pixel());
            }
            else
            {
                foreach (var gameObject in gameObjects)
                {
                    var symbol = gameObject.Symbol;
                    var color = gameObject.Color;

                    cell.Add(new Pixel { Symbol = symbol, Color = color });
                }

                for (int i = 0; i < capacity - numGgameObjects; i++)
                    cell.Add(new Pixel());
            }

            return cell.ToArray();
        }

        private void entry(int x, int y, string name, int value, ConsoleColor color)
        {
            screen[x, y] = new Cell(capacity);

            var text = String.Format("{0, 8}", name) + " = " + String.Format("{0, 3}", value);
            
            screen[x, y].Pixels = status(text, color);
        }

        private void entry(int x, int y, string text, ConsoleColor color)
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
