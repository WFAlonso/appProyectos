using System;
using System.Collections.Generic;

#nullable disable

namespace Base_de_datos.Entidades
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int PerfilId { get; set; }
        public string Clave { get; set; }
        public string User { get; set; }
        public bool Habilitado { get; set; }
        public string Correo { get; set; }
        
        public virtual Perfile Perfil { get; set; }
    }
}
