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
        private const int MENU_WIDTH = 1;
        private const int MENU_HEIGHT = 3;
        private const int MAX_MESSAGES = 12;

        private readonly int worldWidth;
        private readonly int worldHeight;
        private readonly int capacity;
        private readonly int numMonsters;
        private readonly int numHealth;
        private readonly int numStrength;
        private Room[,] rooms;
        private Player player;
        private List<Monster> monsters;
        private List<Health> healths;
        private List<Strength> strengths;
        private Cell[,] screen;
        private Canvas canvas;
        ConcurrentQueue<Pixel> cq;

        private static object syncLock = new object();

        public Game(int worldWidth = WIDTH, int worldHeight = HEIGHT, int capacity = CAPACITY, int numMonsters = NUM_MONSTERS, int numHealth = NUM_HEALTH, int numStrength = NUM_STRENGTH)
        {
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            this.capacity = capacity;
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

        private void announce(string text, ConsoleColor color = ConsoleColor.White)
        {
            cq.Enqueue(new Pixel { Symbol = text, Color = color });
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

            screen = new Cell[worldWidth + MENU_WIDTH, worldHeight + MENU_HEIGHT];
            canvas = new Canvas(worldHeight, worldWidth, MENU_WIDTH, MENU_HEIGHT, screen, capacity);
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

                exit = rooms[x, y].Enter(go);
            }

            go.Move(x, y);
        }

        private void displayWorld()
        {
            Console.Clear();

            canvas.Draw(rooms, player, cq);
        }

        public bool Play(ConsoleKeyInfo key)
        {
            lock (syncLock)
            {
                displayWorld();

                movePlayer(key);

                moveMonsters();

                battle();

                clearWorld();
            }

            if (player.Strength < 0)
                return true;

            return false;
        }

        private void movePlayer(ConsoleKeyInfo key)
        {
            var next = Utils.GetMove(key);

            moveCreature(player, player.X + next.Item1, player.Y + next.Item2);

            if (rooms[player.X, player.Y].Health(player))
                announce("Player got health.", ConsoleColor.Green);

            if (rooms[player.X, player.Y].Strength(player))
                announce("Player got strength.", ConsoleColor.Cyan);
        }

        private void moveMonsters()
        {
            foreach (var monster in monsters)
            {
                var next = Utils.MoveRandomly(monster.X, monster.Y);

                moveCreature(monster, next.Item1, next.Item2);
            }
        }

        private void moveCreature(GameObject go, int x, int y)
        {
            var currentX = go.X;
            var currentY = go.Y;

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
            {
                var messages = new List<string>(MAX_MESSAGES);

                room.Battle(messages);

                foreach (var message in messages)
                    announce(message, ConsoleColor.Red);
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
        }

        private void leaveRooms(IEnumerable<GameObject> lc)
        {
            var lr = lc.Where(c => c.Remove);

            foreach (var c in lr)
                rooms[c.X, c.Y].Leave(c);
        }
    }
}
