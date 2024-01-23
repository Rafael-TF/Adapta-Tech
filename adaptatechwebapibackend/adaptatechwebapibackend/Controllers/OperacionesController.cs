using adaptatechwebapibackend.Models;
using adaptatechwebapibackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace adaptatechwebapibackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // El filtro de excepción también puede aplicar solo a uno o varios controladores
    // La siguiente línea activaría este control de errores solo a este controlador
    // Nosotros lo hemos configurado a nivel global en el Program, que sería el sitio idóneo
    // para que todos los controladores tuvieran integrado el control de errores
    //    [TypeFilter(typeof(FiltroDeExcepcion))]
    public class OperacionesController : ControllerBase
    {

        private readonly AdaptatechContext _context;

        // private readonly IGestorArchivos _gestorArchivosLocal;
        private readonly OperacionesService _operacionesService;

        public OperacionesController(AdaptatechContext context, OperacionesService operacionesService)
        {
            _context = context;
            //_gestorArchivosLocal = gestorArchivosLocal;
            _operacionesService = operacionesService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operacione>>> GetOperaciones()
        {
            if (_context.Operaciones == null)
            {
                return NotFound();
            }

            await _operacionesService.AddOperacion("Get", "GetOperaciones");


            return Ok(await _context.Operaciones.ToListAsync());
        }

    }
}
