using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicioProyecto
{
    public interface IJwtAuthenticationServices
    {
        string Autenticate(string username, string password);
    }
}
