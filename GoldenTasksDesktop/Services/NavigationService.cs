using GoldenTasksDesktop.ViewModels;
using GoldenTasksDesktop.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoldenTasksDesktop.Services
{
    public class NavigationService(Func<Type, object?, BaseViewModel> viewModelFactory) : INavigationService
    {
        private readonly Func<Type, object?, BaseViewModel> _viewModelFactory = viewModelFactory;

        public void NavegarA<TViewModel>(object? param = null) where TViewModel : BaseViewModel
        {
            var viewModel = _viewModelFactory(typeof(TViewModel), param);
            var window = CrearViewParaViewModel(viewModel);

            window.DataContext = viewModel;

            viewModel.RequestClose += (_, _) => window.Close();

            window.Show();
        }

        private static Window CrearViewParaViewModel(BaseViewModel viewModel)
        {
            if (viewModel is LoginViewModel) 
            {
                return new LoginView();
            }
            if (viewModel is CrearUsuarioViewModel)
            {
                return new CrearUsuarioView();
            }
            if (viewModel is DashboardViewModel)
            {
                return new DashboardView();
            }
            if(viewModel is AgregarTareaViewModel)
            {
                return new AgregarTareaView();
            }
            throw new Exception("No hay una View para ese ViewModel.");
        }
    }
}
