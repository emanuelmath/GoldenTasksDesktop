using GoldenTasksDesktop.Commands;
using GoldenTasksDesktop.Data;
using GoldenTasksDesktop.Data.Repositories;
using GoldenTasksDesktop.Models;
using GoldenTasksDesktop.Services;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GoldenTasksDesktop.ViewModels
{
    public class AgregarTareaViewModel : BaseViewModel
    {
        private readonly int _idUsuario;
        private readonly ITareaRepository _tareaRepository;
        private readonly INavigationService _navigationService;
        private readonly Action<bool>? _onAgregarTarea;

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
        private string _descripcion = "";
        public string Descripcion
        {
            get => _descripcion;
            set
            {
                _descripcion = value;
                OnPropertyChanged(nameof(Descripcion));
            }
        }

        private DateTime _fechaDeExpiracion = DateTime.Now.Date;
        public DateTime FechaDeExpiracion
        {
            get => _fechaDeExpiracion;
            set
            {
                _fechaDeExpiracion = value;
                OnPropertyChanged(nameof(FechaDeExpiracion));
            }
        }

        private string _horaDeExpiracion = DateTime.Now.Hour.ToString();
        public int HoraDeExpiracion
        {
            get => Convert.ToInt32(_horaDeExpiracion);
            set
            {
                _horaDeExpiracion = value.ToString();
                OnPropertyChanged(nameof(HoraDeExpiracion));
            }
        }

        private string _minutoDeExpiracion = DateTime.Now.Minute.ToString();
        public int MinutoDeExpiracion
        {
            get => Convert.ToInt32(_minutoDeExpiracion);
            set
            {
                _minutoDeExpiracion = value.ToString();
                OnPropertyChanged(nameof(MinutoDeExpiracion));
            }
        }

        private bool _radioButtonOro;
        public bool RadioButtonOro
        {
            get => _radioButtonOro;
            set
            {
                _radioButtonOro = value;
                OnPropertyChanged(nameof(RadioButtonOro));
            }
        }
        private bool _radioButtonPlata;
        public bool RadioButtonPlata
        {
            get => _radioButtonPlata;
            set
            {
                _radioButtonPlata = value;
                OnPropertyChanged(nameof(RadioButtonPlata));
            }
        }
        private bool _radioButtonBronce;
        public bool RadioButtonBronce
        {
            get => _radioButtonBronce;
            set
            {
                _radioButtonBronce = value;
                OnPropertyChanged(nameof(RadioButtonBronce));
            }
        }

        private string _mensaje;
        public string Mensaje
        {
            get => _mensaje;
            set
            {
                _mensaje = value;
                OnPropertyChanged(nameof(Mensaje));
            }
        }

        public List<int> Horas { get; } = [.. Enumerable.Range(1, 24)];
        public List<int> Minutos { get; } = [.. Enumerable.Range(0, 59)];

        public ICommand CerrarCommand { get; }
        public ICommand AgregarTareaCommand { get; }

        public AgregarTareaViewModel(int idUsuario, ITareaRepository tareaRepository, INavigationService navigationService, Action<bool>? onAgregarTarea)
        {
            _idUsuario = idUsuario;
            _tareaRepository = tareaRepository;
            _navigationService = navigationService;
            _onAgregarTarea = onAgregarTarea;

            CerrarCommand = new RelayCommand(_ => NavegarADashboard());
            AgregarTareaCommand = new RelayCommand(async _ => await AgregarTarea(_idUsuario, Nombre, Descripcion, FechaDeExpiracion));
        }

        public void NavegarADashboard()
        {
            Close();
        }

        public async Task AgregarTarea(int idUsuario, string nombre, string descripcion, DateTime fechaDeExpiracion)
        {
            try
            {
                if (!String.IsNullOrEmpty(nombre.Trim()) && !String.IsNullOrEmpty(descripcion.Trim())
                  && (RadioButtonBronce == true || RadioButtonPlata == true || RadioButtonOro == true) && !String.IsNullOrEmpty(fechaDeExpiracion.ToString()))
                {
                   if(FechaDeExpiracion == DateTime.Now.Date)
                   {
                        if(HoraDeExpiracion >= DateTime.Now.Hour && MinutoDeExpiracion >= DateTime.Now.Minute + 5)
                        {
                            DateTime fechaDeExpiracionCompleta = new(FechaDeExpiracion.Year, FechaDeExpiracion.Month,
                                FechaDeExpiracion.Day, HoraDeExpiracion, MinutoDeExpiracion, 0);

                            Tarea tarea = new()
                            {
                                Nombre = nombre.Trim(),
                                Descripcion = descripcion.Trim(),
                                //Estado = "INCOMPLETA", -> En el modelo se pone automáticamente. 
                                Archivada = false, // -> No ha sido completada para archivar.
                                Clasificacion = AsignarClasificacion(RadioButtonBronce, RadioButtonPlata, RadioButtonOro),
                                IdUsuario = idUsuario,
                                FechaDeExpiracion = fechaDeExpiracionCompleta

                            };

                            await _tareaRepository.AgregarTareaAsync(tarea);
                            _onAgregarTarea?.Invoke(true);

                            Nombre = "";
                            Descripcion = "";
                            RadioButtonBronce = false;
                            RadioButtonPlata = false;
                            RadioButtonOro = false;
                            FechaDeExpiracion = DateTime.Today;
                            HoraDeExpiracion = 0;
                            MinutoDeExpiracion = 0;
                            Mensaje = "";

                            MessageBox.Show($"¡La tarea {tarea.Nombre} ha sido agregada correctamente!");
                            Close();
                        }
                        else
                        {
                            Mensaje = "Debes tener mínimo 5 minutos para hacer una tarea.";
                        }
                   }
                   else if(FechaDeExpiracion > DateTime.Now.Date)
                   {
                        DateTime fechaDeExpiracionCompleta = new(FechaDeExpiracion.Year, FechaDeExpiracion.Month,
                                FechaDeExpiracion.Day, HoraDeExpiracion, MinutoDeExpiracion, 0);

                        Tarea tarea = new()
                        {
                            Nombre = nombre.Trim(),
                            Descripcion = descripcion.Trim(),
                            //Estado = "INCOMPLETA", -> En el modelo se pone automáticamente. 
                            Archivada = false, // -> No ha sido completada para archivar.
                            Clasificacion = AsignarClasificacion(RadioButtonBronce, RadioButtonPlata, RadioButtonOro),
                            IdUsuario = idUsuario,
                            FechaDeExpiracion = fechaDeExpiracionCompleta

                        };

                        await _tareaRepository.AgregarTareaAsync(tarea);
                        _onAgregarTarea?.Invoke(true);

                        Nombre = "";
                        Descripcion = "";
                        RadioButtonBronce = false;
                        RadioButtonPlata = false;
                        RadioButtonOro = false;
                        FechaDeExpiracion = DateTime.Today;
                        HoraDeExpiracion = 0;
                        MinutoDeExpiracion = 0;
                        Mensaje = "";

                        MessageBox.Show($"¡La tarea {tarea.Nombre} ha sido agregada correctamente!");
                        Close();
                   }
                   else
                   {
                        Mensaje = "Debes poner la fecha de hoy o posterior.";
                   }
                }
                else
                {
                    Mensaje = "Faltan datos a llenar.";
                }            
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        public int AsignarClasificacion(bool bronce, bool plata, bool oro)
        {
            if(bronce && !plata && !oro)
            {
                return 1;
            }
            else if (!bronce && plata && !oro)
            {
                return 2;
            }
            else if (!bronce && !plata && oro)
            {
                return 3;
            }
            throw new Exception();
 
        }

        /*
        Func<bool, bool, bool, int> onClasificacionElegida
         (b1, b2, b3) =>
            {
                if(b1 && !b2 && !b3)
                {
                    return 1;
                }
                else if(!b1 && b2 && !b3)
                {
                    return 2;
                }
                else if(!b1 && !b2 && b3)
                {
                    return 3;
                }
                throw new Exception();
            },
         */
    }
}
