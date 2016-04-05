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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Game
    {
        private const int WIDTH = 20;
        private const int HEIGHT = 20;
        private const int CAPACITY = 2;
        private const int NUM_MONSTERS = 10;
        private const int NUM_HEALTH = 10;
        private const int NUM_STRENGTH = 10;
        private const int NUM_TREASURE = 10;
        private const int MENU_WIDTH = 1;
        private const int MENU_HEIGHT = 3;
        private const int MAX_MESSAGES = 11;
        private const int STATUS_LEN = 30;
        private const int DIFFICULTY = 30;
        private const int STRENGTH_LOST = 1;
        private readonly int worldWidth;
        private readonly int worldHeight;
        private readonly int capacity;
        private readonly int numMonsters;
        private readonly int numHealth;
        private readonly int numStrength;
        private readonly int numTreasure;
        private Room[,] rooms;
        private Player player;
        private List<Monster> monsters;
        private List<Health> healths;
        private List<Strength> strengths;
        private List<Treasure> treasures;
        private Canvas canvas;
        ConcurrentQueue<Pixel> cq;

        private static object syncLock = new object();

        public Game(int worldWidth = WIDTH, int worldHeight = HEIGHT, int capacity = CAPACITY, int numMonsters = NUM_MONSTERS, int numHealth = NUM_HEALTH, int numStrength = NUM_STRENGTH, int numTreasure = NUM_TREASURE)
        {
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            this.capacity = capacity;
            this.numMonsters = numMonsters;
            this.numHealth = numHealth;
            this.numStrength = numStrength;
            this.numTreasure = numTreasure;
        }

        public void Start()
        {
            createWorld();

            initPlayer();

            initHealth();

            initStrength();

            initTreasure();

            initMonsters();
        }

        private void announce(string text, ConsoleColor color = ConsoleColor.White)
        {
            cq.Enqueue(new Pixel { Symbol = text.PadRight(STATUS_LEN, ' '), Color = color });
            lock (this)
            {
                Pixel overflow;
                while (cq.Count > MAX_MESSAGES && cq.TryDequeue(out overflow)) ;
            }
        }

        private void createWorld()
        {
            cq = new ConcurrentQueue<Pixel>();
            announce("Creating world.");

            rooms = new Room[worldWidth, worldHeight];

            for (int y = 0; y < worldWidth; y++)
                for (int x = 0; x < worldHeight; x++)
                    rooms[x, y] = new Room(capacity);

            canvas = new Canvas(worldHeight, worldWidth, MENU_WIDTH, MENU_HEIGHT, capacity);
        }

        private void initPlayer()
        {
            announce("Initializing player.");

            player = new Player();

            var startX = (int)Math.Round((double)worldWidth / 2);
            var startY = (int)Math.Round((double)worldHeight / 2);

            rooms[startX, startY].Enter(player);
            player.Move(startX, startY);
        }

        private void initHealth()
        {
            announce("Initializing health.");

            healths = new List<Health>();

            for (int i = 0; i < numHealth; i++)
            {
                var health = new Health();

                healths.Add(health);
                placeRandomly(health);
            }
        }

        private void initStrength()
        {
            announce("Initializing strength.");

            strengths = new List<Strength>();

            for (int i = 0; i < numStrength; i++)
            {
                var strength = new Strength();

                strengths.Add(strength);
                placeRandomly(strength);
            }
        }

        private void initTreasure()
        {
            announce("Initializing treasure.");

            treasures = new List<Treasure>();

            for (int i = 0; i < numTreasure; i++)
            {
                var treasure = new Treasure();

                treasures.Add(treasure);
                placeRandomly(treasure);
            }
        }

        private void initMonsters()
        {
            announce("Initializing monsters.");

            monsters = new List<Monster>();

            for (int i = 0; i < numMonsters; i++)
            {
                var monster = new Monster();

                monsters.Add(monster);
                placeRandomly(monster);
            }
        }

        private void placeRandomly(GameObject go)
        {
            bool exit = false;
            var x = 0;
            var y = 0;

            while (!exit)
            {
                x = Utils.RandomNumber(0, worldWidth - 1);
                y = Utils.RandomNumber(0, worldHeight - 1);

                if (!rooms[x, y].HasRoomForTwo())
                    continue;

                exit = rooms[x, y].Enter(go);
            }

            go.Move(x, y);
        }

        private void displayWorld()
        {
            canvas.Draw(rooms, player, cq, NUM_TREASURE - treasures.Count);
        }

        public Tuple<bool, bool> Play(ConsoleKeyInfo key)
        {
            lock (syncLock)
            {
                displayWorld();

                movePlayer(key);

                moveMonsters();

                battle();

                clearWorld();
            }

            if (player.Health < 0)
                return Tuple.Create(false, false);
            else if (treasures.Count == 0)
                return Tuple.Create(true, true);

            return Tuple.Create(true, false); 
        }

        private void movePlayer(ConsoleKeyInfo key)
        {
            var next = Utils.GetMove(key);

            if (!moveCreature(player, player.X + next.Item1, player.Y + next.Item2))
                return;

            var healths = rooms[player.X, player.Y].GetHealths();

            foreach (var health in healths)
            {
                player.Health += health.Points;
                health.Delete();
            }

            if (healths.Count<Health>() > 0)
                announce("Player got health.", ConsoleColor.Green);

            var strengths = rooms[player.X, player.Y].GetStrengths();

            foreach (var strength in strengths)
            {
                player.Strength += strength.Points;
                strength.Delete();
            }

            if (strengths.Count<Strength>() > 0)
                announce("Player got strength.", ConsoleColor.Cyan);

            var treasures = rooms[player.X, player.Y].GetTreasures();

            foreach (var treasure in treasures)
            {
                treasure.Delete();
            }

            if (treasures.Count<Treasure>() > 0)
                announce("Player got treasure.", ConsoleColor.Blue);
        }

        private void moveMonsters()
        {
            foreach (var monster in monsters)
            {
                Tuple<int, int> next;

                if (Utils.flip(DIFFICULTY))
                    next = Utils.MoveRandomly(monster);
                else
                    next = Utils.MoveToPlayer(monster, player);

                moveCreature(monster, next.Item1, next.Item2);
            }
        }

        private bool moveCreature(GameObject go, int x, int y)
        {
            var currentX = go.X;
            var currentY = go.Y;

            if (x < 0 || y < 0 || x >= worldWidth || y >= worldHeight)
                return false;

            var entered = rooms[x, y].Enter(go);

            if (entered)
            {
                rooms[currentX, currentY].Leave(go);
                go.Move(x, y);
            }

            return entered;
        }

        private void battle()
        {
            foreach (var room in rooms)
            {
                var fighters = room.GetCreatures();

                foreach (var fighter in fighters)
                {
                    var opponents = fighters.Where(f => f != fighter);

                    foreach (var o in opponents)
                    {
                        fighter.Fight(o);

                        var fighterName = fighter.GetType().Name;
                        var opponentName = o.GetType().Name;
                        var message = fighterName + " attacked " + opponentName + "!";
                        var color = ConsoleColor.DarkRed;

                        if (fighter == player)
                        {
                            fighter.Strength -= STRENGTH_LOST;
                            color = ConsoleColor.Yellow;
                        } 
                        else if (o == player)
                            color = ConsoleColor.Red;

                        announce(message, color);

                        if (o.Health < 0)
                        {
                            o.Delete();
                            message = opponentName + " was killed!";
                            color = ConsoleColor.Magenta;
                            announce(message, color);
                        }
                    }
                }
            }
        }

        private void clearWorld()
        {
            leaveRooms(monsters);
            monsters.RemoveAll(c => c.Remove);

            leaveRooms(healths);
            healths.RemoveAll(c => c.Remove);

            leaveRooms(strengths);
            strengths.RemoveAll(c => c.Remove);

            leaveRooms(treasures);
            treasures.RemoveAll(c => c.Remove);
        }

        private void leaveRooms(IEnumerable<GameObject> lc)
        {
            var lr = lc.Where(c => c.Remove);

            foreach (var c in lr)
                rooms[c.X, c.Y].Leave(c);
        }
    }
}
