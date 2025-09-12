using GoldenTasksDesktop.Data;
using GoldenTasksDesktop.Data.Repositories;
using GoldenTasksDesktop.Services;
using GoldenTasksDesktop.Services.NavigationData;
using GoldenTasksDesktop.ViewModels;
using GoldenTasksDesktop.Views;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;


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
            //GoldenTasksDbContext.Database.Migrate();

            INavigationService? navigationService = null;

            BaseViewModel viewModelFactory(Type vmType, object? param = null) //Por ahora pongo un object nulleable dependiendo de qué parámetro extra necesite el VM.
            {
                if (navigationService == null)
                {
                    throw new Exception("El NavigationService no fue inicializado.");
                }

                if (vmType == typeof(LoginViewModel))
                {
                    return new LoginViewModel(new UsuarioRepository(GoldenTasksDbContext!), navigationService);
                }
                if (vmType == typeof(CrearUsuarioViewModel))
                {
                    return new CrearUsuarioViewModel(new UsuarioRepository(GoldenTasksDbContext!), navigationService);
                }
                if (vmType == typeof(DashboardViewModel))
                {
                    if (param != null) //&& typeof(param) == typeof(int)) 
                    {
                        return new DashboardViewModel(new UsuarioRepository(GoldenTasksDbContext!), new TareaRepository(GoldenTasksDbContext!), navigationService, Convert.ToInt32(param));
                    }
                }
                if (vmType == typeof(AgregarTareaViewModel))
                {
                    if(param is AgregarTareaParams agregarTareaParams)
                    {
                        return new AgregarTareaViewModel(agregarTareaParams.IdUsuario, new TareaRepository(GoldenTasksDbContext!), navigationService, agregarTareaParams.OnAgregarTarea);
                    }
                }
                throw new Exception("Error al asignar un ViewModel o error en sus dependencias.");
            }

            navigationService = new NavigationService(viewModelFactory);

            navigationService.NavegarA<LoginViewModel>();

            base.OnStartup(e);
        }
    }


}

