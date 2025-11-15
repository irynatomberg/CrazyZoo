using CrazyZoo.domain.interfaces;
using System;

namespace CrazyZoo.domain.entity
{
    public class Horse : Animal, ICrazyAction
    {
        public override string MakeSound() => "Ihahaa!";

        public string ActCrazy()
        {
            return $"{Name} galoppis hullumeelselt ringi! 🐎";
        }
    }
}
