﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeons
{
    internal class Player : Creature
    {
        public Player(string name = "Player", string symbol = "P") : base(name, symbol) { }
    }
}