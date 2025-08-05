using GoldenTasksDesktop.Data;
using GoldenTasksDesktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GoldenTasksDesktop.Views
{
    /// <summary>
    /// Lógica de interacción para DashboardView.xaml
    /// </summary>
    public partial class DashboardView : Window
    {
        private readonly GoldenTasksDbContext? _context;
        public DashboardView()
        {
            _context = App.GoldenTasksDbContext;

            if(_context != null)
            {
                UsuarioViewModel usuarioViewModel = new(_context);
                this.DataContext = usuarioViewModel;
            }

            InitializeComponent();
        }
    }
}
