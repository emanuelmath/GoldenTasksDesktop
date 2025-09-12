using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenTasksDesktop.Services.NavigationData
{
    public class AgregarTareaParams
    {
        public int IdUsuario { get; set; }
        public Action<bool>? OnAgregarTarea { get; set; } 
    }
}
