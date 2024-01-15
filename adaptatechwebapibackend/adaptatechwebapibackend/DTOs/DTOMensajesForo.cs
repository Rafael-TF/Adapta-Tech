using System;
namespace adaptatechwebapibackend.DTOs
{
	public class DTOMensajesForo
	{
        public int IdMensaje { get; set; }
        public int? UsuarioId { get; set; }
        public string? Texto { get; set; }
        public DateTime? FechaMensaje { get; set; }
        public string? UsuarioNombre { get; set; }
    }
}

