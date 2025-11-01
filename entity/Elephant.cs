using CrazyZoo.interfaces;

namespace CrazyZoo.entity
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
