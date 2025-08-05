using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenTasksDesktop.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Nombre { get; set; }
        [Required] 
        [EmailAddress]
        public required string Email { get; set; }
        [Required] 
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
        public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>(); //Relación. 1:M
        public ICollection<Medalla> Medallas { get; set; } = new List<Medalla>();  // Las medallas del usuario. 1:M.

    }
}
