using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal abstract class Creature : GameObject
    {
        protected const int HEALTH = 100;
        protected const int STRENGTH = 10;

        public int Health { get; set; }
        public int Strength { get; set; }

        public Creature(string name, string symbol, ConsoleColor color, int health = HEALTH, int strength = STRENGTH) : base(name, symbol, color)
        {
            Health = health;
            Strength = strength;
        }

        public void Fight(Creature opponent)
        {
            opponent.Health -= Strength;
        }
    }
}
