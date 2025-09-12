using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenTasksDesktop.Models
{
    [Table("Tarea")]
    public class Tarea
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Nombre { get; set; }
        [Required]
        public required string Descripcion { get; set; }
        [Required]
        public DateTime FechaDeExpiracion { get; set; }
        [Required]
        [Range(1,3)]
        public int Clasificacion { get; set; }
        [Required]
        public string Estado { get; set; } = "INCOMPLETA"; //Enum sugerencia
        [Required]
        public bool Archivada { get; set; } = false;
        [Required]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
