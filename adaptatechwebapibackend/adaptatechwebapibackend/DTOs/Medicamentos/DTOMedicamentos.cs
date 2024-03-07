using System;
namespace adaptatechwebapibackend.DTOs.Medicamentos
{
	public class DTOMedicamentos
	{
        public int? IdMedicamento { get; set; }
        public string Medicacion { get; set; }
        public string Posologia { get; set; }
        public string Funcion { get; set; }
        public string DiaSemana { get; set; }
		public int? IdPerfilUsuario { get; set; }
	}
}

