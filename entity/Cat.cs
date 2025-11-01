﻿using CrazyZoo.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.entity
{
    public class Cat : Animal, ICrazyAction
    {
        public override string MakeSound() => "Mjäu!";
        public string ActCrazy() => $"{Name} varastas köögist juustu.";
    }

}
