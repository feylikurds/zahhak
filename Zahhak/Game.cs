/*
Zahhak, a C# 6.0 coding example in form of a console game.
Copyright (C) 2016 Aryo Pehlewan aryopehlewan@hotmail.com

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

namespace Zahhak
{
    internal class Game
    {
        public const int DIFFICULTY = 30;
        public const int WIDTH = 20;
        public const int HEIGHT = 20;
        public const int NUM_MONSTERS = 10;
        public const int NUM_HEALTH = 10;
        public const int NUM_STRENGTH = 10;
        public const int NUM_TREASURE = 10;
        public const int CAPACITY = 2;
        public const int MENU_WIDTH = 1;
        public const int MENU_HEIGHT = 3;
        public const int MAX_MESSAGES = 11;
        public const int STATUS_LEN = 30;
        public const int STRENGTH_LOST = 1;

        private readonly int difficulty;
        private readonly int worldWidth;
        private readonly int worldHeight;
        private readonly int numMonsters;
        private readonly int numHealth;
        private readonly int numStrength;
        private readonly int numTreasure;
        private readonly int capacity;

        private Room[,] rooms;
        private Player player;
        private List<Monster> monsters;
        private List<Health> healths;
        private List<Strength> strengths;
        private List<Treasure> treasures;
        private Canvas canvas;
        private ConcurrentQueue<Pixel> statuses;

        private static object syncLock = new object();

        public Game(int difficulty = DIFFICULTY, int worldWidth = WIDTH, int worldHeight = HEIGHT, int numMonsters = NUM_MONSTERS, int numHealth = NUM_HEALTH, int numStrength = NUM_STRENGTH, int numTreasure = NUM_TREASURE, int capacity = CAPACITY)
        {
            this.difficulty = difficulty;
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
            statuses.Enqueue(new Pixel { Symbol = text.PadRight(STATUS_LEN, ' '), Color = color });
            lock (this)
            {
                Pixel overflow;
                while (statuses.Count > MAX_MESSAGES && statuses.TryDequeue(out overflow)) ;
            }
        }

        private void createWorld()
        {
            statuses = new ConcurrentQueue<Pixel>();
            announce("Creating world.");

            rooms = new Room[worldWidth, worldHeight];

            for (int y = 0; y < worldHeight; y++)
                for (int x = 0; x < worldWidth; x++)
                    rooms[x, y] = new Room(capacity);

            canvas = new Canvas(worldWidth, worldHeight, MENU_WIDTH, MENU_HEIGHT, capacity);
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

        private void placeRandomly(GameObject gameObject)
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

                exit = rooms[x, y].Enter(gameObject);
            }

            gameObject.Move(x, y);
        }

        private void displayWorld()
        {
            canvas.Draw(rooms, player, statuses, NUM_TREASURE - treasures.Count);
        }

        public Tuple<bool, bool> Play(ConsoleKey key)
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

        private void movePlayer(ConsoleKey key)
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

                if (Utils.flip(difficulty))
                    next = Utils.MoveRandomly(monster);
                else
                    next = Utils.MoveToPlayer(monster, player);

                moveCreature(monster, next.Item1, next.Item2);
            }
        }

        private bool moveCreature(GameObject gameObject, int x, int y)
        {
            var currentX = gameObject.X;
            var currentY = gameObject.Y;

            if (x < 0)
                x = worldWidth - 1;
            else if (x >= worldWidth)
                x = 0;

            if (y < 0)
                y = worldHeight - 1;
            else if (y >= worldHeight)
                y = 0;

            var entered = rooms[x, y].Enter(gameObject);

            if (entered)
            {
                rooms[currentX, currentY].Leave(gameObject);
                gameObject.Move(x, y);
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

					foreach (var opponent in opponents)
                    {
						fighter.Fight(opponent);

                        var fighterName = fighter.GetType().Name;
						var opponentName = opponent.GetType().Name;
                        var message = fighterName + " attacked " + opponentName + "!";
                        var color = ConsoleColor.DarkRed;

                        if (fighter == player)
                        {
                            fighter.Strength -= STRENGTH_LOST;
                            color = ConsoleColor.Yellow;
                        } 
						else if (opponent == player)
                            color = ConsoleColor.Red;

                        announce(message, color);

						if (opponent.Health < 0)
                        {
							opponent.Delete();
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
            monsters.RemoveAll(monster => monster.Remove);

            leaveRooms(healths);
            healths.RemoveAll(health => health.Remove);

            leaveRooms(strengths);
            strengths.RemoveAll(strength => strength.Remove);

            leaveRooms(treasures);
            treasures.RemoveAll(treasure => treasure.Remove);
        }

        private void leaveRooms(IEnumerable<GameObject> collectionGameObjects)
        {
            var deletedGameObjects = collectionGameObjects.Where(gameObject => gameObject.Remove);

            foreach (var gameObject in deletedGameObjects)
                rooms[gameObject.X, gameObject.Y].Leave(gameObject);
        }
    }
}
