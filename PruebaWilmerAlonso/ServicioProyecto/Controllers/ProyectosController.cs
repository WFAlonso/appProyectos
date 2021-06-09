using Base_de_datos.Entidades;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;

namespace ServicioTarea.Properties
{
    [ApiController]
    [Route("Proyectos")]
    public class ProyectosController : Controller
    {
        #region Class Attributes
        private object _response = null;
        #endregion

        #region Class Constructor
        private readonly AppDbContext _context;

        public ProyectosController(AppDbContext context)
        {
            _context = context;
        }
        #endregion



        [HttpGet]
        public ActionResult<Response> Get()
        {
            var obj = _context.Proyectos.ToList();

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
            var obj = ProyectoDetail(id);

            if (obj != null)
            {
                _response = new Response()
                {
                    Object = obj,
                    StatusCode = (int)HttpStatusCode.OK,
                };
                return Ok(_response);
            }
            return BadRequest();
        }

        [HttpPost]
        public ActionResult<Response> Post(Proyectos proyectos)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(proyectos.Nombre) ||
                    string.IsNullOrWhiteSpace(proyectos.Descripcion) ||
                    DateTime.Now >= proyectos.FechaInicio ||
                    proyectos.FechaInicio >= proyectos.FechFin)
                {
                    return BadRequest();
                }

                proyectos.Estado = "Creada";

                _context.Add(proyectos);
                _ = _context.SaveChangesAsync().Result;

                _response = new Response()
                {
                    Object = proyectos,
                    mensaje = "Creación existosa",
                    StatusCode = (int)HttpStatusCode.OK,
                };
                return Ok(_response);
            }
            return BadRequest();
        }

        [HttpPut("[controller]/{id}")]
        public ActionResult Put(int id, CambioProyecto proyectos) 
        {
            if (ModelState.IsValid)
            {
                Proyecto proyect = ProyectoDetail(id);
                if (proyect.Tareas.Select(t => t.Estado == "Pendiente").ToList().Count > 0 ||
                    proyect.Tareas.Select(t => t.FechaFin < proyectos.FechFin).ToList().Count > 0)
                {
                    _response = new Response()
                    {
                        mensaje = "No se puede actualizar el registro",
                        StatusCode = (int)HttpStatusCode.OK,
                    };

                    return BadRequest(_response);
                }

                proyect.FechaFin = proyectos.FechFin;
                proyect.Nombre = proyectos.Nombre;
                proyect.Descripcon = proyectos.Descripcion;

                _context.Update(proyect);
                _ = _context.SaveChangesAsync().Result;

                _response = new Response()
                {
                    Object = proyect,
                    mensaje = "Actualización existosa",
                    StatusCode = (int)HttpStatusCode.OK,
                };
                return Ok(_response);
            }
            return BadRequest();            
        }

        [HttpPut("[controller]/CambioEstado/{id}")]
        [Description("Actualiza el estado ")]
        public ActionResult PutEstado(int id, string estado)
        {
            if (ModelState.IsValid)
            {
                var obj = ProyectoDetail(id);

                if (obj.Estado != "EnProgreso" &&
                    obj.Tareas.Select(t => t.Estado == "Pendiente").ToList().Count > 0 )
                {
                    _response = new Response()
                    {
                        mensaje = "No se puede actualizar el registro",
                        StatusCode = (int)HttpStatusCode.OK,
                    };

                    return BadRequest(_response);
                }
                
                obj.Estado = estado;
                _ = _context.SaveChangesAsync().Result;

                _response = new Response()
                {
                    Object = obj,
                    mensaje = "Actualización existosa",
                    StatusCode = (int)HttpStatusCode.OK,
                };
                return Ok(_response);
            }
            return BadRequest();
            
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var proyDetalle = _context.Proyectos
                .Include(t => t.Tareas)
                .Where(i => i.ProyectoId == id)
                .FirstOrDefault();


                _context.Tareas.RemoveRange(proyDetalle.Tareas);
                _context.Proyectos.Remove(proyDetalle);
                _ = _context.SaveChangesAsync().Result;

                _response = new Response()
                {
                    mensaje = "Eliminación existosa",
                    StatusCode = (int)HttpStatusCode.OK,
                };

                return Ok(_response);
            }
            return BadRequest();
        }

        private bool ProyectoExists(int id)
        {
            return _context.Proyectos.Any(e => e.ProyectoId == id);
        }

        private Proyecto ProyectoDetail(int id)
        {
            return _context.Proyectos
                .Include( t => t.Tareas)
                .Where(u => u.ProyectoId == id).FirstOrDefault();
        }

        private static void EnviarCorreo(string deCorreo, string [] paraCorreo, string nombreProyecto, int idProyecto )
        {
            try
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("Sistema",
                deCorreo);
                message.From.Add(from);

                string correo = string.Empty;
                foreach (var item in paraCorreo)
                {
                    correo = correo + item + ";";
                }

                MailboxAddress to = new MailboxAddress("Administradores", correo);
                message.To.Add(to);

                message.Subject = "Proyecto completado";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<h1>Hola, se culminó un proyecto</h1><br>" +
                    "<h2>proyecto id: " + idProyecto + "</h2><br><h2>nombre: " + nombreProyecto + " !</h2>";
                bodyBuilder.TextBody = "Hola, se culminó un proyecto" +
                    " proyecto id: " + idProyecto + "nombre: " + nombreProyecto + ".";

                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("prueba@gmail", "L1nD4@PRU#b@0912");

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
            }
            catch (Exception)
            {
            }
        }
    }
}
