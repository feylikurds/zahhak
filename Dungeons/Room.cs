/*
Dungeons, a C# 6.0 coding example in form of a console game.
Copyright (C) 2016 Aryo Pehlewan feylikurds@gmail.com

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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

        public bool Health(Creature player)
        {
            var healths = gameObjects.Where(go => go is Health).Cast<Health>().ToList();

            foreach (var health in healths)
            {
                player.Health += health.Amount;
                health.Delete(" ");
            }

            if (healths.Count > 0)
                return true;

            return false;
        }

        public bool Strength(Creature player)
        {
            var strengths = gameObjects.Where(go => go is Strength).Cast<Strength>().ToList();

            foreach (var strength in strengths)
            {
                player.Strength += strength.Amount;
                strength.Delete(" ");
            }

            if (strengths.Count > 0)
                return true;

            return false;
        }

        public void Battle(List<string> messages)
        {
            var fighters = gameObjects.Where(go => go is Creature).Cast<Creature>().ToList();

            foreach (var fighter in fighters)
            {
                var opponents = fighters.Where(f => f != fighter).ToList();

                foreach (var o in opponents)
                {
                    fighter.Fight(o);

                    var text = fighter.GetType().Name + " attacked " + o.GetType().Name + "!";
                    messages.Add(text);

                    if (o.Health < 0)
                    {
                        text = o.GetType().Name + " was killed!";
                        messages.Add(text);

                        o.Delete(" ");
                    }
                }
            }
        }
    }
}
