using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    class Game
    {
        int worldWidth;
        int worldHeight;
        Player player;
        Room[,] rooms;
        
        public Game(int worldWidth = 20, int worldHeight = 20)
        {
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
        }

        public void Start()
        {
            CreateWorld();
            DisplayWorld();
        }

        private void CreateWorld()
        {
            rooms = new Room[worldHeight, worldHeight];

            for (int y = 0; y < rooms.GetLength(1); y++)
                for (int x = 0; x < rooms.GetLength(0); x++)
                    rooms[x, y] = new Room();
        }

        private void DisplayWorld()
        {
            for (int y = 0; y < rooms.GetLength(1); y++)
            {
                for (int x = 0; x < rooms.GetLength(0); x++)
                        Console.Write(rooms[x, y] + " ");

                Console.Write(Environment.NewLine);
            }

            Console.WriteLine(); 
        }
    }
}
