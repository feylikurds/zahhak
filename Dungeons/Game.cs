using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    class Game
    {
        const int WIDTH = 20;
        const int LENGTH = 20;

        readonly int worldWidth;
        readonly int worldHeight;
        Player player;
        Room[,] rooms;
        
        public Game(int worldWidth = WIDTH, int worldHeight = LENGTH)
        {
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
        }

        public void Start()
        {
            createWorld();

            initPlayer();

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

        private void moveObject(GameObject gameObject, int x, int y)
        {
            rooms[x, y].Enter(gameObject);
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


        private void movePlayer(double startX, double startY)
        {
            throw new NotImplementedException();
        }
    }
}
