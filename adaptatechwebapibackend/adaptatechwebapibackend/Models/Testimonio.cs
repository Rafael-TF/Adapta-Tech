using System;
using System.Collections.Generic;

namespace adaptatechwebapibackend.Models;

public partial class Testimonio
{
    public int IdTestimonio { get; set; }

    public int? IdPerfilUsuario { get; set; }

    public string? TextoTestimonio { get; set; }

    public string? Titulo { get; set; }

    public virtual PerfilUsuario? IdPerfilUsuarioNavigation { get; set; }
}
