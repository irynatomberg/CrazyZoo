using CrazyZoo.domain.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrazyZoo.domain
{
    public class Enclosure<T> where T : Animal
    {
        private readonly List<T> _animals = new();
        private readonly Random _rnd = new();

        public string Name { get; }
        public IReadOnlyList<T> Animals => _animals;

        public Enclosure(string name) => Name = name;

        public event EventHandler<AnimalEventArgs>? AnimalJoinedInSameEnclosure;
        public event EventHandler? FoodDropped;
        public event EventHandler? NightEvent;

        public void Add(T animal)
        {
            _animals.Add(animal);
            AnimalJoinedInSameEnclosure?.Invoke(this, new AnimalEventArgs(animal));
        }

        public async Task DropFoodAsync(string food, Action<string> log)
        {
            FoodDropped?.Invoke(this, EventArgs.Empty);

            foreach (var a in _animals)
            {
                var eatMs = GetEatDurationMs(a);
                log($"{a.Name} hakkas sööma {food} (~{eatMs / 1000.0:0.0}s)");
                await Task.Delay(eatMs);
                log($"{a.Name} lõpetas söögi.");
            }
        }

        public void TriggerNight(Action<string> log)
        {
            NightEvent?.Invoke(this, EventArgs.Empty);
            foreach (var a in _animals)
                log($"{a.Name} läks magama 💤");
        }

        private int GetEatDurationMs(Animal a)
        {
            return a switch
            {
                Cat => _rnd.Next(700, 1300),
                Dog => _rnd.Next(900, 1600),
                Bird => _rnd.Next(400, 900),
                Horse => _rnd.Next(1500, 2500),
                Monkey => _rnd.Next(800, 1400),
                Fox => _rnd.Next(900, 1500),
                _ => _rnd.Next(800, 1400)
            };
        }
    }
}
