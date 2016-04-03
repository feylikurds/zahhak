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

        public void Draw()
        {
            var numGameObjects = gameObjects.Count;
            var cell = "";

            if (numGameObjects == 0)
            {
                cell = ".";

                for (int i = 0; i < capacity - 1; i++)
                    cell += " ";

                Console.Write(cell);
            }
            else
            {
                foreach (var go in gameObjects)
                {
                    var symbol = go.Symbol;
                    var color = go.Color;

                    Console.ForegroundColor = color;
                    Console.Write(symbol);
                    Console.ResetColor();
                }

                for (int i = 0; i < capacity - numGameObjects; i++)
                    cell += " ";

                Console.Write(cell);
            }
        }

        public void Health(Creature player)
        {
            var healths = gameObjects.Where(go => go is Health).Cast<Health>().ToList();

            foreach (var health in healths)
            {
                player.Health += health.Amount;
                health.Delete(" ");
            }
        }

        public void Strength(Creature player)
        {
            var strengths = gameObjects.Where(go => go is Strength).Cast<Strength>().ToList();

            foreach (var strength in strengths)
            {
                player.Strength += strength.Amount;
                strength.Delete(" ");
            }
        }

        public void Battle()
        {
            var fighters = gameObjects.Where(go => go is Creature).Cast<Creature>().ToList();

            foreach (var fighter in fighters)
            {
                var opponents = fighters.Where(f => f != fighter).ToList();

                foreach (var o in opponents)
                {
                    fighter.Fight(o);

                    if (o.Health < 0)
                        o.Delete(" ");
                }
            }
        }
    }
}