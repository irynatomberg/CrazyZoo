using CrazyZoo.application.interfaces;
using CrazyZoo.domain.entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyZoo.infrastructure.database
{
    public class EfAnimalRepository : IRepository<Animal>
    {
        private readonly ZooDbContext _ctx;

        public EfAnimalRepository(ZooDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<Animal> GetAll() =>
            _ctx.Animals.ToList();

        public Animal? Find(Func<Animal, bool> predicate) =>
            _ctx.Animals.FirstOrDefault(predicate);

        public void Add(Animal animal)
        {
            _ctx.Animals.Add(animal);
            _ctx.SaveChanges();
        }

        public void Remove(Animal animal)
        {
            _ctx.Animals.Remove(animal);
            _ctx.SaveChanges();
        }
    }
}
