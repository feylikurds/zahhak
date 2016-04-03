using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal abstract class Creature : GameObject
    {
        public int Health { get; set; }
        public int Strength { get; set; } = 10;

        public Creature(string name, string symbol, int health = 100, int strength = 10) : base(name, symbol)
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
