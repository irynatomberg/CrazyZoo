using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.domain.entity
{
    public abstract class Animal
    {
        public int Id { get; set; } 

        public string Name { get; set; } = "";
        public int Age { get; set; }

        public virtual string Describe() => $"{Name}, {Age} aastat vana.";
        public abstract string MakeSound();
    }
}
