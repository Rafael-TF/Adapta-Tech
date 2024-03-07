using System;
using System.Globalization;
using adaptatechwebapibackend.DTOs.CitasMedicas;
using adaptatechwebapibackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace adaptatechwebapibackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CitasMedicasController : ControllerBase
	{
		private readonly AdaptatechContext _context;

		public CitasMedicasController(AdaptatechContext context)
		{
			_context = context;
		}

        // GET: api/CitasMedicas/{idPerfilUsuario}
        [HttpGet("{idPerfilUsuario}")]
        public async Task<ActionResult<IEnumerable<CitaMedica>>> GetCitasMedicas(int idPerfilUsuario)
        {
            var citasMedicas = await _context.CitaMedicas
                .Where(c => c.IdPerfilUsuario == idPerfilUsuario).Select(x => new DTOCitaMedica
                {
                    IdCita = x.IdCita,
                    Medico = x.Medico,
                    FechaHora = x.FechaHora,
                    CentroMedico = x.CentroMedico,
                    DiaSemana = x.DiaSemana
                })
                .ToListAsync();

            if (citasMedicas == null)
            {
                return NotFound();
            }

            return Ok(citasMedicas);
        }

        //Post agregar una cita médica

        [HttpPost("agregarCitaMedica")]

        public async Task<ActionResult> PostCitaMedica([FromBody] DTOAgregarCitaMedica citaMedica)
        {
            var perfilUsuario = await _context.PerfilUsuarios.FindAsync(citaMedica.IdPerfilUsuario);
            if (perfilUsuario == null)
            {
                return NotFound("El perfil de usuario no fue encontrado.");
            }

            try
            {
                
                var citaNueva = new CitaMedica
                {
                    Medico = citaMedica.Medico,
                    FechaHora = citaMedica.FechaHora,
                    CentroMedico = citaMedica.CentroMedico,
                    IdPerfilUsuario = citaMedica.IdPerfilUsuario
                };

                _context.CitaMedicas.Add(citaNueva);
                await _context.SaveChangesAsync();
                return Created("Cita", new { citaMedica = citaNueva });
            }
            catch (FormatException ex)
            {
                // Manejar la excepción de formato incorrecto de la hora
                return BadRequest("El formato de la hora proporcionada no es válido.");
            }
        }

        // PUT: api/CitasMedicas/{id}
        [HttpPut("modificarCitaMedica/{id}")]
        public async Task<IActionResult> PutCitaMedica([FromRoute] int id, [FromBody] DTOModificarCitaMedica citaMedica)
        {
            // Verificar si el ID de la cita médica es diferente al proporcionado en el DTO
            if (id != citaMedica.IdCita)
            {
                return BadRequest("El ID de la cita médica proporcionado no coincide con el ID de la ruta.");
            }

            // Obtener la cita médica de la base de datos
            var citaExistente = await _context.CitaMedicas.FindAsync(id);
            if (citaExistente == null)
            {
                return NotFound("No se encontró la cita médica.");
            }

            // Actualizar las propiedades de la cita médica existente con los valores del DTO
            citaExistente.Medico = citaMedica.Medico;
            citaExistente.FechaHora = citaMedica.FechaHora;
            citaExistente.CentroMedico = citaMedica.CentroMedico;
            // Ajusta las demás propiedades según la estructura de tu DTO

            try
            {
                // Guardar los cambios en la base de datos
                _context.Update(citaExistente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verificar si ocurrió un error de concurrencia al guardar los cambios
                if (!CitaMedicaExists(id))
                {
                    return NotFound("No se encontró la cita médica.");
                }
                else
                {
                    throw; // Lanzar la excepción si no se puede manejar
                }
            }

            return NoContent(); // Devolver una respuesta exitosa (204 No Content)
        }

        // Método privado para verificar si una cita médica existe por su ID
        private bool CitaMedicaExists(int id)
        {
            return _context.CitaMedicas.Any(e => e.IdCita == id);
        }

        // DELETE: api/CitasMedicas/{id}
        [HttpDelete("eliminarCitaMedica/{id}")]
        public async Task<IActionResult> DeleteCitaMedica(int id)
        {
            // Obtener la cita médica de la base de datos
            var citaExistente = await _context.CitaMedicas.FindAsync(id);
            if (citaExistente == null)
            {
                return NotFound("No se encontró la cita médica.");
            }

            // Eliminar la cita médica de la base de datos
            _context.CitaMedicas.Remove(citaExistente);
            await _context.SaveChangesAsync();

            return NoContent(); // Devolver una respuesta exitosa (204 No Content)
        }


    }
}

