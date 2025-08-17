using GoldenTasksDesktop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace GoldenTasksDesktop.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private GoldenTasksDbContext _context { get; set; }
        public UsuarioRepository(GoldenTasksDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Usuario>> ObtenerUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }
        public async Task AgregarUsuarioAsync(Usuario usuario)
        {
            try
            {  
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception();
                //Console.WriteLine(ex.ToString()); //Cambiar luego
            }
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public Usuario? ObtenerUsuarioPorId(int id)
        {
            return _context.Usuarios.Find(id);
        }
        public async Task<Usuario?> ObtenerUsuarioPorUserNameAsync(string username)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<Usuario?> ObtenerUsuarioPorEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ICollection<Tarea>> ObtenerTareasDelUsuarioAsync(int id)
        {
            var usuario = await ObtenerUsuarioPorIdAsync(id);
            return usuario!.Tareas!;
        }
    }
}
