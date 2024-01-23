using System;
namespace adaptatechwebapibackend.DTOs
{
	public class DTOMensajeForo
	{
        public int IdMensaje { get; set; }
        public int? IdUsuariomensaje { get; set; }
        public int? IdPerfilUsuariomensaje { get; set; }
        public int? IdTema { get; set; }
        public string Texto { get; set; }
        public DateTime? FechaMensaje { get; set; }
    }
}

