using CrazyZoo.domain.interfaces;
using System;

namespace CrazyZoo.domain.entity
{
    public class Fox : Animal, ICrazyAction
    {
        public override string MakeSound() => "Ring-ding-ding!";

        public string ActCrazy()
        {
            return $"{Name} peitis kellegi toidu ära! 🦊";
        }
    }
}
