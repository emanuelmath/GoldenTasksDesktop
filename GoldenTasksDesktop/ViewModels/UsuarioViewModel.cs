using GoldenTasksDesktop.Data.Repositories;
using GoldenTasksDesktop.Models;
using GoldenTasksDesktop.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GoldenTasksDesktop.Data;
using System.Windows.Input;

namespace GoldenTasksDesktop.ViewModels
{
    public class UsuarioViewModel : BaseViewModel
    {
        private readonly GoldenTasksDbContext _context;
        private readonly IUsuarioRepository _usuarioRepository;

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

        public UsuarioViewModel(GoldenTasksDbContext context)
        {
           _context = context;
           _usuarioRepository = new UsuarioRepository(context);

            IniciarSesionCommand = new RelayCommand(async _ => await IniciarSesionAsync(UserName, Password), _ => PuedeIniciarSesion(UserName, Password));
            CrearUsuarioCommand = new RelayCommand(async _ => await AgregarUsuarioAsync(Nombre, Email, UserName, Password, ConfirmPassword));
        }

        public ICommand IniciarSesionCommand { get; }
        public ICommand CrearUsuarioCommand { get; }
        public async Task IniciarSesionAsync(string userNameOrEmail, string password)
        {
           Usuario? usuario = await _usuarioRepository.ObtenerUsuarioPorUserNameAsync(userNameOrEmail.ToLower().Trim());

           if(usuario != null)
           {
                if(BCrypt.Net.BCrypt.Verify(password, usuario.Password))
                {
                    Mensaje = "Inicio de sesión correcto.";
                }
                else
                {
                    Mensaje = "Contraseña incorrecta.";
                }
           }
           else
           {
               Mensaje = "No existe el usuario con esas credenciales.";
           }

        }

        public bool PuedeIniciarSesion(string username = "", string password = "")
        {
            if(!String.IsNullOrEmpty(username.Trim()) && !String.IsNullOrEmpty(password.Trim())) 
            {
                return true;
            }
            Mensaje = "Datos vacíos";
            Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("12345"));
            return false;
        }

        public async Task AgregarUsuarioAsync(string nombre, string email, string userName, string password, string confirmPassword)
        {
            if(!String.IsNullOrEmpty(nombre.Trim()) && !String.IsNullOrEmpty(email.Trim()) 
                && !String.IsNullOrEmpty(userName.Trim()) && !String.IsNullOrEmpty(password.Trim()) 
                && !String.IsNullOrEmpty(confirmPassword.Trim()))
            {
                if(userName.Trim() == password.Trim())
                {
                    Usuario usuario = new Usuario()
                    {
                        Nombre = nombre.Trim(),
                        Email = email.Trim().ToLower(),
                        UserName = userName.Trim().ToLower(),
                        Password = BCrypt.Net.BCrypt.HashPassword(password.Trim())
                    };

                    await _usuarioRepository.AgregarUsuarioAsync(usuario);
                    Mensaje = "¡Te has registrado exitosamente!";
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


    }
}
