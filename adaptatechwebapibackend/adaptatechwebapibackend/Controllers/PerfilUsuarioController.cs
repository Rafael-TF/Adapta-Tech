using adaptatechwebapibackend.DTOs;
using adaptatechwebapibackend.DTOs.PerfilUsuario;
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

    public class PerfilUsuarioController : ControllerBase
        {

        /**
         * 
         * Clase controladora sobre el modelo PERFIL_USUARIO.
         * 
         * Siempre que se refiera en este documento a PERFIL_USUARIO, 
         * me estaré refiriendo a la tabla en la DATABASE.
         * 
         * */


        //Variable objeto _context
        //Instanciamos el contexto de Entity Framework.

        private readonly AdaptatechContext _context;
        private readonly OperacionesService _operacionesService;
        private readonly GestorArchivosLocal _gestorArchivosLocal;


        /**
         * 
         * Constructor con un parámetro de inyección de servicio SCOPED
         * 
         * */

        public PerfilUsuarioController(AdaptatechContext context, OperacionesService operacionesService, GestorArchivosLocal gestorArchivosLocal)
            {
            _context = context;
            _operacionesService = operacionesService;
            _gestorArchivosLocal = gestorArchivosLocal;
            }

        /**
         * 
         * Método GetPerfilesUsuarios()
         * 
         * Devuelve todos los perfiles de usuario en la base de datos.
         * 
         * */

        [HttpGet("perfiles")]
        public async Task<List<PerfilUsuario>> GetPerfilesUsuarios()
            {

            var lista = await _context.PerfilUsuarios.ToListAsync();

            await _operacionesService.AddOperacion("Get", "Perfiles");
            return lista;

            }

        /**
         * 
         * Método GetPerfilesUsuariosSincrono()
         * 
         * Devuelve todos los perfiles de usuario de forma síncrona.
         * 
         * */

        [HttpGet("sincrono")]
        public List<PerfilUsuario> GetPerfilesUsuariosSincrono()
            {
            // Las operaciones contra una base de datos DEBEN SER SIEMPRE ASÍNCRONAS. Para liberar los hilos de ejecución en cada petición, eso no debe hacerse nunca
            var lista = _context.PerfilUsuarios.ToList();

            _operacionesService.AddOperacion("Get", "PerfilesSíncronos");
            return lista;

            }

        /**
         * 
         * Método GetPerfilesUsuariosOrdenadosAsc()
         * 
         * Devuelve todos los perfiles de usuario ordenados ascendentemente por fecha de nacimiento.
         * 
         * */

        [HttpGet("ordenadosfechaascendente")]
        public async Task<List<PerfilUsuario>> GetPerfilesUsuariosOrdenadosAsc()
            {
            var listaOrdenadaAscendente = await _context.PerfilUsuarios.OrderBy(x => x.FechaNacimiento).ToListAsync();

            await _operacionesService.AddOperacion("Get", "PerfilesFechasAscendentes");
            return listaOrdenadaAscendente;
            }

        /**
         * 
         * Método GetPerfilesUsuariosOrdenadosDesc()
         * 
         * Devuelve todos los perfiles de usuario ordenados descendentemente por fecha de nacimiento.
         * 
         * */

        [HttpGet("ordenadosfechadescendente")]
        public async Task<List<PerfilUsuario>> GetPerfilesUsuariosOrdenadosDesc()
            {
            var listaOrdenadaDescendente = await _context.PerfilUsuarios.OrderByDescending(x => x.FechaNacimiento).ToListAsync();

            await _operacionesService.AddOperacion("Get", "PerfilesFechasDescendentes");
            return listaOrdenadaDescendente;
            }

        /**
         * 
         * Método GetPerfilUsuarioPorId([FromRoute] int id)
         * 
         * Devuelve un perfil de usuario correspondiente a una id de PERFIL_USUARIO.
         * 
         * */

        [HttpGet("perfilPorId/{id:int}")]
        public async Task<ActionResult<PerfilUsuario>> GetPerfilUsuarioPorId([FromRoute] int id)
            {
            var perfil = await _context.PerfilUsuarios.FindAsync(id);

            if (perfil == null)
                {
                return NotFound("El perfil con " + id + " no existe.");
                }

            await _operacionesService.AddOperacion("Get", "PerfilesPorId");
            return Ok(perfil);
            }
        /**
         * 
         * Método GetPerfilUsuarioPorEmail([FromRoute] string email)
         * 
         * Devuelve un perfil de usuario correspondiente a un email de USUARIO.
         * 
         * */

        [HttpGet("poremail/{email}")]
        public async Task<ActionResult<PerfilUsuario>> GetPerfilUsuarioPorEmail([FromRoute] string email)
            {
            var perfil = await _context.PerfilUsuarios.Where(x => x.Usuario!.Email.Equals(email)).FirstOrDefaultAsync();

            if (perfil == null)
                {
                return NotFound("El perfil con email: " + email + " no existe.");
                }
            await _operacionesService.AddOperacion("Get", "PerfilPorEmail");
            return Ok(perfil);
            }

        /**
         * 
         * Método GetPerfilUsuarioPorNombre([FromRoute] string nombre)
         * 
         * Devuelve un perfil de usuario correspondiente a un nombre de PERFIL_USUARIO.
         * 
         * */

        [HttpGet("pornombre/{nombre}")]
        public async Task<ActionResult<PerfilUsuario>> GetPerfilUsuarioPorNombre([FromRoute] string nombre)
            {
            var perfil = await _context.PerfilUsuarios.Where(x => x.Nombre.ToLower().Equals(nombre.ToLower())).ToListAsync();

            if (perfil == null)
                {
                return NotFound("El perfil con nombre: " + nombre + " no existe.");
                }
            await _operacionesService.AddOperacion("Get", "PerfilPorNombre");
            return Ok(perfil);
            }

        /**
         * 
         * Método GetPerfilUsuarioPorApellidos([FromRoute] string nombre)
         * 
         * Devuelve un perfil de usuario correspondiente a un nombre de PERFIL_USUARIO.
         * 
         * */

        [HttpGet("porapellidos/{apellidos}")]
        public async Task<ActionResult<PerfilUsuario>> GetPerfilUsuarioPorApellidos([FromRoute] string apellidos)
            {
            var perfil = await _context.PerfilUsuarios.Where(x => x.Apellidos.ToLower().Equals(apellidos.ToLower())).ToListAsync();

            if (perfil == null)
                {
                return NotFound("El perfil con apellidos: " + apellidos + " no existe.");
                }
            await _operacionesService.AddOperacion("Get", "PerfilPorApellidos");
            return Ok(perfil);
            }

        /**
         * 
         * Método PostPerfilUsuario([FromRoute] int id, DTOPerfilUsuarioPost perfil)
         * 
         * Añade un PERFIL_USUARIO.
         *
         * 
         * */

        [HttpPost("nuevoPerfil")]
        public async Task<ActionResult> PostPerfilUsuario([FromBody] DTOPerfilAvatar perfil)
            {
            var newPerfil = new PerfilUsuario()
                {

                Nombre = perfil.NombreDTO,
                Apellidos = perfil.ApellidosDTO,
                Telefono = perfil.TelefonoDTO,
                UsuarioId = perfil.IdUsuarioDTO,
                FechaNacimiento = perfil.FechaNacimientoDTO,
                Avatar = "",
                Alias = perfil.AliasDTO,

                };

            if (perfil.AvatarDTO is not null)
                {

                using (var memoryStream = new MemoryStream())
                    {
                    // Extraemos la imagen de la petición
                    await perfil.AvatarDTO.CopyToAsync(memoryStream);
                    // La convertimos a un array de bytes que es lo que necesita el método de guardar
                    var contenido = memoryStream.ToArray();
                    DTOArchivo archivo = new DTOArchivo
                        {
                        Nombre = perfil.AvatarDTO.FileName,
                        Contenido = contenido,
                        Carpeta = "imagenes",
                        ContentType = perfil.AvatarDTO.ContentType
                        };


                    var nombreArchivo = await _gestorArchivosLocal.GuardarArchivo(archivo.Contenido, archivo.Nombre, archivo.Carpeta, archivo.ContentType);

                    newPerfil.Avatar = nombreArchivo;

                    }


                }



            await _context.AddAsync(newPerfil);
            await _context.SaveChangesAsync();

            await _operacionesService.AddOperacion("Post", "NuevoPerfil");
            return Created("PerfilUsuario", new
                {
                perfil = newPerfil
                });
            }

        /**
         * 
         * Método PutPerfilUsuario([FromRoute] int id, DTOPerfilUsuarioPut perfil)
         * 
         * Actualiza la información o módifica la información de un registro de PERFIL_USUARIO en la base de datos.
         * 
         * */

        [HttpPut("cambiarDatosPerfil/{id}")]
        public async Task<ActionResult> PutPerfilUsuario([FromRoute] int id, DTOPerfilUsuarioPut perfil)
            {
            if (id != perfil.IdPerfil)
                {
                return BadRequest("Los ids proporcionados son diferentes");
                }
            var perfilUpdate = await _context.PerfilUsuarios.AsTracking().FirstOrDefaultAsync(x => x.IdPerfil == id);
            if (perfilUpdate == null)
                {
                return NotFound();
                }
            perfilUpdate.UsuarioId = perfil.IdUsuario;
            perfilUpdate.Nombre = perfil.Nombre;
            perfilUpdate.Apellidos = perfil.Apellidos;
            perfilUpdate.FechaNacimiento = perfil.FechaNacimiento;
            perfilUpdate.Avatar = perfil.Avatar;
            perfilUpdate.Alias = perfil.Alias;


            _context.Update(perfilUpdate);

            await _context.SaveChangesAsync();
            await _operacionesService.AddOperacion("Put", "PerfilModificado");
            return NoContent();
            }


        /**
         * 
         * Método DeletePerfilUsuarioId(int id)
         * 
         * Elimina un perfil de usuario correspondiente a una id de PERFIL_USUARIO.
         * 
         * */

        [HttpDelete("borrarPerfil/{id}")]
        public async Task<ActionResult> DeletePerfilUsuarioId(int id)
            {

            var perfil = await _context.PerfilUsuarios.FindAsync(id);
            if (perfil is null)
                {
                return NotFound("El perfil de usuario no existe.");
                }

            _context.Remove(perfil);
            await _context.SaveChangesAsync();

            await _operacionesService.AddOperacion("Delete", "PerfilBorrado");
            return Ok();

            }


        }
    }

