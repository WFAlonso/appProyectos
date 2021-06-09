using Base_de_datos.Entidades;
using Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;

namespace PruebaWilmerAlonso.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Registro")]
    public class UsuariosController : ControllerBase
    {
        #region Class Attributes
        private object _response = null;
        #endregion

        #region Class Constructor
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }
        #endregion



        [HttpGet]
        public ActionResult<Response> Get()
        {
            var obj = _context.Usuarios.ToList();

            _response = new Response()
            {
                Object = obj,
                StatusCode = (int)HttpStatusCode.OK,
            };
            return Ok(_response);
        }

        [HttpGet("[controller]/{id}")]
        public ActionResult<Response> Get(int id)
        {
            var obj = UsuarioDetail(id);

            _response = new Response()
            {
                Object = obj,
                StatusCode = (int)HttpStatusCode.OK,
            };
            return Ok(_response);
        }

        [HttpPost]
        public ActionResult<Response> Post(Funcionario usuario)
        {
            if (ModelState.IsValid)
            {
                Usuario obj = UsuarioDetail(usuario.IdCreador);

                if (obj.PerfilId == 1)
                {
                    var usr = new Usuario()
                    {
                        Clave = usuario.Clave,
                        Nombre = usuario.Nombre,
                        PerfilId = usuario.PerfilId,
                        Habilitado = usuario.Habilitado,
                        Id = usuario.Id,
                        Correo = usuario.correo,
                        User = usuario.User
                    };

                    _context.Add(usr);
                    var c = _context.SaveChangesAsync().Result;

                    if (c != 1)
                        EnviarCorreo("adminCorreo@gmail.com", usuario.correo, usuario.Nombre, usuario.User, usr.Clave);

                    _response = new Response()
                    {
                        Object = usuario,
                        mensaje = "Creación existosa",
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                    return Ok(_response);
                }
            }
            return BadRequest();
        }

        [HttpPut("[controller]/{id}")]
        public ActionResult Put(int id, Funcionario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (usuario.PerfilId == 0)
                    {
                        BadRequest();
                    }

                    var per = PerfilDetail(usuario.PerfilId);
                    if (per.Count == 0)
                    {
                        BadRequest();
                    }
                    
                    var usr = new Usuario()
                    {
                        Clave = usuario.Clave,
                        Nombre = usuario.Nombre,
                        PerfilId = usuario.PerfilId,
                        Habilitado = usuario.Habilitado,
                        Id = id,
                        Perfil = per[0],
                    };

                    _context.Update(usr);
                    var c =_context.SaveChangesAsync().Result;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var obj = UsuarioDetail(id);
                _response = new Response()
                {
                    Object = obj,
                    mensaje = "Actualización existosa",
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }

             return Ok(_response);
        }

        [HttpPut("[controller]/CambioClave/{id}")]
        public ActionResult PutPass(int id, CambioCalve usuario)
        {
            if (ModelState.IsValid)
            {
                Usuario usrClave = _context.Usuarios.Where(u => u.Id == id)
                    .Where(u => u.Clave == usuario.ClaveIn).FirstOrDefault();

                if (usrClave == null)
                    return BadRequest();

                usrClave.Clave = usuario.ClaveNew;
                _context.Update(usrClave);
                var c = _context.SaveChangesAsync().Result;

                if (c != 1)
                {
                    var obj = UsuarioDetail(id);
                    _response = new Response()
                    {
                        Object = obj,
                        mensaje = "Actualización existosa",
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                }
                return Ok(_response);
            }

            return BadRequest();
        }

        [HttpPut("[controller]/CambioEstado/{id}")]
        public ActionResult PutEstado(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usr = UsuarioDetail(id);

                    usr.Habilitado = !usr.Habilitado;
                    var c = _context.SaveChangesAsync().Result;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var obj = UsuarioDetail(id);
                _response = new Response()
                {
                    Object = obj,
                    mensaje = "Actualización existosa",
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }

            return Ok(_response);
        }

        [HttpDelete]
        public ActionResult DeleteConfirmed(int id)
        {
            var usuario = _context.Usuarios.FindAsync(id).Result;
            _context.Usuarios.Remove(usuario);
            var d = _context.SaveChangesAsync().Result;

            _response = new Response()
            {
                mensaje = "Eliminación existosa",
                StatusCode = (int)HttpStatusCode.OK,
            };

            return Ok(_response);
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        private Usuario UsuarioDetail(int id)
        {
            return _context.Usuarios.Where(u => u.Id == id).FirstOrDefault();
        }

        private List<Perfile> PerfilDetail(int id)
        {
            var vart = _context.Perfiles.Where(u => u.Id == id).ToList();
            return vart;
        }

        private static void EnviarCorreo(string deCorreo, string paraCorreo, string nombre, string usr, string pass)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("Administrador",
                deCorreo);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(nombre,
                paraCorreo);
                message.To.Add(to);

                message.Subject = "Envío contraseña";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<h1>Hola, se te asignó el usuario!</h1><br>" +
                    "<h2>usuario: "+ usr + "</h2><br><h2>clave: " + pass + " !</h2>";
                bodyBuilder.TextBody = "Hola, se te asignó el usuario!" +
                    "usuario: " + usr + "clave: " + pass + ""; 

                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("prueba@gmail", "L1nD4@PRU#b@0912");

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
            }
            catch 
            {
            }
        }
    }
}
