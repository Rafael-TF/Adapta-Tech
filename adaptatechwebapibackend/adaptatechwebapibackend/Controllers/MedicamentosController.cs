using System;
using adaptatechwebapibackend.DTOs.Medicamentos;
using adaptatechwebapibackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace adaptatechwebapibackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicamentosController : ControllerBase
	{
        private readonly AdaptatechContext _context;

        public MedicamentosController(AdaptatechContext context)
		{
            _context = context;
		}


        // GET: api/Medicamentos/{idPerfilUsuario}
        [HttpGet("porPerfilUsuario/{idPerfilUsuario}")]
        public async Task<ActionResult<IEnumerable<Medicamento>>> GetMedicamentos(int idPerfilUsuario)
        {
            // Verificar si el perfil de usuario existe
            var perfilUsuario = await _context.PerfilUsuarios.FindAsync(idPerfilUsuario);
            if (perfilUsuario == null)
            {
                return NotFound("El perfil de usuario no existe.");
            }

            var medicamentos = await _context.Medicamentos
                .Where(m => m.IdPerfilUsuario == idPerfilUsuario).Select(x => new DTOMedicamentos
                {
                    Medicacion = x.Medicacion,
                    Posologia = x.Posologia,
                    Funcion = x.Funcion,
                    DiaSemana = x.DiaSemana

                })
                .ToListAsync();

            // Verificar si no hay medicamentos asociados al perfil de usuario
            if (medicamentos == null || medicamentos.Count == 0)
            {
                return NotFound("No se encontraron medicamentos para este perfil de usuario.");
            }

            return Ok(medicamentos);
        }

        // GET: api/Medicamentos/porDiaSemana/{idPerfilUsuario}
        [HttpGet("porDiaSemana/{idPerfilUsuario}")]
        public async Task<ActionResult<Dictionary<string, List<DTOMedicamentos>>>> GetMedicamentosPorDiaSemana(int idPerfilUsuario)
        {
            // Verificar si el perfil de usuario existe
            var perfilUsuario = await _context.PerfilUsuarios.FindAsync(idPerfilUsuario);
            if (perfilUsuario == null)
            {
                return NotFound("El perfil de usuario no existe.");
            }

            // Obtener los medicamentos del perfil de usuario
            var medicamentos = await _context.Medicamentos
                .Where(m => m.IdPerfilUsuario == idPerfilUsuario)
                .Select(x => new DTOMedicamentos
                {
                    Medicacion = x.Medicacion,
                    Posologia = x.Posologia,
                    Funcion = x.Funcion,
                    DiaSemana = x.DiaSemana
                })
                .ToListAsync();

            // Agrupar los medicamentos por DiaSemana
            var medicamentosPorDiaSemana = medicamentos.GroupBy(m => m.DiaSemana)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Verificar si no hay medicamentos asociados al perfil de usuario
            if (medicamentosPorDiaSemana == null || medicamentosPorDiaSemana.Count == 0)
            {
                return NotFound("No se encontraron medicamentos para este perfil de usuario.");
            }

            return Ok(medicamentosPorDiaSemana);
        }

        // GET: api/Medicamentos/porDiaSemana/{idPerfilUsuario}/{dia}
        [HttpGet("porDiaSemana/{idPerfilUsuario}/{dia}")]
        public async Task<ActionResult<List<DTOMedicamentos>>> GetMedicamentosPorDia(int idPerfilUsuario, string dia)
        {
            // Verificar si el perfil de usuario existe
            var perfilUsuario = await _context.PerfilUsuarios.FindAsync(idPerfilUsuario);
            if (perfilUsuario == null)
            {
                return NotFound("El perfil de usuario no existe.");
            }

            // Obtener los medicamentos del perfil de usuario para el día especificado
            var medicamentos = await _context.Medicamentos
                .Where(m => m.IdPerfilUsuario == idPerfilUsuario && m.DiaSemana == dia)
                .Select(x => new DTOMedicamentos
                {
                    IdMedicamento = x.IdMedicamento,
                    Medicacion = x.Medicacion,
                    Posologia = x.Posologia,
                    Funcion = x.Funcion,
                    DiaSemana = x.DiaSemana
                })
                .ToListAsync();

            // Verificar si no hay medicamentos asociados al perfil de usuario para el día especificado
            if (medicamentos == null || medicamentos.Count == 0)
            {
                return NotFound("No se encontraron medicamentos para este perfil de usuario en el día especificado.");
            }

            return Ok(medicamentos);
        }



        // POST: api/Medicamentos/agregarMedicamento
        [HttpPost("agregarMedicamento")]
        public async Task<ActionResult<Medicamento>> PostMedicamento(DTOPostMedicamento medicamento)
        {
            // Obtener el perfil de usuario correspondiente al idPerfilUsuario
            var perfilUsuario = await _context.PerfilUsuarios.FindAsync(medicamento.IdPerfilUsuario);

            // Verificar si se encontró el perfil de usuario
            if (perfilUsuario == null)
            {
                // Devolver un error si el perfil de usuario no se encontró
                return BadRequest("No se encontró el perfil de usuario especificado.");
            }
            var nuevoMedicamento = new Medicamento
            {
                Medicacion = medicamento.Medicacion,
                Posologia = medicamento.Posologia,
                Funcion = medicamento.Funcion,
                DiaSemana = medicamento.DiaSemana,
                IdPerfilUsuario = medicamento.IdPerfilUsuario
            };

            _context.Medicamentos.Add(nuevoMedicamento);
            await _context.SaveChangesAsync();

            // Devuelve el objeto Medicamento recién creado con el ID generado
            return CreatedAtAction(nameof(GetMedicamentoById), new { id = nuevoMedicamento.IdMedicamento }, nuevoMedicamento);
        }

        // Método que devuelve un medicamento por su ID específico
        [HttpGet("{id}")]
        public async Task<ActionResult<Medicamento>> GetMedicamentoById(int id)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);

            if (medicamento == null)
            {
                return NotFound();
            }

            return medicamento;
        }

        // PUT: api/Medicamentos/{id}
        [HttpPut("modificarMedicamento/{id}")]
        public async Task<IActionResult> PutMedicamento(int id, DTOModificarMedicamento medicamento)
        {
            // Verificar si el ID del medicamento es diferente al proporcionado en el DTO
            if (id != medicamento.IdMedicamento)
            {
                return BadRequest("El ID del medicamento proporcionado no coincide con el ID de la ruta.");
            }

            // Obtener el medicamento de la base de datos
            var medicamentoExistente = await _context.Medicamentos.FindAsync(id);
            if (medicamentoExistente == null)
            {
                return NotFound("No se encontró el medicamento.");
            }

            // Actualizar las propiedades del medicamento existente con los valores del DTO
            medicamentoExistente.Medicacion = medicamento.Medicacion;
            medicamentoExistente.Posologia = medicamento.Posologia;
            medicamentoExistente.Funcion = medicamento.Funcion;
            medicamentoExistente.DiaSemana = medicamento.DiaSemana;

            try
            {
                // Guardar los cambios en la base de datos
                _context.Update(medicamentoExistente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Verificar si ocurrió un error de concurrencia al guardar los cambios
                if (!MedicamentoExists(id))
                {
                    return NotFound("No se encontró el medicamento.");
                }
                else
                {
                    throw; // Lanzar la excepción si no se puede manejar
                }
            }

            return NoContent(); // Devolver una respuesta exitosa (204 No Content)
        }

        // Método privado para verificar si un medicamento existe por su ID
        private bool MedicamentoExists(int id)
        {
            return _context.Medicamentos.Any(e => e.IdMedicamento == id);
        }



        // DELETE: api/Medicamentos/{id}
        [HttpDelete("eliminarMedicamento/{id}")]
        public async Task<IActionResult> DeleteMedicamento(int id)
        {
            // Buscar el medicamento por su ID
            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento == null)
            {
                // Si no se encuentra, devolver un error 404 (Not Found)
                return NotFound("No se encontró el medicamento.");
            }

            // Eliminar el medicamento de la base de datos
            _context.Medicamentos.Remove(medicamento);
            await _context.SaveChangesAsync();

            // Devolver una respuesta exitosa con un mensaje indicando que el medicamento fue eliminado
            return Ok("Medicamento eliminado exitosamente.");
        }


    }
}

