using CrazyZoo.interfaces;
using CrazyZoo.entity;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrazyZoo.repositories
{
    public class DatabaseRepository : IRepository<Animal>
    {
        private readonly string _dbPath;

        public DatabaseRepository(string dbPath = "crazyzoo.db")
        {
            _dbPath = Path.GetFullPath(dbPath);
            EnsureDatabase();
        }

        private void EnsureDatabase()
        {
            using var conn = new SqliteConnection($"Data Source={_dbPath}");
            conn.Open();

            string sql = @"
            CREATE TABLE IF NOT EXISTS Animals (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Age INTEGER NOT NULL,
                Type TEXT NOT NULL
            );";

            using var cmd = new SqliteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public void Add(Animal animal)
        {
            using var conn = new SqliteConnection($"Data Source={_dbPath}");
            conn.Open();

            var cmd = new SqliteCommand("INSERT INTO Animals (Name, Age, Type) VALUES (@n, @a, @t)", conn);
            cmd.Parameters.AddWithValue("@n", animal.Name);
            cmd.Parameters.AddWithValue("@a", animal.Age);
            cmd.Parameters.AddWithValue("@t", animal.GetType().Name);
            cmd.ExecuteNonQuery();
        }

        public void Remove(Animal animal)
        {
            using var conn = new SqliteConnection($"Data Source={_dbPath}");
            conn.Open();

            var cmd = new SqliteCommand("DELETE FROM Animals WHERE Name=@n AND Type=@t", conn);
            cmd.Parameters.AddWithValue("@n", animal.Name);
            cmd.Parameters.AddWithValue("@t", animal.GetType().Name);
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Animal> GetAll()
        {
            var list = new List<Animal>();
            using var conn = new SqliteConnection($"Data Source={_dbPath}");
            conn.Open();

            var cmd = new SqliteCommand("SELECT Name, Age, Type FROM Animals", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                int age = reader.GetInt32(1);
                string type = reader.GetString(2);

                Animal a = type switch
                {
                    "Cat" => new Cat { Name = name, Age = age },
                    "Dog" => new Dog { Name = name, Age = age },
                    "Bird" => new Bird { Name = name, Age = age },
                    "Monkey" => new Monkey { Name = name, Age = age },
                    "Fox" => new Fox { Name = name, Age = age },
                    "Horse" => new Horse { Name = name, Age = age },
                    "Elephant" => new Elephant { Name = name, Age = age },
                    "Wolf" => new Wolf { Name = name, Age = age },
                    "Tiger" => new Tiger { Name = name, Age = age },
                    _ => new Cat { Name = name, Age = age }
                };

                list.Add(a);
            }

            return list;
        }

        public Animal? Find(Func<Animal, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }
    }
}
