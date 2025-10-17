using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.entity
{
    public abstract class Animal
    {
        public required string Name { get; set; }
        public required int Age { get; set; }

        public virtual string Describe() => $"{Name}, {Age} aastat vana.";
        public abstract string MakeSound();
    }
}
