using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Base_de_datos.Entidades;

namespace ServicioProyecto
{
    public class JwtAuthenticationServices : IJwtAuthenticationServices
    {
        private readonly AppDbContext _context;
        private readonly string _key;

        public JwtAuthenticationServices(string key, AppDbContext context)
        {
            _key = key;
            _context = context;
        }

        public string Autenticate(string username, string password)
        {
            var obj = _context.Usuarios.Where(u => u.User == username && u.Clave == password).FirstOrDefault();

            if ((string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) ||  (obj != null) || (!obj.Habilitado))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials =  new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
