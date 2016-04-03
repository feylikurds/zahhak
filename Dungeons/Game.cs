using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Game
    {
        private const int WIDTH = 20;
        private const int LENGTH = 20;
        private const int NUM_MONSTERS = 10;
        private const int NUM_HEALTH = 10;
        private const int NUM_STRENGTH = 10;

        private readonly int worldWidth;
        private readonly int worldHeight;
        private readonly int numMonsters;
        private readonly int numHealth;
        private readonly int numStrength;
        private Room[,] rooms;
        private Player player;
        private Monster[] monsters;

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public Game(int worldWidth = WIDTH, int worldHeight = LENGTH, int numMonsters = NUM_MONSTERS, int numHealth = NUM_HEALTH, int numStrength = NUM_STRENGTH)
        {
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            this.numMonsters = numMonsters;
            this.numHealth = numHealth;
            this.numStrength = numStrength;
        }

        public void Start()
        {
            createWorld();

            initPlayer();

            initHealth();

            initStrength();

            initMonsters();

            displayWorld();
        }

        private void createWorld()
        {
            rooms = new Room[worldHeight, worldHeight];

            for (int y = 0; y < rooms.GetLength(1); y++)
                for (int x = 0; x < rooms.GetLength(0); x++)
                    rooms[x, y] = new Room();
        }

        private void initPlayer()
        {
            player = new Player();

            var startX = (int)Math.Round((double)worldWidth / 2);
            var startY = (int)Math.Round((double)worldHeight / 2);

            rooms[startX, startY].Enter(player);
        }

        private void initHealth()
        {
            for (int i = 0; i < numHealth; i++)
                placeRandomly(new Health());
        }

        private void initStrength()
        {
            for (int i = 0; i < numStrength; i++)
                placeRandomly(new Strength());
        }

        private void initMonsters()
        {
            monsters = new Monster[numMonsters];

            for (int i = 0; i < numMonsters; i++)
            {
                monsters[i] = new Monster();

                placeRandomly(monsters[i]);
            }
        }

        private void placeRandomly(GameObject go)
        {
            bool exit = false;
            var x = 0;
            var y = 0;

            while (!exit)
            {
                x = RandomNumber(0, worldWidth - 1);
                y = RandomNumber(0, worldHeight - 1);

                exit = rooms[x, y].Enter(go);
            }

            go.Move(x, y);
        }

        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { 
                return random.Next(min, max);
            }
        }

        private void displayWorld()
        {
            for (int y = 0; y < rooms.GetLength(1); y++)
            {
                for (int x = 0; x < rooms.GetLength(0); x++)
                        Console.Write(rooms[x, y]);

                Console.Write(Environment.NewLine);
            }

            Console.WriteLine(); 
        }
    }
}
