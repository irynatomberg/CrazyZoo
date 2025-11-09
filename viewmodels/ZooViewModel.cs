using CrazyZoo.domain;
using CrazyZoo.entity;
using CrazyZoo.infrastructure;
using CrazyZoo.interfaces;
using CrazyZoo.repositories;
using CrazyZoo.logging;
using CrazyZoo.resources;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CrazyZoo.viewmodels
{
    public class ZooViewModel : BaseViewModel
    {
        private readonly IRepository<Animal> _repo;
        private readonly ILogger _logger;
        private readonly Enclosure<Animal> _enclosure = new(Strings.EnclosureName);
        private readonly System.Timers.Timer _nightTimer = new(10000);

        public ObservableCollection<Animal> Animals { get; } = new();

        private Animal? _selected;
        public Animal? Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged();
                RaiseCommandStates();
            }
        }

        private AnimalType _newType = AnimalType.Kass;
        public AnimalType NewType
        {
            get => _newType;
            set { _newType = value; OnPropertyChanged(); }
        }

        private string _newName = "";
        public string NewName
        {
            get => _newName;
            set { _newName = value; OnPropertyChanged(); }
        }

        private string _newAge = "";
        public string NewAge
        {
            get => _newAge;
            set { _newAge = value; OnPropertyChanged(); }
        }

        private string _food = "";
        public string Food
        {
            get => _food;
            set
            {
                _food = value;
                OnPropertyChanged();
                (FeedSelectedCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private readonly StringBuilder _log = new();
        public string LogText => _log.ToString();

        private string _stats = "";
        public string Stats
        {
            get => _stats;
            set { _stats = value; OnPropertyChanged(); }
        }

        public ICommand AddAnimalCommand { get; }
        public ICommand RemoveSelectedCommand { get; }
        public ICommand MakeSoundCommand { get; }
        public ICommand FeedSelectedCommand { get; }
        public ICommand CrazyActionCommand { get; }
        public ICommand DropFoodToAllCommand { get; }

        public ZooViewModel()
        {
            _repo = App.Services.GetRequiredService<IRepository<Animal>>();
            _logger = App.Services.GetRequiredService<ILogger>();

            _enclosure.AnimalJoinedInSameEnclosure += (_, e) =>
                Log(string.Format(Strings.AnimalJoined, e.Animal.Name));
            _enclosure.FoodDropped += (_, __) => Log(Strings.FoodDropped);
            _enclosure.NightEvent += (_, __) => Log(Strings.NightEvent);

            LoadFromDatabase();

            AddAnimalCommand = new RelayCommand(_ => AddAnimal());
            RemoveSelectedCommand = new RelayCommand(_ => RemoveSelected(), _ => Selected != null);
            MakeSoundCommand = new RelayCommand(_ => MakeSound(), _ => Selected != null);
            FeedSelectedCommand = new RelayCommand(_ => FeedSelected(), _ => Selected != null && !string.IsNullOrWhiteSpace(Food));
            CrazyActionCommand = new RelayCommand(_ => CrazyAction(), _ => Selected is ICrazyAction);
            DropFoodToAllCommand = new RelayCommand(async _ => await _enclosure.DropFoodAsync(FoodOrDefault(), Log));

            _nightTimer.Elapsed += (_, __) => _enclosure.TriggerNight(Log);
            _nightTimer.AutoReset = true;
            _nightTimer.Start();

            RebuildStats();
        }

        private void LoadFromDatabase()
        {
            var allAnimals = _repo.GetAll().ToList();

            if (allAnimals.Count == 0)
            {
                Seed();
                Log(Strings.DatabaseSeeded);
                return;
            }

            foreach (var a in allAnimals)
            {
                Animals.Add(a);
                _enclosure.Add(a);
            }

            Log(string.Format(Strings.DatabaseLoaded, allAnimals.Count));
        }

        private void Seed()
        {
            var start = new Animal[]
            {
                new Cat { Name = "Muri", Age = 3 },
                new Dog { Name = "Pontu", Age = 5 },
                new Bird { Name = "Tibu", Age = 1 },
                new Monkey { Name = "Tark", Age = 4 },
                new Fox { Name = "Kaval", Age = 2 },
                new Horse { Name = "Torm", Age = 6 },
                new Elephant { Name = "Suur", Age = 8 },
                new Wolf { Name = "Hall", Age = 5 },
                new Tiger { Name = "Triibu", Age = 7 }
            };

            foreach (var a in start)
            {
                _repo.Add(a);
                Animals.Add(a);
                _enclosure.Add(a);
            }

            Log(Strings.InitialAnimalsCreated);
        }

        private void AddAnimal()
        {
            if (!int.TryParse(NewAge, out var age) || age < 0)
            {
                Log(Strings.InvalidAge);
                return;
            }

            var name = string.IsNullOrWhiteSpace(NewName) ? Strings.DefaultName : NewName;

            Animal a = NewType switch
            {
                AnimalType.Kass => new Cat { Name = name, Age = age },
                AnimalType.Koer => new Dog { Name = name, Age = age },
                AnimalType.Lind => new Bird { Name = name, Age = age },
                AnimalType.Hobune => new Horse { Name = name, Age = age },
                AnimalType.Ahv => new Monkey { Name = name, Age = age },
                AnimalType.Rebane => new Fox { Name = name, Age = age },
                AnimalType.Elevant => new Elephant { Name = name, Age = age },
                AnimalType.Hunt => new Wolf { Name = name, Age = age },
                AnimalType.Tiiger => new Tiger { Name = name, Age = age },
                _ => new Cat { Name = name, Age = age }
            };

            _repo.Add(a);
            Animals.Add(a);
            _enclosure.Add(a);
            Log(string.Format(Strings.AnimalAdded, a.Name));

            NewName = NewAge = "";
            OnPropertyChanged(nameof(NewName));
            OnPropertyChanged(nameof(NewAge));
            RebuildStats();
        }

        private void RemoveSelected()
        {
            if (Selected == null) return;

            var removed = Selected;

            _repo.Remove(removed);
            Animals.Remove(removed);
            Log(string.Format(Strings.AnimalRemoved, removed.Name));

            Selected = null;
            RebuildStats();
        }

        private void MakeSound()
        {
            if (Selected == null) return;
            Log($"{Selected.Name}: {Selected.MakeSound()}");
        }

        private void FeedSelected()
        {
            if (Selected == null || string.IsNullOrWhiteSpace(Food)) return;
            Log(string.Format(Strings.AteFood, Selected.Name, Food));
        }

        private void CrazyAction()
        {
            if (Selected is ICrazyAction crazy)
                Log(crazy.ActCrazy());
            else
                Log(Strings.NoCrazyAction);
        }

        private string FoodOrDefault() =>
            string.IsNullOrWhiteSpace(Food) ? Strings.DefaultFood : Food;

        private void Log(string msg)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            string full = $"[{timestamp}] {msg}{Environment.NewLine}";
            _logger.Log(msg);
            _log.Insert(0, full);
            OnPropertyChanged(nameof(LogText));
        }

        private void RaiseCommandStates()
        {
            (MakeSoundCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (FeedSelectedCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (CrazyActionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (RemoveSelectedCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void RebuildStats()
        {
            var all = Animals.ToList();
            if (all.Count == 0)
            {
                Stats = Strings.NoAnimals;
                return;
            }

            var byType = all
                .GroupBy(a => a.GetType().Name)
                .Select(g => $"{g.Key}: {g.Count()} tk (avg {g.Average(x => x.Age):0.0})");

            var oldest = all.OrderByDescending(a => a.Age).FirstOrDefault();
            var oldestStr = oldest != null
                ? string.Format(Strings.OldestAnimal, oldest.Name, oldest.GetType().Name, oldest.Age)
                : "";

            var avgAll = all.Average(a => a.Age);

            Stats = string.Join("\n", byType) +
                    (string.IsNullOrEmpty(oldestStr) ? "" : $"\n{oldestStr}") +
                    $"\n" + string.Format(Strings.AvgAgeTotal, avgAll);
        }
    }
}
