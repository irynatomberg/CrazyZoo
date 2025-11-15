using CrazyZoo.domain.interfaces;
using System;

namespace CrazyZoo.domain.entity
{
    public class Monkey : Animal, ICrazyAction
    {
        public override string MakeSound() => "Oo-oo-aa-aa!";

        public string ActCrazy()
        {
            return $"{Name} vahetas kahe looma nimed omavahel! 🐒";
        }
    }
}