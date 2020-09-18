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


        static ColorScheme baseColorScheme = Colors.TopLevel;

        static void Main(string[] args)
        {
            var services = ConfigureServices();

            Application.Init();
            var top = Application.Top;

            // Add Components
            var today = services.GetService<Today>();
            today.Init(Application.Top, x: 0, y: 0, height: 12, width: Dim.Fill(), baseColorScheme);
            today.Setup();

            var whiteboard = services.GetService<TrelloBoard>();
            whiteboard.Init(Application.Top, x: 0, y: Pos.Bottom(today.Frame) + 1, height: 10, width: Dim.Fill(), baseColorScheme);
            whiteboard.Setup();

            var gitInfo = services.GetService<GitInfo>();
            gitInfo.Init(Application.Top, x: 0, y: Pos.Bottom(whiteboard.Frame) + 1, height: 22, width: Dim.Fill(), baseColorScheme);
            gitInfo.Setup();


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
    }
}
