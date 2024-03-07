using Microsoft.AspNetCore.Mvc.Filters;

namespace adaptatechwebapibackend.Filters
{
    public class FiltroDeExcepcion : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _env;

        public FiltroDeExcepcion(IWebHostEnvironment env)
        {
            _env = env;
        }

        // Cuando haya errores inesperados en un controller vendrá siempre al método OnException
        // para dar respuesta al error, nuestro trabajo pasa por integrar esto en el proyecto,
        // en el program, línea de AddControllers incluir la configuración de este filtro de excepción
        // ExceptionContext encapsula toda la información del error
        // En el constructor debemos inyectar otras dependencias que debemos usar, en este caso IWebHostEnvironment
        // porque vamos a registrar el error en un archivo

        public override void OnException(ExceptionContext context)
        {

            var path = $@"{_env.ContentRootPath}\wwwroot\ErrorLog.txt";
            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                //writer.WriteLine($"------ERROR '{context.Exception.Message}'-------");
                writer.WriteLine($"IP: {context.HttpContext.Connection.RemoteIpAddress!.ToString()} " +
                    $"- Fecha/Hora: {DateTime.Now.ToString("dd MMMM yyyy HH: mm:ss")} " +
                    $"- Ruta: {context.HttpContext.Request.Path.ToString()} " +
                    $"- Método: {context.HttpContext.Request.Method} " +
                    $"- Excepción: {context.Exception.Message}");
                //writer.WriteLine($"------Fin del registro-------");
            }

            base.OnException(context);
        }
    }
}
