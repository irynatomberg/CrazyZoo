using CrazyZoo.application.interfaces;
using CrazyZoo.domain.entity;
using CrazyZoo.infrastructure.database;
using CrazyZoo.infrastructure.logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace CrazyZoo
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            var sc = new ServiceCollection();

            sc.AddDbContext<ZooDbContext>(opt =>
                opt.UseSqlite("Data Source=zoo.db"));

            sc.AddSingleton<ILogger, XmlLogger>();
            sc.AddScoped<IRepository<Animal>, EfAnimalRepository>();

            Services = sc.BuildServiceProvider();
        }
    }
}
