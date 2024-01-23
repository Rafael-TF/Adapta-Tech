namespace adaptatechwebapibackend.DTOs
    {
    public class DTOPerfilUsuarioPut
        {

        public int IdPerfil { get; set; }

        public int IdUsuario { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Telefono { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public byte[]? Avatar { get; set; }

        public string? Alias { get; set; }

        }
    }
