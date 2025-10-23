using CrazyZoo.interfaces;
using System;

namespace CrazyZoo.entity
{
    public class Monkey : Animal, ICrazyAction
    {
        public override string MakeSound() => "Oo-oo-aa-aa!";

        public void ActCrazy()
        {
            Console.WriteLine($"{Name} vahetas kahe looma nimed omavahel!");
        }
    }
}