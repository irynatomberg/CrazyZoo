using CrazyZoo.domain.entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CrazyZoo.infrastructure.database
{
    public class ZooDbContext : DbContext
    {
        public DbSet<Animal> Animals { get; set; }

        public ZooDbContext(DbContextOptions<ZooDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>()
                .HasKey(a => a.Id);        

            modelBuilder.Entity<Cat>();
            modelBuilder.Entity<Dog>();
            modelBuilder.Entity<Bird>();
            modelBuilder.Entity<Monkey>();
            modelBuilder.Entity<Fox>();
            modelBuilder.Entity<Horse>();
            modelBuilder.Entity<Elephant>();
            modelBuilder.Entity<Wolf>();
            modelBuilder.Entity<Tiger>();
        }
    }
}
