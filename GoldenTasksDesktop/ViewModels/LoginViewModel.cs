using GoldenTasksDesktop.Commands;
using GoldenTasksDesktop.Data.Repositories;
using GoldenTasksDesktop.Models;
using GoldenTasksDesktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GoldenTasksDesktop.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INavigationService _navigationService;

        #region Atributos de UI

        private string _userNameOrEmail = "";
        public string UserNameOrEmail
        {
            get => _userNameOrEmail;
            set
            {
                _userNameOrEmail = value;
                OnPropertyChanged(nameof(UserNameOrEmail));
            }
        }
        private string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _mensaje = "";
        public string Mensaje
        {
            get => _mensaje;
            set
            {
                _mensaje = value;
                OnPropertyChanged(nameof(Mensaje));
            }
        }
        #endregion

        #region Comandos
        public ICommand IniciarSesionCommand { get; }
        #endregion

        public LoginViewModel(IUsuarioRepository usuarioRepository, INavigationService navigationService)
        {
            _usuarioRepository = usuarioRepository;
            _navigationService = navigationService;

            IniciarSesionCommand = new RelayCommand(async _ => await IniciarSesionAsync(UserNameOrEmail, Password)); //,_ => PuedeIniciarSesion(UserName, Password));
        }
        public async Task IniciarSesionAsync(string userNameOrEmail, string password)
        {
            Usuario? usuario;
            string userNameOrEmailFormateado = userNameOrEmail.ToLower().Trim();
            string passwordFormateado = password.Trim();
            bool esCorreo = false;

            if (String.IsNullOrEmpty(userNameOrEmail) || String.IsNullOrEmpty(passwordFormateado))
            {
                Mensaje = "Llena todos los campos.";
                return;
            }

            try
            {
                MailAddress m = new MailAddress(userNameOrEmailFormateado);
                usuario = await _usuarioRepository.ObtenerUsuarioPorEmailAsync(userNameOrEmailFormateado);
                esCorreo = true;
            }
            catch (FormatException)
            {
                usuario = await _usuarioRepository.ObtenerUsuarioPorUserNameAsync(userNameOrEmailFormateado);
            }


            if (usuario != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, usuario.Password))
                {
                    Mensaje = "Inicio de sesión correcto.";
                    _navigationService.NavegarA<DashboardViewModel>(usuario.Id);
                    Close();
                }
                else
                {
                    Mensaje = "Contraseña incorrecta.";
                }
            }
            else
            {
                Mensaje = !esCorreo ? "Ese nombre de usuario no está registrado." : "Ese correo no está registrado.";
            }

        }

        /* Comentada por ahora
       public bool PuedeIniciarSesion(string username = "", string password = "")
       {
           if(!String.IsNullOrEmpty(username.Trim()) && !String.IsNullOrEmpty(password.Trim())) 
           {
               return true;
           }
           Mensaje = "Datos vacíos";

           return false;
       }*/
    }
}
