using System;
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
        private List<Monster> monsters;
        private List<Health> healths;
        private List<Strength> strengths;

        private static Random random = new Random();
        private static object syncLock = new object();

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
            player.Move(startX, startY);
        }

        private void initHealth()
        {
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
            strengths = new List<Strength>();

            for (int i = 0; i < numStrength; i++)
            {
                var strength = new Strength();

                strengths.Add(strength);
                placeRandomly(strength);
            }
        }

        private void initMonsters()
        {
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
            Console.Clear();

            for (int y = 0; y < rooms.GetLength(1); y++)
            {
                for (int x = 0; x < rooms.GetLength(0); x++)
                    rooms[x, y].Draw();

                Console.Write(Environment.NewLine);
            }
        }

        public bool Play(ConsoleKeyInfo key)
        {
            displayWorld();

            movePlayer(key);

            moveMonsters();

            battle();

            clearWorld();

            if (player.Strength < 0)
                return true;

            return false;
        }

        private void movePlayer(ConsoleKeyInfo key)
        {
            var next = getMove(key);

            moveCreature(player, player.X + next.Item1, player.Y + next.Item2);

            rooms[player.X, player.Y].Health(player);

            rooms[player.X, player.Y].Strength(player);
        }

        private Tuple<int, int> getMove(ConsoleKeyInfo key)
        {
            var x = 0;
            var y = 0;

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    y = -1;
                    break;
                           
                case ConsoleKey.DownArrow:
                    y = 1;
                    break;

                case ConsoleKey.LeftArrow:
                    x = -1;
                    break;

                case ConsoleKey.RightArrow:
                    x = 1;
                    break;

                default:
                    break;
            }

            Debug.WriteLine($"key: {key.Key} x: {x} y: {y}");

            return Tuple.Create(x, y);
        }

        private void moveMonsters()
        {
            foreach (var monster in monsters)
            {
                var next = moveRandomly(monster.X, monster.Y);

                moveCreature(monster, next.Item1, next.Item2);
            }
        }

        private Tuple<int, int> moveRandomly(int currentX, int currentY)
        {
            var x = currentX;
            var y = currentY;

            var n = RandomNumber(0, 100);

            if (n > 50)
            {
                n = RandomNumber(0, 100);

                if (n > 50)
                    x += -1;
                else
                    x += 1;
            }
            else
            {
                n = RandomNumber(0, 100);

                if (n > 50)
                    y += -1;
                else
                    y += 1;
            }

            return Tuple.Create(x, y);
        }

        private void moveCreature(GameObject go, int x, int y)
        {
            var currentX = go.X;
            var currentY = go.Y;

            Debug.WriteLine($"go: {go} x: {x} y: {y}");

            if (x < 0 || y < 0 || x >= worldWidth || y >= worldHeight)
                return;

            var entered = rooms[x, y].Enter(go);

            if (entered)
            {
                rooms[currentX, currentY].Leave(go);
                go.Move(x, y);
            }
        }

        private void battle()
        {
            foreach (var room in rooms)
                room.Battle();
        }

        private void clearWorld()
        {
            leaveRooms(monsters);
            monsters.RemoveAll(c => c.Remove);

            leaveRooms(healths);
            healths.RemoveAll(c => c.Remove);

            leaveRooms(strengths);
            strengths.RemoveAll(c => c.Remove);
        }

        private void leaveRooms(IEnumerable<GameObject> lc)
        {
            var lr = lc.Where(c => c.Remove);

            foreach (var c in lr)
                rooms[c.X, c.Y].Leave(c);
        }
    }
}
