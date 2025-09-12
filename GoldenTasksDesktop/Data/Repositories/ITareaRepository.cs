using GoldenTasksDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenTasksDesktop.Data.Repositories
{
    public interface ITareaRepository
    {
        Task<ICollection<Tarea>> ObtenerTareasDelUsuarioAsync(int idUsuario);
        ICollection<Tarea> ObtenerTareasDelUsuario(int idUsuario);
        Task AgregarTareaAsync(Tarea tarea);
        Task EditarTareaAsync(int id, Tarea tarea);
        Task EliminarTareaAsync(int id);

    }
}
