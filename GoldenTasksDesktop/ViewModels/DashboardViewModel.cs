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
using System.Windows.Threading;

namespace GoldenTasksDesktop.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITareaRepository _tareaRepository;
        private readonly INavigationService _navigationService;
        private readonly int _idUsuario;
        private readonly DispatcherTimer _temporizadorDeTareas;

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
        public ICommand CompletarTareaCommand { get; }
        public DashboardViewModel(IUsuarioRepository usuarioRepository, ITareaRepository tareaRepository, INavigationService navigationService, int idUsuario)
        {
            _usuarioRepository = usuarioRepository;
            _tareaRepository = tareaRepository;
            _navigationService = navigationService;
            _idUsuario = idUsuario;
            _temporizadorDeTareas = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1),
            };

            ObtenerUsuario(idUsuario);

            _temporizadorDeTareas.Tick += Tiempo;
            _temporizadorDeTareas.Start();
            
            AgregarTareaCommand = new RelayCommand( _ => AgregarTarea(_idUsuario));
            CompletarTareaCommand = new RelayCommand(async param =>
            {
                if (param is int tareaId)
                {
                    await CompletarTarea(tareaId, OnTareaCompletada);
                }
            },
            param =>
            {
                if (param is int tareaId)
                {
                   return TareaYaCompletada(tareaId);
                }
                return true;
            });
            
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

        public async Task CompletarTarea(int tareaId, Action? onTareaCompletada)
        {
            bool tareaExpirada = await _tareaRepository.TareaExpiradaAsync(tareaId);
            if(!tareaExpirada)
            {
                await _tareaRepository.CompletarTareaAsync(tareaId);
                onTareaCompletada?.Invoke();
            }
        }

        private void OnTareaCompletada()
        {
            Usuario.Tareas = _tareaRepository.ObtenerTareasDelUsuario(_idUsuario);
            OnPropertyChanged(nameof(Usuario));
        }

        private bool TareaYaCompletada(int idTarea)
        {
            var usuario = Usuario.Tareas.FirstOrDefault(t => t.Id == idTarea);
            if (usuario != null)
            {
                return usuario.Estado != "COMPLETADA" && usuario.Estado != "EXPIRADA";
            }
            return false;
        }

        private async void Tiempo(object? sender, EventArgs args)
        {
            var fechaDelMomento = DateTime.Now;
            foreach(Tarea tarea in Usuario.Tareas)
            {
                if (tarea.FechaDeExpiracion <= fechaDelMomento)
                {
                    tarea.Estado = "EXPIRADA";
                    await OnTareaExpirada(tarea);
                }
            }
        }

        private async Task OnTareaExpirada(Tarea tarea)
        {
            await _tareaRepository.EditarTareaAsync(_idUsuario, tarea);
            Usuario.Tareas = _tareaRepository.ObtenerTareasDelUsuario(_idUsuario);
            OnPropertyChanged(nameof(Usuario));
        }
    }
}
