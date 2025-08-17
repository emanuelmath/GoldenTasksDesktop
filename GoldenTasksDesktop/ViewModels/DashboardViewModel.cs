using GoldenTasksDesktop.Data.Repositories;
using GoldenTasksDesktop.Models;
using GoldenTasksDesktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenTasksDesktop.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INavigationService _navigationService;
        private readonly int _idUsuario;

        private Usuario _usuario;
        public Usuario Usuario
        {
            get => _usuario;
            set
            {
                _usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        //Por ahora puedo bindear los atributos del objeto de usuario.
        /*private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }*/

        public DashboardViewModel(IUsuarioRepository usuarioRepository, INavigationService navigationService, int idUsuario)
        {
            _usuarioRepository = usuarioRepository;
            _navigationService = navigationService;
            _idUsuario = idUsuario;

            ObtenerUsuario(idUsuario);
            
        }

        public void ObtenerUsuario(int idUsuario)
        {
            var user = _usuarioRepository.ObtenerUsuarioPorId(idUsuario);

            if(user != null)
            {
                Usuario = user;
                //Nombre = user.Nombre;
            }
        }

    }
}
