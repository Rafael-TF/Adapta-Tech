using System;
using System.Collections.Generic;

namespace adaptatechwebapibackend.Models;

public partial class MensajeForo
{
    public int IdMensaje { get; set; }

    public int? IdUsuariomensaje { get; set; }

    public int? IdPerfilUsuariomensaje { get; set; }

    public int? IdTema { get; set; }

    public string? Texto { get; set; }

    public DateTime? FechaMensaje { get; set; }

    public virtual PerfilUsuario IdPerfilUsuariomensajeNavigation { get; set; }

    public virtual TemasForo? IdTemaNavigation { get; set; }

    public virtual Usuario? IdUsuariomensajeNavigation { get; set; }
}
