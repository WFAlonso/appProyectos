using System;
using System.Collections.Generic;

#nullable disable

namespace Base_de_datos.Entidades
{
    public partial class Sesion
    {
        public int UsuarioId { get; set; }
        public string Token { get; set; }
        public DateTime FechaIni { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
