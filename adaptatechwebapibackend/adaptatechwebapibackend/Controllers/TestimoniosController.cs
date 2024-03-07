using System;
using adaptatechwebapibackend.DTOs.CitasMedicas;
using adaptatechwebapibackend.DTOs.Testimonios;
using adaptatechwebapibackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace adaptatechwebapibackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestimoniosController : ControllerBase
	{
        private readonly AdaptatechContext _context;

        public TestimoniosController(AdaptatechContext context)
        {
            _context = context;
        }

        // GET: api/Testimonios
        [HttpGet("MostrarTestimonios")]
        public async Task<ActionResult<IEnumerable<DTOTestimonios>>> GetTestimonios()
        {
            var testimonios = await _context.Testimonios
                .Include(t => t.IdPerfilUsuarioNavigation) // Incluye el perfil de usuario para obtener el avatar y el nombre
                .Select(t => new DTOTestimonios
                {
                    IdTestimonio = t.IdTestimonio,
                    IdPerfilUsuario= t.IdPerfilUsuarioNavigation.IdPerfil,
                    Avatar = t.IdPerfilUsuarioNavigation.Avatar,
                    TituloTestimonio = t.Titulo,
                    NombrePerfilUsuario = t.IdPerfilUsuarioNavigation.Nombre,
                    TextoTestimonio = t.TextoTestimonio
                })
                .ToListAsync();

            return testimonios;
        }

        [HttpPost("agregarTestimonio")]

        public async Task<ActionResult> PostTestimonio([FromBody] DTOTestimonioPost testimonio)
        {
            var perfilUsuario = await _context.PerfilUsuarios.FindAsync(testimonio.IdPerfilUsuario);
            if (perfilUsuario == null)
            {
                return NotFound("El perfil de usuario no fue encontrado.");
            }

            try
            {

                var nuevoTestimonio = new Testimonio
                {
                    IdPerfilUsuario = testimonio.IdPerfilUsuario,
                    Titulo = testimonio.TituloTestimonio,
                    TextoTestimonio = testimonio.TextoTestimonio
                };

                _context.Testimonios.Add(nuevoTestimonio);
                await _context.SaveChangesAsync();
                return Created("Cita", new { testimonio = nuevoTestimonio });
            }
            catch (FormatException ex)
            {
                // Manejar la excepción de formato incorrecto de la hora
                return BadRequest("El formato de la hora proporcionada no es válido.");
            }
        }

        //// PUT: api/Testimonios/{idTestimonio}/{idPerfilUsuario}
        //[HttpPut("modificarTestimonio/{idTestimonio}/{idPerfilUsuario}")]
        //public async Task<IActionResult> PutTestimonio([FromRoute] int idTestimonio, [FromRoute] int idPerfilUsuario, [FromBody] DTOTestimonioPut testimonio)
        //{
        //    // Verificar si el ID del testimonio es diferente al proporcionado en el DTO
        //    if (idTestimonio != testimonio.IdTestimonio)
        //    {
        //        return BadRequest("El ID del testimonio proporcionado no coincide con el ID de la ruta.");
        //    }

        //    // Obtener el testimonio de la base de datos
        //    var testimonioExistente = await _context.Testimonios.FindAsync(idTestimonio);
        //    if (testimonioExistente == null)
        //    {
        //        return NotFound("No se encontró el testimonio.");
        //    }

        //    // Verificar si el idPerfilUsuario en la ruta coincide con el idPerfilUsuario asociado al testimonio
        //    if (idPerfilUsuario != testimonio.IdPerfilUsuario)
        //    {
        //        return Unauthorized("No tienes permiso para modificar este testimonio.");
        //    }

        //    // Actualizar las propiedades de la cita médica existente con los valores del DTO
        //    testimonioExistente.IdTestimonio = testimonio.IdTestimonio;
        //    testimonioExistente.IdPerfilUsuarioNavigation.IdPerfil = testimonio.IdPerfilUsuario;
        //    testimonioExistente.Titulo = testimonio.TituloTestimonio;
        //    testimonioExistente.TextoTestimonio = testimonio.TextoTestimonio;


        //    try
        //    {
        //        // Guardar los cambios en la base de datos
        //        _context.Update(testimonioExistente);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        // Verificar si ocurrió un error de concurrencia al guardar los cambios
        //        if (!TestimonioExists(idTestimonio))
        //        {
        //            return NotFound("No se encontró el usuario.");
        //        }
        //        else
        //        {
        //            throw; // Lanzar la excepción si no se puede manejar
        //        }
        //    }

        //    return NoContent(); // Devolver una respuesta exitosa (204 No Content)
        //}

        

        // PUT: api/Testimonios/{idTestimonio}/{idPerfilUsuario}
        [HttpPut("modificarTestimonio/{idTestimonio}/{idPerfilUsuario}")]
        public async Task<IActionResult> PutTestimonio([FromRoute] int idTestimonio, [FromRoute] int idPerfilUsuario, [FromBody] DTOTestimonioPut testimonio)
        {
            // Verificar si el ID del testimonio es diferente al proporcionado en el DTO
            if (idTestimonio != testimonio.IdTestimonio)
            {
                return BadRequest("El ID del testimonio proporcionado no coincide con el ID de la ruta.");
            }

            // Obtener el testimonio de la base de datos
            var testimonioExistente = await _context.Testimonios
                .Include(t => t.IdPerfilUsuarioNavigation) // Cargar la propiedad de navegación IdPerfilUsuarioNavigation
                .FirstOrDefaultAsync(t => t.IdTestimonio == idTestimonio);

            if (testimonioExistente == null)
            {
                return NotFound("No se encontró el testimonio.");
            }

            // Verificar si el idPerfilUsuario en la ruta coincide con el idPerfilUsuario asociado al testimonio
            if (testimonioExistente.IdPerfilUsuarioNavigation.IdPerfil != idPerfilUsuario)
            {
                return Unauthorized("No tienes permiso para modificar este testimonio.");
            }

            // Actualizar las propiedades del testimonio existente con los valores del DTO
            testimonioExistente.Titulo = testimonio.TituloTestimonio;
            testimonioExistente.TextoTestimonio = testimonio.TextoTestimonio;

            try
            {
                // Guardar los cambios en la base de datos
                _context.Update(testimonioExistente);
                await _context.SaveChangesAsync();
            }
            catch (FormatException ex)
            {
                // Manejar la excepción de formato incorrecto de la hora
                return BadRequest("No se ha podido realizar la modificación del testimonio");
            }

            return NoContent(); // Devolver una respuesta exitosa (204 No Content)
        }


        // DELETE: api/Testimonios/{id}
        [HttpDelete("eliminarTestimonio/{idTestimonio}/{idPerfilUsuario}")]
        public async Task<IActionResult> DeleteCitaMedica([FromRoute] int idTestimonio, int idPerfilUsuario)
        {
            // Obtener el testimonio de la base de datos
            var testimonioExistente = await _context.Testimonios
                .Include(t => t.IdPerfilUsuarioNavigation) // Cargar la propiedad de navegación IdPerfilUsuarioNavigation
                .FirstOrDefaultAsync(t => t.IdTestimonio == idTestimonio);
            if (testimonioExistente == null)
            {
                return NotFound("No se encontró el testimonio.");
            }

            // Verificar si el idPerfilUsuario en la ruta coincide con el idPerfilUsuario asociado al testimonio
            if (testimonioExistente.IdPerfilUsuarioNavigation.IdPerfil != idPerfilUsuario)
            {
                return Unauthorized("No tienes permiso para eliminar este testimonio.");
            }

            // Eliminar la cita médica de la base de datos
            _context.Testimonios.Remove(testimonioExistente);
            await _context.SaveChangesAsync();

            return NoContent(); // Devolver una respuesta exitosa (204 No Content)
        }
    }
}

