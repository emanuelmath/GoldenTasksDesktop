using GoldenTasksDesktop.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;

namespace GoldenTasksDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static GoldenTasksDbContext? GoldenTasksDbContext { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var options = new DbContextOptionsBuilder<GoldenTasksDbContext>()
                .UseSqlite("Data Source=C:\\Temp\\GoldenTasks.db")
                .Options;

            GoldenTasksDbContext = new GoldenTasksDbContext(options);
            GoldenTasksDbContext.Database.Migrate();

            base.OnStartup(e);
        }
    }

}
