using System;
namespace adaptatechwebapibackend.DTOs.Medicamentos
{
	public class DTOModificarMedicamento
	{
        public int IdMedicamento { get; set; }
        public string? Medicacion { get; set; }
        public string? Posologia { get; set; }
        public string? Funcion { get; set; }
        public string? DiaSemana { get; set; }
    }
}

