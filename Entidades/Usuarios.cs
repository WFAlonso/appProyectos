using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Usuarios
    {
        public string token { get; set; }
        public string Nombre { get; set; }
        public int PerfilId { get; set; }
        public string Clave { get; set; }
        public bool Habilitado { get; set; }

    }
}
