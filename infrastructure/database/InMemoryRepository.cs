using CrazyZoo.application.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyZoo.infrastructure.database
{
    public class InMemoryRepository<T> : IRepository<T>
    {
        private readonly List<T> _items = new();

        public void Add(T item) => _items.Add(item);
        public void Remove(T item) => _items.Remove(item);
        public IEnumerable<T> GetAll() => _items.ToList();
        public T? Find(Func<T, bool> predicate) => _items.FirstOrDefault(predicate);
    }
}
