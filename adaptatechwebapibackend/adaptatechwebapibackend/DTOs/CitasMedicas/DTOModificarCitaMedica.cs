using System;
namespace adaptatechwebapibackend.DTOs.CitasMedicas
{
	public class DTOModificarCitaMedica
	{
        public int? IdCita { get; set; }
        public string Medico { get; set; }
        public DateTime FechaHora { get; set; }
        public string CentroMedico { get; set; }
    }
}

