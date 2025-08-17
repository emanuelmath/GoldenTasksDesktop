using GoldenTasksDesktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoldenTasksDesktop.Services
{
    public interface INavigationService
    {
        void NavegarA<TViewModel>(object? param = null) where TViewModel : BaseViewModel;
    }
}
