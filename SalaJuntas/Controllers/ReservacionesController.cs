using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SalaJuntas.Data;
using SalaJuntas.Models;

namespace SalaJuntas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservacionesController : ControllerBase
    {
        private readonly DataContext _context;

        public ReservacionesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Reservaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservaciones>>> GetReservaciones()
        {
          if (_context.Reservaciones == null)
          {
              return NotFound();
          }
            return await _context.Reservaciones.ToListAsync();
        }

        // GET: api/Reservaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservaciones>> GetReservaciones(int id)
        {
          if (_context.Reservaciones == null)
          {
              return NotFound();
          }
            var reservaciones = await _context.Reservaciones.FindAsync(id);

            if (reservaciones == null)
            {
                return NotFound();
            }

            return reservaciones;
        }

        // PUT: api/Reservaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservaciones(int id, Reservaciones reservaciones)
        {
            if (id != reservaciones.Id)
            {
                return BadRequest();
            }

            _context.Entry(reservaciones).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservacionesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Reservaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reservaciones>> PostReservaciones(Reservaciones reservaciones)
        {
            var duracion = reservaciones.fecha_hora_final - reservaciones.fecha_hora_inicial;
            if (duracion.Minutes > 120)
            {
                return Problem("No se puede reservar la sala por mas de 2 horas.");
            }
            
          if (_context.Reservaciones == null)
          {
              return Problem("Entity set 'DataContext.Reservaciones'  is null.");
          }
            var reservado = false;
            var Bd_fecha = await _context.Reservaciones.Select(r => new
            {
                r.liberado,
                r.fecha_hora_inicial,
                r.fecha_hora_final,
                r.Id_sala
            }).Where(r => r.Id_sala == reservaciones.Id_sala).Where(r => r.liberado == false).ToListAsync();
            foreach(var f in Bd_fecha)
            {
                var revision = comprobar(f.fecha_hora_inicial, f.fecha_hora_final, reservaciones.fecha_hora_inicial);
                if(revision == true)
                {
                    reservado = true;
                }
            }

            foreach (var f in Bd_fecha)
            {
                var revision = comprobar(f.fecha_hora_inicial, f.fecha_hora_final, reservaciones.fecha_hora_final);
                if (revision == true)
                {
                    reservado = true;
                }
            }
            ActionResult mensaje;

            if (reservado == false)
            {
                _context.Reservaciones.Add(reservaciones);
                await _context.SaveChangesAsync();

                mensaje = CreatedAtAction("GetReservaciones", new { id = reservaciones.Id }, reservaciones);
            }
            else
            {
                mensaje = Problem("Error la fecha ya esta ocupada.");
            }
            return mensaje;
        }

        protected bool comprobar(DateTime fecha_inicio, DateTime fecha_fin, DateTime fecha)
        {
            return fecha.CompareTo(fecha_inicio) >= 0 && fecha.CompareTo(fecha_fin) <= 0;
        }

        // DELETE: api/Reservaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservaciones(int id)
        {
            if (_context.Reservaciones == null)
            {
                return NotFound();
            }
            var reservaciones = await _context.Reservaciones.FindAsync(id);
            if (reservaciones == null)
            {
                return NotFound();
            }

            _context.Reservaciones.Remove(reservaciones);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservacionesExists(int id)
        {
            return (_context.Reservaciones?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
