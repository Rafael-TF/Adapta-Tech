using adaptatechwebapibackend.DTOs;
using adaptatechwebapibackend.Models;
using adaptatechwebapibackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace adaptatechwebapibackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AdaptatechContext _context;
        private readonly HashService _hashService;
        private readonly TokenService _tokenService;

        public AuthController(AdaptatechContext context, HashService hashService, TokenService tokenService)
        {
            _context = context;
            _hashService = hashService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> PostNuevoUsuarioHash([FromBody] DTOUsuario usuario)
        {
            // Verificar si el usuario ya existe
            if (_context.Usuarios.Any(u => u.Email == usuario.Email))
            {
                // Retorna un error indicando que el usuario ya existe
                return BadRequest("El usuario ya existe.");
            }

            var resultadoHash = _hashService.Hash(usuario.Password);
            var newUsuario = new Usuario
            {
                Email = usuario.Email,
                Password = resultadoHash.Hash,
                Salt = resultadoHash.Salt
            };

            await _context.Usuarios.AddAsync(newUsuario);
            await _context.SaveChangesAsync();

            return Ok(newUsuario);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] DTOUsuario usuario)
        {
            // Busca en la base de datos un usuario que coincida con el correo electrónico proporcionado.
            var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuario.Email);
            if (usuarioDB == null)
            {
                return Unauthorized(); // Si no se encuentra el usuario, devuelve un error Unauthorized.
            }

            // Calcula el hash del password proporcionado utilizando el salt del usuario almacenado en la base de datos.
            var resultadoHash = _hashService.Hash(usuario.Password, usuarioDB.Salt);
            if (usuarioDB.Password == resultadoHash.Hash)
            {
                // Si las contraseñas coinciden, genera un token JWT llamando al método GenerarToken y lo devuelve junto con el email.
                var response = _tokenService.GenerarToken(usuario);
                return Ok(response);
            }
            else
            {
                return Unauthorized(); // Si las contraseñas no coinciden, devuelve un error Unauthorized.
            }

        }
    }
}
