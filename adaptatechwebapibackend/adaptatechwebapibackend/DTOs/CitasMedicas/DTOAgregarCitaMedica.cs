using System;
namespace adaptatechwebapibackend.DTOs.CitasMedicas
{
	public class DTOAgregarCitaMedica
	{
        public string Medico { get; set; }
        public DateTime FechaHora { get; set; }
        public string CentroMedico { get; set; }
        public int IdPerfilUsuario { get; set; }
    }
}

