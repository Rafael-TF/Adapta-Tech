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

    public string? Avatar { get; set; }

    public string? Alias { get; set; }

    public virtual ICollection<CitaMedica> CitaMedicas { get; set; } = new List<CitaMedica>();

    public virtual ICollection<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();

    public virtual ICollection<MensajeForo> MensajeForos { get; set; } = new List<MensajeForo>();

    public virtual ICollection<Testimonio> Testimonios { get; set; } = new List<Testimonio>();

    public virtual Usuario? Usuario { get; set; }
}
