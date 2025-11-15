using CrazyZoo.domain.interfaces;

namespace CrazyZoo.domain.entity
{
    public class Tiger : Animal, ICrazyAction
    {
        public override string MakeSound() => "Roaaar! Tiiger näitab kihvu! 🐯";

        public string ActCrazy()
        {
            return $"{Name} hüppas basseini ja hakkas ujuma nagu kass! 🐾";
        }
    }
}
