using Base_de_datos.Entidades;
using Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ServicioProyecto.Properties
{
    [Authorize]
    [ApiController]
    [Route("Tareas")]
    public class TareasController : Controller
    {
        #region Class Attributes
        private Response _response = null;
        #endregion

        #region Class Constructor
        private readonly AppDbContext _context;

        public TareasController(AppDbContext context)
        {
            _context = context;
        }
        #endregion



        [HttpGet]
        public ActionResult<Response> Get()
        {
            var obj = _context.Tareas.ToList();

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
            var obj = TareaDetail(id);

            _response = new Response()
            {
                Object = obj,
                StatusCode = (int)HttpStatusCode.OK,
            };
            return Ok(_response);
        }

        [HttpPost]
        public ActionResult<Response> Post(CreaTarea tarea)
        {
            if (ModelState.IsValid)
            {
                Proyecto obj = _context.Proyectos.Where(p => p.ProyectoId == tarea.ProyectoId).FirstOrDefault();

                if (obj == null && (tarea.FechaInicio < tarea.FechaFin) && 
                    (tarea.FechaInicio >= obj.FechaInicio && tarea.FechaInicio < obj.FechaFin) &&
                    (tarea.FechaFin < obj.FechaFin )
                    )
                {
                    _response = new Response()
                    {
                        mensaje = "No se pudo crear la tarea",
                        StatusCode = (int)HttpStatusCode.BadRequest,
                    };
                    return BadRequest(Response);
                }

                Tarea UltimoObj = _context.Tareas.Where(p => p.ProyectoId == tarea.ProyectoId).Last();

                int id = 0;
                if (UltimoObj == null)
                    id = UltimoObj.TareaId + 1;

                Tarea nueva = new()
                {
                    TareaId = UltimoObj.TareaId,
                    Nombre = tarea.Nombre,
                    Descripcon = tarea.Descripcion,
                    Estado = "Creada",
                    ProyectoId = obj.ProyectoId,
                    FechaInicio = tarea.FechaInicio
                };

                _context.Add(nueva);
                _ = _context.SaveChangesAsync().Result;

                _response = new Response()
                {
                    Object = tarea,
                    mensaje = "Creación existosa",
                    StatusCode = (int)HttpStatusCode.OK,
                };
                return Ok(_response);
            }
            return BadRequest();
        }

        [HttpPut("[controller]/{id}")]
        public ActionResult Put(int id, CreaTarea tarea)
        {
            if (ModelState.IsValid)
            {
                Tarea t = TareaDetail(id);
                Proyecto obj = t.Proyecto;

                _response = new Response()
                {
                    Object = tarea,
                    mensaje = "No se puede actualizar la tarea"
                };

                if (obj.Estado == "Finalizado")
                {
                    _response.StatusCode = (int)HttpStatusCode.Conflict;
                    BadRequest(_response);
                }

                if (string.IsNullOrEmpty(tarea.Nombre) &&
                    (tarea.FechaInicio < tarea.FechaFin) &&
                    (tarea.FechaInicio >= obj.FechaInicio && tarea.FechaInicio < obj.FechaFin) &&
                    (tarea.FechaFin < obj.FechaFin) )
                {
                    _response.StatusCode = (int)HttpStatusCode.BadRequest;
                    BadRequest(_response);
                }

                t.Nombre = tarea.Nombre;
                t.Descripcon = tarea.Descripcion;
                t.FechaInicio = tarea.FechaInicio;
                t.FechaFin = tarea.FechaFin;
                _context.Update(t);
                _= _context.SaveChangesAsync().Result;
                _response = new Response()
                {
                    Object = t,
                    mensaje = "Actualización existosa",
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }

            return BadRequest();
        }

        [HttpPut("[controller]/CambioEstado/{id}")]
        public ActionResult PutEstado(int id, string estado)
        {
            if (ModelState.IsValid)
            {
                Tarea task = TareaDetail(id);
                task.Estado = estado;
                _ = _context.SaveChangesAsync().Result;

                _response = new Response()
                {
                    mensaje = "Actualización existosa",
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }

            return BadRequest();
        }

        [HttpDelete]
        public ActionResult DeleteConfirmed(int id)
        {
            Tarea Tareas = TareaDetail(id);
            _context.Tareas.Remove(Tareas);
            _ = _context.SaveChangesAsync().Result;

            _response = new Response()
            {
                mensaje = "Eliminación existosa",
                StatusCode = (int)HttpStatusCode.OK,
            };

            return Ok(_response);
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.TareaId == id);
        }

        private Tarea TareaDetail(int id)
        {
            return _context.Tareas
                .Include(p => p.Proyecto)
                .Where(u => u.TareaId == id).FirstOrDefault();
        }

    }
}
