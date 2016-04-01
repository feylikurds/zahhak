using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Room : GameObject
    {
        readonly int capacity;
        List<GameObject> gameObjects = new List<GameObject>();

        public Room(string name = "Room", string symbol = "R", int capacity = 2) : base(name, symbol)
        {
            this.capacity = capacity;
        }

        public bool Enter(GameObject gameObject)
        {
            var numGameObjects = gameObjects.Count;

            if (numGameObjects >= 2)
                return false;

            gameObjects.Add(gameObject);

            return true;
        }

        public void Leave(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
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
                foreach (var gameObject in gameObjects)
                    cell += gameObject.Symbol;

                for (int i = 0; i < capacity - numGameObjects; i++)
                    cell += " ";
            }

            return cell;
        }
    }
}