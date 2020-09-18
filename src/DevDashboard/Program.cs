using DevDashboard.Components;
using DevDashboard.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Terminal.Gui;

namespace DevDashboard
{
    class Program
    {
        // TODO: check into GitHub (squash *before* checking in and *after* secrets implemented!!)
        static IServiceProvider services = ConfigureServices();

        static void Main(string[] args)
        {
            Application.Init();

            // Add Components
            var today = InitDashboardComponent<Today>(x: 0, y: 0, height: 12);
            var whiteboard = InitDashboardComponent<TrelloBoard>(x: 0, y: Pos.Bottom(today.Frame) + 1, height: 10);
            var gitInfo = InitDashboardComponent<GitInfo>(x: 0, y: Pos.Bottom(whiteboard.Frame) + 1, height: 22);

            Application.Run();
        }

        private static IServiceProvider ConfigureServices()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(config);
            services.AddTransient<Today>();
            services.AddTransient<TrelloBoard>();
            services.AddTransient<TrelloClient>();
            services.AddTransient<GitInfo>();
            return services.BuildServiceProvider();
        }

        private static T InitDashboardComponent<T>(Pos x, Pos y, int height) where T : DashComponent
        {
            var component = services.GetService<T>();
            component.Init(Application.Top, x: x, y: y, height: height, width: Dim.Fill(), Colors.TopLevel);
            component.Setup();
            return component;
        }
    }
}
