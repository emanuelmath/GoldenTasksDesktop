using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoldenTasksDesktop.Models;
using Microsoft.EntityFrameworkCore;

namespace GoldenTasksDesktop.Data.Repositories
{
    public class TareaRepository(GoldenTasksDbContext dbContext) : ITareaRepository
    {
        private readonly GoldenTasksDbContext _dbContext = dbContext;
        public async Task<ICollection<Tarea>> ObtenerTareasDelUsuarioAsync(int idUsuario)
        {
            return await _dbContext.Tareas.Where(t => t.IdUsuario == idUsuario).ToListAsync();
        }

        public ICollection<Tarea> ObtenerTareasDelUsuario(int idUsuario)
        {
            return [.. _dbContext.Tareas.Where(t => t.IdUsuario == idUsuario)];
        }

        public async Task AgregarTareaAsync(Tarea tarea)
        {
            _dbContext.Tareas.Add(tarea);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditarTareaAsync(int id, Tarea tarea) 
        {
            var tareaAEditar = await _dbContext.Tareas.FindAsync(id);//FirstOrDefaultAsync(t => t.IdUsuario == id);
            tareaAEditar = tarea;
            await _dbContext.SaveChangesAsync();    
        }

        public async Task EliminarTareaAsync(int id)
        {
            var tareaABorrar = await _dbContext.Tareas.FindAsync(id);
            _dbContext.Tareas.Remove(tareaABorrar!);
            await _dbContext.SaveChangesAsync();
        }
    }
}
