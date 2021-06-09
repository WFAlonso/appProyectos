using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Sesiones
    {
        public int UsuarioId { get; set; }
        public string token { get; set; }
        public DateTime FechaIni { get; set; }
    }
}
