using Base_de_datos.Entidades;
using Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServicioRegistrar;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PruebaWilmerAlonso.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class AccesoController : ControllerBase
    {
        private readonly IJwtAuthenticationServices __authServices;

        public AccesoController(IJwtAuthenticationServices _authServices)
        {
            __authServices = _authServices;
        }

        [AllowAnonymous]
        [HttpGet]
        public object Get()
        {
            var response = new { Status = "Ejecutando" };
            return response;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Autenticate([FromBody] AuthInfo user)
        {
            var token = __authServices.Autenticate(user.usranme, user.password);
            
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}
