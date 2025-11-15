using CrazyZoo.domain.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.domain.entity
{
    public class Bird : Animal, IFlyable, ICrazyAction
    {
        public bool IsFlying { get; private set; }

        public override string MakeSound() => "Tširp!";

        public void Fly()
        {
            IsFlying = !IsFlying;
        }

        public string ActCrazy()
        {
            Fly();
            return $"{Name} {(IsFlying ? "lendas hullult ringi ja karjus: CHIRP!!!" : "lendas vastu puud! 😅")}";
        }
    }
}
