using System;
using System.Collections.Generic;

namespace adaptatechwebapibackend.Models;

public partial class CitaMedica
{
    public int IdCita { get; set; }

    public string Medico { get; set; } = null!;

    public DateTime FechaHora { get; set; }

    public string? CentroMedico { get; set; }

    public int? IdPerfilUsuario { get; set; }

    public string? DiaSemana { get; set; }

    public virtual PerfilUsuario? IdPerfilUsuarioNavigation { get; set; }
}
