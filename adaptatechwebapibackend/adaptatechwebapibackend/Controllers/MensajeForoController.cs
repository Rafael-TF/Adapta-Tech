using adaptatechwebapibackend.DTOs;
using adaptatechwebapibackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace adaptatechwebapibackend.Controllers
{
    public class MensajeForoController : Controller
    {
        private readonly AdaptatechContext _context;

        public MensajeForoController(AdaptatechContext context)
        {
            _context = context;
        }

        // GET: api/MensajeForo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MensajeForo>>> GetMensajeForos()
        {
            // Obtiene todos los mensajes del foro
            return await _context.MensajeForos.ToListAsync();
        }

        // GET: api/MensajeForo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MensajeForo>> GetMensajeForo(int id)
        {
            // Obtiene un mensaje específico por su ID
            var mensajeForo = await _context.MensajeForos.FindAsync(id);

            if (mensajeForo == null)
            {
                return NotFound();
            }

            return mensajeForo;
        }

        // Obtener mensajes por alias
        [HttpGet("{alias}")]
        public async Task<ActionResult<MensajeForo>> GetMensajeForoAias(string alias)
        {
            // Obtiene un mensaje específico por su alias
            var mensajeForo = await _context.MensajeForos.FindAsync(alias);

            if (mensajeForo == null)
            {
                return NotFound();
            }

            return mensajeForo;
        }

        // POST: api/MensajeForo
        [HttpPost]
        public async Task<ActionResult<MensajeForo>> PostMensajeForo(MensajeForo mensajeForo)
        {
            // Crea un nuevo mensaje en el foro
            _context.MensajeForos.Add(mensajeForo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMensajeForo", new { id = mensajeForo.IdMensaje }, mensajeForo);
        }

        [HttpPost("agregarmensaje")]
        public async Task<ActionResult<MensajeForo>> AgregarMensaje([FromBody] DTOMensajeForo mensaje)
        {
            try
            {
                // Verificar si el usuario que envía el mensaje existe en la base de datos
                var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == mensaje.IdUsuariomensaje);
                if (usuarioExistente == null)
                {
                    return BadRequest("El usuario especificado no existe.");
                }

                //Verificar si el perfil de usuario asociado al mensaje existe en la base de datos
                var perfilUsuarioExistente = await _context.PerfilUsuarios.FirstOrDefaultAsync(p => p.IdPerfil == mensaje.IdPerfilUsuariomensaje);
                if (perfilUsuarioExistente == null)
                {
                    return BadRequest("El perfil de usuario especificado no existe.");
                }

                // Verificar si el tema al que pertenece el mensaje existe en la base de datos
                //var temaExistente = await _context.TemasForos.FirstOrDefaultAsync(t => t.IdTema == mensaje.IdTema);
                //if (temaExistente == null)
                //{
                //    return BadRequest("El tema especificado no existe.");
                //}

                // Si todos los elementos existen, se crea el nuevo mensaje
                var nuevoMensaje = new MensajeForo
                {
                    IdUsuariomensaje = mensaje.IdUsuariomensaje,
                    IdPerfilUsuariomensaje = mensaje.IdPerfilUsuariomensaje,
                    //IdTema = mensaje.IdTema,
                    Texto = mensaje.Texto,
                    FechaMensaje = DateTime.Now
                };

                // Agregar el mensaje a la base de datos
                _context.MensajeForos.Add(nuevoMensaje);
                await _context.SaveChangesAsync();

                return Created("MensajeForo", new { mensaje = nuevoMensaje });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al agregar el mensaje: {ex.Message}");
            }
        }


        // PUT: api/MensajeForo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMensajeForo(int id, MensajeForo mensajeForo)
        {
            if (id != mensajeForo.IdMensaje)
            {
                return BadRequest();
            }

            // Actualiza un mensaje existente en el foro
            _context.Entry(mensajeForo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MensajeForoExists(id))
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

        // DELETE: api/MensajeForo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMensajeForo(int id)
        {
            // Elimina un mensaje del foro por su ID
            var mensajeForo = await _context.MensajeForos.FindAsync(id);
            if (mensajeForo == null)
            {
                return NotFound();
            }

            _context.MensajeForos.Remove(mensajeForo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/MensajeForo/ByPerfilUsuario/5
        [HttpGet("ByPerfilUsuario/{perfilUsuarioId}")]
        public async Task<ActionResult<IEnumerable<MensajeForo>>> GetMensajesByPerfilUsuario(int perfilUsuarioId)
        {
            // Obtiene todos los mensajes asociados a un perfil de usuario específico
            var mensajesByPerfil = await _context.MensajeForos
                .Where(m => m.IdPerfilUsuariomensaje == perfilUsuarioId)
                .ToListAsync();

            if (mensajesByPerfil == null || mensajesByPerfil.Count == 0)
            {
                return NotFound($"No se encontraron mensajes para el perfil de usuario con ID {perfilUsuarioId}.");
            }

            return mensajesByPerfil;
        }

        private bool MensajeForoExists(int id)
        {
            // Verifica si un mensaje del foro con el ID proporcionado existe en la base de datos
            return _context.MensajeForos.Any(e => e.IdMensaje == id);
        }

    }
}
