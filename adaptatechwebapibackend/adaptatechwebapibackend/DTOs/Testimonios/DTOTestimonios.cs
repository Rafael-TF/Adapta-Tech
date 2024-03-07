using System;
namespace adaptatechwebapibackend.DTOs.Testimonios
{
	public class DTOTestimonios
	{
        public int IdTestimonio { get; set; }
        public int IdPerfilUsuario { get; set; }
        public string Avatar { get; set; }
        public string NombrePerfilUsuario { get; set; }
        public string TituloTestimonio { get; set; }
        public string TextoTestimonio { get; set; }
    }
}

