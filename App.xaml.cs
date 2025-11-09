using CrazyZoo.entity;
using CrazyZoo.interfaces;
using CrazyZoo.logging;
using CrazyZoo.repositories;
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
            base.OnStartup(e);

            var services = new ServiceCollection();

            string dbPath = "crazyzoo.db";

            services.AddSingleton<ILogger, JsonLogger>();

            services.AddSingleton<IRepository<Animal>>(sp => new DatabaseRepository(dbPath));

            Services = services.BuildServiceProvider();
        }
    }
}
