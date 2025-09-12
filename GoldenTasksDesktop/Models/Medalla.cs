using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenTasksDesktop.Models
{
    [Table("Medalla")]
    public class Medalla
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, 3)]
        public int Tipo { get; set; }
        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        [Required]
        [ForeignKey("Tarea")]
        public int IdTarea { get; set; }
        [Required]
        public DateTime FechaDeObtencion { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Tarea Tarea { get; set; }

    }
}
