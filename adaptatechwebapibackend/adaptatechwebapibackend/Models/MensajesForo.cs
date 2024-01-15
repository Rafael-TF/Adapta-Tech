using System;
using System.Collections.Generic;

namespace adaptatechwebapibackend.Models;

public partial class MensajesForo
{
    public int IdMensaje { get; set; }

    public int? UsuarioId { get; set; }

    public string? Texto { get; set; }

    public DateTime? FechaMensaje { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
