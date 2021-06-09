using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Funcionario
    {
        public int IdCreador { get; set; }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string User { get; set; }
        public int PerfilId { get; set; }
        public string Clave { get; set; }
        public bool Habilitado { get; set; }
        public string correo { get; set; }
    }
}
