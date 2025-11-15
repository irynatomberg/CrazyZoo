using CrazyZoo.domain.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.domain.entity
{
    public class Dog : Animal, ICrazyAction
    {
        public override string MakeSound() => "Auh!";

        public string ActCrazy()
        {
            string sound = string.Join(" ", Enumerable.Repeat("Auh!", 5));
            return $"{Name} haukus hullumeelselt: {sound}";
        }
    }
}
