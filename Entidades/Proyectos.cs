using System;

namespace Entidades
{
    public class Proyectos
    {        
        public int ProyectoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }        
        public string Estado { get; set; }        
        public DateTime FechaInicio { get; set; }
        public DateTime FechFin { get; set; }
    }
}
