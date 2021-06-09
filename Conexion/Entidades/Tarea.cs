using System;
using System.Collections.Generic;

#nullable disable

namespace Base_de_datos.Entidades
{
    public partial class Tarea
    {
        public int TareaId { get; set; }
        public int ProyectoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcon { get; set; }
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public virtual Proyecto Proyecto { get; set; }
    }
}
