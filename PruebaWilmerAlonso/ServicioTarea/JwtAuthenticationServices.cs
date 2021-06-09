using Base_de_datos.Entidades;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

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
        // DbContextOptions<AppDbContext>
        public string Autenticate(string username, string password)
        {
            var obj = _context.Usuarios.Where(u => u.User == username && u.Clave == password).FirstOrDefault();

            if ((string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ) && (obj != null))// || username != "prueba" || password != "123456")
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
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
