using GoldenTasksDesktop.Commands;
using GoldenTasksDesktop.Data.Repositories;
using GoldenTasksDesktop.Models;
using GoldenTasksDesktop.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace GoldenTasksDesktop.ViewModels
{
    public class CrearUsuarioViewModel : BaseViewModel
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INavigationService _navigationService;

        #region Atributos de UI

        private string _nombre = "";
        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }
        private string _email = "";
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private string _userName = "";
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
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
        private string _confirmPassword = "";
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
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

        public ICommand CrearUsuarioCommand { get; }
        public ICommand RegresarALoginCommand { get; }

        public CrearUsuarioViewModel(IUsuarioRepository usuarioRepository, INavigationService navigationService)
        {
            _usuarioRepository = usuarioRepository;
            _navigationService = navigationService;

            CrearUsuarioCommand = new RelayCommand(async _ => await AgregarUsuarioAsync(Nombre, Email, UserName, Password, ConfirmPassword));
            RegresarALoginCommand = new RelayCommand(_ => RegresarALogin());
        }

        public async Task AgregarUsuarioAsync(string nombre, string email, string userName, string password, string confirmPassword)
        {
            if (!String.IsNullOrEmpty(nombre.Trim()) && !String.IsNullOrEmpty(email.Trim())
                && !String.IsNullOrEmpty(userName.Trim()) && !String.IsNullOrEmpty(password.Trim())
                && !String.IsNullOrEmpty(confirmPassword.Trim()))
            {
                if (password.Trim() == confirmPassword.Trim())
                {
                    Usuario usuario = new Usuario()
                    {
                        Nombre = nombre.Trim(),
                        Email = email.Trim().ToLower(),
                        UserName = userName.Trim().ToLower(),
                        Password = BCrypt.Net.BCrypt.HashPassword(password.Trim())
                    };

                    try
                    {
                        MailAddress m = new(email.Trim());
                        var uUserName = await _usuarioRepository.ObtenerUsuarioPorUserNameAsync(userName.Trim());
                        var uEmail = await _usuarioRepository.ObtenerUsuarioPorEmailAsync(email.Trim());

                        if (uEmail != null)
                        {
                            Mensaje = "Ese correo ya está en uso.";
                        }
                        else if (uUserName != null)
                        {
                            Mensaje = "Ese nombre de usuario ya está en uso.";
                        }
                        else
                        {
                            await _usuarioRepository.AgregarUsuarioAsync(usuario);
                            Nombre = "";
                            Email = "";
                            UserName = "";
                            Password = "";
                            ConfirmPassword = "";
                            Mensaje = "¡Te has registrado exitosamente!";
                        }
                    }
                    catch (FormatException)
                    {
                        Mensaje = "Ingresa un correo válido.";
                    }

                }
                else
                {
                    Mensaje = "¡No has confirmado correctamente tu contraseña!";
                }
            }
            else
            {
                Mensaje = "Tienes uno o más campos vacíos.";
            }
        }

        public void RegresarALogin()
        {
            _navigationService.NavegarA<LoginViewModel>();
            Close();
        }
    }
}
