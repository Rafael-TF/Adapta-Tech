using adaptatechwebapibackend.Validators;

namespace adaptatechwebapibackend.DTOs.PerfilUsuario
    {
    public class DTOPerfilAvatar
        {

        public string NombreDTO { get; set; }
        public string ApellidosDTO { get; set; }
        public string TelefonoDTO { get; set; }

        public string AliasDTO { get; set; }

        //OBJETO USUARIO (NO ID DE PERFIL)
        public int IdUsuarioDTO { get; set; }

        public DateTime FechaNacimientoDTO { get; set; }

        [PesoArchivoValidacion(PesoMaximoEnMegaBytes: 4)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public IFormFile AvatarDTO { get; set; }


        }
    }
