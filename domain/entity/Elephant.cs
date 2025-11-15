using CrazyZoo.domain.interfaces;

namespace CrazyZoo.domain.entity
{
    public class Elephant : Animal, ICrazyAction
    {
        public override string MakeSound() => "Tööörr! Suur elevant tõstis lonti! 🐘";

        public string ActCrazy()
        {
            return $"{Name} pritsis veega kõik loomad märjaks! 💦";
        }
    }
}
