using CrazyZoo.domain.entity;
using System;

namespace CrazyZoo.domain
{
    public class AnimalEventArgs : EventArgs
    {
        public AnimalEventArgs(Animal animal) => Animal = animal;
        public Animal Animal { get; }
    }
}
