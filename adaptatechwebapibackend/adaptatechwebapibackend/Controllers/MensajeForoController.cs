using adaptatechwebapibackend.DTOs.MensajesForo;
using adaptatechwebapibackend.Models;
using adaptatechwebapibackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace adaptatechwebapibackend.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MensajeForoController : Controller
        {

        /**
        * 
        * Clase controladora sobre el modelo MENSAJES.
        * 
        * Siempre que se refiera en este documento a MENSAJES, 
        * me estaré refiriendo a la tabla en la DATABASE.
        * 
        * */


        private readonly AdaptatechContext _context;
        private readonly OperacionesService _operacionesService;

        public MensajeForoController(AdaptatechContext context, OperacionesService operacionesService)
            {
            _context = context;
            _operacionesService = operacionesService;
            }

        /**
         * 
         * Método GetMensajeForos()
         * 
         * Devuelve todos los mensajes en la base de datos.
         * 
         * */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MensajeForo>>> GetMensajeForos()
            {
            // Obtiene todos los mensajes del foro
            await _operacionesService.AddOperacion("Get", "Mensajes");
            return await _context.MensajeForos.ToListAsync();
            }

        /**
         * 
         * Método GetMensajeForo(int id)
         * 
         * Devuelve un mensaje por ID en la base de datos.
         * 
         * */

        [HttpGet("{id}")]
        public async Task<ActionResult<MensajeForo>> GetMensajeForo(int id)
            {
            // Obtiene un mensaje específico por su ID
            var mensajeForo = await _context.MensajeForos.FindAsync(id);

            if (mensajeForo == null)
                {
                return NotFound();
                }

            await _operacionesService.AddOperacion("Get", "MensajesPorId");
            return mensajeForo;
            }

        /**
        * 
        * Método GetMensajeForo(int id)
        * 
        * Devuelve un mensaje por ID en la base de datos.
        * 
        * */

        [HttpGet("mensajesPorTema/{idTemaForo:int}")]
        public async Task<ActionResult<List<DTOMensajeForo>>> GetMensajeForoPorTema(int idTemaForo)
            {
            // Obtiene un mensaje específico por su ID
            var mensajesTemaForo = await _context.MensajeForos.Where(x => x.IdTema == idTemaForo).Select(x =>
                new DTOMensajeForo
                    {

                    Alias = x.IdPerfilUsuariomensajeNavigation.Alias,
                    IdMensaje = x.IdMensaje,
                    FechaMensaje = x.FechaMensaje,
                    IdPerfilUsuariomensaje = x.IdPerfilUsuariomensaje,
                    IdTema = x.IdTema,
                    IdUsuariomensaje = x.IdUsuariomensaje,
                    Texto = x.Texto,



                    }).ToListAsync();

            if (mensajesTemaForo == null)
                {
                return NotFound();
                }

            await _operacionesService.AddOperacion("Get", "GetMensajesForoPortema");
            return Ok(mensajesTemaForo);
            }


        // Obtener mensajes por alias
        //[HttpGet("{alias}")]
        //public async Task<ActionResult<MensajeForo>> GetMensajeForoAias(string alias)
        //{
        //    // Obtiene un mensaje específico por su alias
        //    var mensajeForo = await _context.MensajeForos.FindAsync(alias);

        //    if (mensajeForo == null)
        //    {
        //        return NotFound();
        //    }

        //    return mensajeForo;
        //}

        /**
         * 
         * Método GetMensajeForoAias(string alias)
         * 
         * Devuelve un mensaje por ALIAS en la base de datos.
         * 
         * */
        [HttpGet("alias/{alias}")]
        public async Task<ActionResult<MensajeForo>> GetMensajeForoAias(string alias)
            {
            // Obtiene un mensaje específico por su alias
            var mensajeForo = await _context.MensajeForos.Include(x => x.IdPerfilUsuariomensajeNavigation)
                .Where(y => y.IdPerfilUsuariomensajeNavigation.Alias.Equals(alias)).FirstOrDefaultAsync();


            if (mensajeForo == null)
                {
                return NotFound();
                }

            await _operacionesService.AddOperacion("Get", "MensajesPorAlias");
            return mensajeForo;
            }


        /**
         * 
         * Método PostMensajeForo(MensajeForo mensajeForo)
         * 
         * Inserta un mensaje en la base de datos.
         * 
         * */
        [HttpPost("agregarmensaje")]
        public async Task<ActionResult<MensajeForo>> PostMensajeForo(MensajeForo mensajeForo)
            {
            // Crea un nuevo mensaje en el foro
            _context.MensajeForos.Add(mensajeForo);
            await _context.SaveChangesAsync();

            await _operacionesService.AddOperacion("Post", "NuevoMensaje");
            return CreatedAtAction("GetMensajeForo", new { id = mensajeForo.IdMensaje }, mensajeForo);
            }

        /**
         * 
         * Método AgregarMensaje([FromBody] DTOMensajeForo mensaje)
         * 
         * Inserta un mensaje en la base de datos validando ciertas cuestiones(Usuario, Perfil, Tema).
         * 
         * */

        [HttpPost]
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

                //Verificar si el tema al que pertenece el mensaje existe en la base de datos
                var temaExistente = await _context.TemasForos.FirstOrDefaultAsync(t => t.IdTema == mensaje.IdTema);
                if (temaExistente == null)
                    {
                    return BadRequest("El tema especificado no existe.");
                    }

                // Si todos los elementos existen, se crea el nuevo mensaje
                var nuevoMensaje = new MensajeForo
                    {
                    IdUsuariomensaje = mensaje.IdUsuariomensaje,
                    IdPerfilUsuariomensaje = mensaje.IdPerfilUsuariomensaje,
                    IdTema = mensaje.IdTema,
                    Texto = mensaje.Texto,
                    FechaMensaje = DateTime.Now
                    };

                // Agregar el mensaje a la base de datos
                _context.MensajeForos.Add(nuevoMensaje);
                await _context.SaveChangesAsync();

                await _operacionesService.AddOperacion("Post", "AgregarMensaje");
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

            await _operacionesService.AddOperacion("Put", "ModificarMensaje");
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

            await _operacionesService.AddOperacion("Delete", "DeleteUsuarioPorId");
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

            await _operacionesService.AddOperacion("Get", "MensajesPorPerfilDeUsuario");
            return mensajesByPerfil;
            }

        private bool MensajeForoExists(int id)
            {
            // Verifica si un mensaje del foro con el ID proporcionado existe en la base de datos
            return _context.MensajeForos.Any(e => e.IdMensaje == id);
            }

        }
    }
