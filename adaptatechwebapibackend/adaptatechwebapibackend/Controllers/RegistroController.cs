using adaptatechwebapibackend.DTOs;
using adaptatechwebapibackend.DTOs.Usuarios;
using adaptatechwebapibackend.Models;
using adaptatechwebapibackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace adaptatechwebapibackend.Controllers
    {
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroController : ControllerBase
        {
        private readonly AdaptatechContext _context;
        private readonly HashService _hashService;
        private readonly OperacionesService _operacionesService;
        private readonly GestorArchivosLocal _gestorArchivosLocal;



        public RegistroController(AdaptatechContext context, HashService hashService, GestorArchivosLocal gestorArchivosLocal, OperacionesService operacionesService)
            {
            _context = context;
            _hashService = hashService;
            _gestorArchivosLocal = gestorArchivosLocal;
            _operacionesService = operacionesService;
            }

        [HttpPost("register")]
        public async Task<ActionResult> PostNuevoUsuarioHash([FromForm] RegistroDTO registro)
            {
            try
                {
                // Verificar si el usuario ya existe
                if (_context.Usuarios.Any(u => u.Email == registro.Email))
                    {
                    return BadRequest("El usuario ya existe.");
                    }

                var resultadoHash = _hashService.Hash(registro.Password);

                // Crear un nuevo usuario
                var nuevoUsuario = new Usuario
                    {
                    Email = registro.Email,
                    Password = resultadoHash.Hash,
                    Salt = resultadoHash.Salt
                    };

                await _context.Usuarios.AddAsync(nuevoUsuario);
                await _context.SaveChangesAsync();
                var nuevoPerfil = new PerfilUsuario // Crear un nuevo perfil de usuario
                    {
                    Nombre = registro.Nombre,
                    Apellidos = registro.Apellidos,
                    Telefono = registro.Telefono,
                    FechaNacimiento = registro.FechaNacimiento,
                    //Avatar = "",
                    Alias = registro.Alias,
                    UsuarioId = nuevoUsuario.IdUsuario
                    };

                if (registro.AvatarDTO is not null)
                    {

                    using (var memoryStream = new MemoryStream())
                        {
                        // Extraemos la imagen de la petición
                        await registro.AvatarDTO.CopyToAsync(memoryStream);
                        // La convertimos a un array de bytes que es lo que necesita el método de guardar
                        var contenido = memoryStream.ToArray();
                        DTOArchivo archivo = new DTOArchivo
                            {
                            Nombre = registro.AvatarDTO.FileName,
                            Contenido = contenido,
                            Carpeta = "imagenes",
                            ContentType = registro.AvatarDTO.ContentType
                            };


                        var nombreArchivo = await _gestorArchivosLocal.GuardarArchivo(archivo.Contenido, archivo.Nombre, archivo.Carpeta, archivo.ContentType);

                        nuevoPerfil.Avatar = nombreArchivo;
                        }


                    }


                // Añadir el nuevo usuario a la base de datos
                await _context.PerfilUsuarios.AddAsync(nuevoPerfil);
                await _context.SaveChangesAsync();

                await _operacionesService.AddOperacion("Post", "Register");
                return Ok(nuevoUsuario);
                }
            catch (Exception ex)
                {
                return StatusCode(500, $"Error al registrar: {ex.Message}");
                }
            }


        }
    }
