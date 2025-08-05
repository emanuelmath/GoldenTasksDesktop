using GoldenTasksDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenTasksDesktop.Data.Repositories
{
    public interface IUsuarioRepository
    {
        Task<ICollection<Usuario>> ObtenerUsuariosAsync();
        Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);
        Task<Usuario?> ObtenerUsuarioPorUserNameAsync(string username);
        Task<Usuario?> ObtenerUsuarioPorEmailAsync(string email);
        Task AgregarUsuarioAsync(Usuario usuario);
        Task<ICollection<Tarea>> ObtenerTareasDelUsuarioAsync(int id);

        

    }
}
