using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Room : GameObject
    {
        protected const string NAME = "Room";
        protected const string SYMBOL = "R";
        protected const int CAPACITY = 2;

        private readonly int capacity;
        private List<GameObject> gameObjects = new List<GameObject>();

        public Room(string name = NAME, string symbol = SYMBOL, int capacity = CAPACITY) : base(name, symbol)
        {
            this.capacity = capacity;
        }

        public bool Enter(GameObject go)
        {
            var numGameObjects = gameObjects.Count;

            if (numGameObjects >= capacity)
                return false;

            gameObjects.Add(go);

            return true;
        }

        public void Leave(GameObject go)
        {
            gameObjects.Remove(go);
        }

        public override string ToString()
        {
            var numGameObjects = gameObjects.Count;
            var cell = "";

            if (numGameObjects == 0)
            {
                cell = ".";

                for (int i = 0; i < capacity - 1; i++)
                    cell += " ";
            }
            else
            {
                foreach (var go in gameObjects)
                    cell += go.Symbol;

                for (int i = 0; i < capacity - numGameObjects; i++)
                    cell += " ";
            }

            return cell;
        }
    }
}