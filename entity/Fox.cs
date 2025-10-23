using CrazyZoo.interfaces;
using System;

namespace CrazyZoo.entity
{
    public class Fox : Animal, ICrazyAction
    {
        public override string MakeSound() => "Ring-ding-ding!";

        public void ActCrazy()
        {
            Console.WriteLine($"{Name} peitis kellegi toidu ära!");
        }
    }
}
