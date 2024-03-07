using System;
namespace adaptatechwebapibackend.DTOs.CitasMedicas
{
	public class DTOCitaMedica
	{
        public int? IdCita { get; set; }
        public string Medico { get; set; }
        public DateTime FechaHora { get; set; }
        public string CentroMedico { get; set; }
        public string DiaSemana { get; set; }
        public int IdPerfilUsuario { get; set; }
    }
}

