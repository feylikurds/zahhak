using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Game
    {
        const int WIDTH = 20;
        const int LENGTH = 20;
        const int NUM_MONSTERS = 10;

        readonly int worldWidth;
        readonly int worldHeight;
        Room[,] rooms;
        Player player;
        Monster[] monsters;

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public Game(int worldWidth = WIDTH, int worldHeight = LENGTH, int numMonsters = NUM_MONSTERS)
        {
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            monsters = new Monster[numMonsters];
        }

        public void Start()
        {
            createWorld();

            initPlayer();

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


        private void initMonsters()
        {

            for (int i = 0; i < monsters.Length; i++)
            {
                monsters[i] = new Monster();

                moveRandomly(monsters[i]);
            }
        }

        private void moveRandomly(GameObject go)
        {
            bool exit = false;

            while (!exit)
            {
                var x = RandomNumber(0, worldWidth - 1);
                var y = RandomNumber(0, worldHeight - 1);

                exit = rooms[x, y].Enter(go);
            }
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
