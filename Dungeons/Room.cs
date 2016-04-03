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

        public Room(int capacity = CAPACITY, string name = NAME, string symbol = SYMBOL) : base(name, symbol)
        {
            this.capacity = capacity;
        }

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

        public Pixel[] Draw()
        {
            var numGameObjects = gameObjects.Count;
            var cell = new List<Pixel>(capacity);

            if (numGameObjects == 0)
            {
                cell.Add(new Pixel());

                for (int i = 0; i < capacity - 1; i++)
                    cell.Add(new Pixel{ Symbol = " " });
            }
            else
            {
                foreach (var go in gameObjects)
                {
                    var symbol = go.Symbol;
                    var color = go.Color;

                    cell.Add(new Pixel { Symbol = symbol, Color = color });
                }

                for (int i = 0; i < capacity - numGameObjects; i++)
                    cell.Add(new Pixel { Symbol = " " });
            }

            return cell.ToArray();
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