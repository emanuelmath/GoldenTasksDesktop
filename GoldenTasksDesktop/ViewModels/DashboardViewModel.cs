using GoldenTasksDesktop.Commands;
using GoldenTasksDesktop.Data.Repositories;
using GoldenTasksDesktop.Models;
using GoldenTasksDesktop.Services;
using GoldenTasksDesktop.Services.NavigationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GoldenTasksDesktop.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITareaRepository _tareaRepository;
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

        public ICommand AgregarTareaCommand { get; }
        public DashboardViewModel(IUsuarioRepository usuarioRepository, ITareaRepository tareaRepository, INavigationService navigationService, int idUsuario)
        {
            _usuarioRepository = usuarioRepository;
            _tareaRepository = tareaRepository;
            _navigationService = navigationService;
            _idUsuario = idUsuario;

            ObtenerUsuario(idUsuario);
            
            AgregarTareaCommand = new RelayCommand( _ => AgregarTarea(_idUsuario));
            
        }

        public void ObtenerUsuario(int idUsuario)
        {
            var user = _usuarioRepository.ObtenerUsuarioPorId(idUsuario);

            if (user != null)
            {
                Usuario = user;
                Usuario.Tareas = _tareaRepository.ObtenerTareasDelUsuario(idUsuario);

                //Nombre = user.Nombre;
            }

        }


        public void AgregarTarea(int idUsuario)
        {
            //try
            //{
            AgregarTareaParams agregarTareaParams = new AgregarTareaParams
            {
                IdUsuario = idUsuario,
                OnAgregarTarea = OnTareaAgregada
                //OnAgregarTarea = (seAgregoTarea) =>
                //{
                //    if (seAgregoTarea)
                //    {
                //        ObtenerUsuario(idUsuario);
                //    }
                //}
            };

            _navigationService.NavegarA<AgregarTareaViewModel>(agregarTareaParams);

            //ObtenerUsuario(idUsuario);
            //}
            //catch (Exception ex) 
            //{
            //    MessageBox.Show(ex.Message);    
            //}
        }

        private void OnTareaAgregada(bool tareaFueAgregada)
        {
            if (tareaFueAgregada)
            {
                Usuario.Tareas = _tareaRepository.ObtenerTareasDelUsuario(_idUsuario);
                OnPropertyChanged(nameof(Usuario));
            }
        }


    }
}
