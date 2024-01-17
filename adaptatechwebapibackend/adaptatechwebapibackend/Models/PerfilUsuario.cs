using System;
using System.Collections.Generic;

namespace adaptatechwebapibackend.Models;

public partial class PerfilUsuario
{
    public int IdPerfil { get; set; }

    public int? UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Telefono { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public byte[]? Avatar { get; set; }

    public string? Alias { get; set; }

    public virtual ICollection<MensajeForo> MensajeForos { get; set; } = new List<MensajeForo>();

    public virtual Usuario? Usuario { get; set; }
}
