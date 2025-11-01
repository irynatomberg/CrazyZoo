﻿using CrazyZoo.interfaces;

namespace CrazyZoo.entity
{
    public class Wolf : Animal, ICrazyAction
    {
        public override string MakeSound() => "Auuuuu! Metsik hunt uluu kuu poole! 🐺";

        public string ActCrazy()
        {
            return $"{Name} hakkas koos teiste huntidega ulguma ja ehmatas kogu loomaaia! 🌕";
        }
    }
}
