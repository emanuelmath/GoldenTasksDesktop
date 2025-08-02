using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoldenTasksDesktop.Models;

namespace GoldenTasksDesktop.Data
{
    public class GoldenTasksDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Medalla> Medallas { get; set; }

        public GoldenTasksDbContext (DbContextOptions<GoldenTasksDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Usuario>().HasIndex(u => u.UserName).IsUnique();
        }

    }
}
