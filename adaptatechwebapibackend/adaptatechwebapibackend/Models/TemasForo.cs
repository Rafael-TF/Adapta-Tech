using System;
using System.Collections.Generic;

namespace adaptatechwebapibackend.Models;

public partial class TemasForo
{
    public int IdTema { get; set; }

    public int? IdTemaUsuario { get; set; }

    public string? Titulo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Usuario? IdTemaUsuarioNavigation { get; set; }

    public virtual ICollection<MensajeForo> MensajeForos { get; set; } = new List<MensajeForo>();
}
