using System;
using System.Collections.Generic;

namespace adaptatechwebapibackend.Models;

public partial class Medicamento
{
    public int IdMedicamento { get; set; }

    public string Medicacion { get; set; } = null!;

    public string Posologia { get; set; } = null!;

    public string? Funcion { get; set; }

    public string? DiaSemana { get; set; }

    public int? IdPerfilUsuario { get; set; }

    public virtual PerfilUsuario? IdPerfilUsuarioNavigation { get; set; }
}
