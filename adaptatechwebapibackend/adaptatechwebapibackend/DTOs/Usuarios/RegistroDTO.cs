using System;
using adaptatechwebapibackend.DTOs.PerfilUsuario;
using adaptatechwebapibackend.Models;
using adaptatechwebapibackend.Validators;

namespace adaptatechwebapibackend.DTOs.Usuarios
    {
    public class RegistroDTO
        {
        public string Email { get; set; }
        public string Password { get; set; }

        [PesoArchivoValidacion(PesoMaximoEnMegaBytes: 4)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public IFormFile AvatarDTO { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Telefono { get; set; }

        public int? IdUsuario { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string? Avatar { get; set; }

        public string? Alias { get; set; }

        //public DTOPerfilUsuarioPost PerfilUsuario { get; set; }

        }
    }

