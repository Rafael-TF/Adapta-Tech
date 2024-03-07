using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace adaptatechwebapibackend.Services
{
    public class GestorArchivosLocal
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GestorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task BorrarArchivo(string ruta, string carpeta)
        {
            if (ruta != null)
            {
                var nombreArchivo = Path.GetFileName(ruta);
                string directorioArchivo = Path.Combine(env.WebRootPath, carpeta, nombreArchivo);

                if (File.Exists(directorioArchivo))
                {
                    File.Delete(directorioArchivo);
                }
            }

            return Task.CompletedTask;
        }

        public async Task<string> EditarArchivo(byte[] contenido, string extension, string carpeta, string ruta, string contentType)
        {
            await BorrarArchivo(ruta, carpeta);
            return await GuardarArchivo(contenido, extension, carpeta, contentType);
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string carpeta, string contentType)
        {
            // Creamos un nombre aleatorio con la extensión original
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            // La ruta será wwwroot/carpeta (en este caso imagenes)
            string folder = Path.Combine(env.WebRootPath, carpeta);

            // Si no existe la carpeta la creamos
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // La ruta donde dejaremos el archivo será la concatenación de la ruta de la carpeta y el nombre del archivo
            string rutaCompleta = Path.Combine(folder, nombreArchivo);
            // Guardamos el archivo
            await File.WriteAllBytesAsync(rutaCompleta, contenido);

            // La url de la imagen será http o https://dominio/carpeta/nombreimagen
            var urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var urlParaBD = Path.Combine(urlActual, carpeta, nombreArchivo).Replace("\\", "/");
            return urlParaBD;
        }
    }
}
