using System;
using System.Collections.Generic;

namespace adaptatechwebapibackend.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Rol { get; set; }

    public byte[] Salt { get; set; } = null!;

    public string? EnlaceCambioPass { get; set; }

    public DateTime? FechaEnvioEnlace { get; set; }

    public virtual ICollection<MensajesForo> MensajesForos { get; set; } = new List<MensajesForo>();

    public virtual ICollection<PerfilUsuario> PerfilUsuarios { get; set; } = new List<PerfilUsuario>();
}
