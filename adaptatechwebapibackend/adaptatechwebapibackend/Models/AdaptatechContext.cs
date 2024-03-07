using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace adaptatechwebapibackend.Models;

public partial class AdaptatechContext : DbContext
{
    public AdaptatechContext()
    {
    }

    public AdaptatechContext(DbContextOptions<AdaptatechContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CitaMedica> CitaMedicas { get; set; }

    public virtual DbSet<Medicamento> Medicamentos { get; set; }

    public virtual DbSet<MensajeForo> MensajeForos { get; set; }

    public virtual DbSet<Operacione> Operaciones { get; set; }

    public virtual DbSet<PerfilUsuario> PerfilUsuarios { get; set; }

    public virtual DbSet<TemasForo> TemasForos { get; set; }

    public virtual DbSet<Testimonio> Testimonios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=adaptatech;Persist Security Info=False;User ID=SA;Password=r48950015n;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CitaMedica>(entity =>
        {
            entity.HasKey(e => e.IdCita).HasName("PK__CitaMedi__394B0202BAF081E1");

            entity.ToTable("CitaMedica");

            entity.Property(e => e.CentroMedico).HasMaxLength(100);
            entity.Property(e => e.DiaSemana).HasMaxLength(15);
            entity.Property(e => e.Medico).HasMaxLength(50);

            entity.HasOne(d => d.IdPerfilUsuarioNavigation).WithMany(p => p.CitaMedicas)
                .HasForeignKey(d => d.IdPerfilUsuario)
                .HasConstraintName("FK__CitaMedic__IdPer__02FC7413");
        });

        modelBuilder.Entity<Medicamento>(entity =>
        {
            entity.HasKey(e => e.IdMedicamento).HasName("PK__Medicame__AC96376ED0A18FAC");

            entity.Property(e => e.DiaSemana).HasMaxLength(20);
            entity.Property(e => e.Funcion).HasMaxLength(255);
            entity.Property(e => e.Medicacion).HasMaxLength(60);
            entity.Property(e => e.Posologia).HasMaxLength(255);

            entity.HasOne(d => d.IdPerfilUsuarioNavigation).WithMany(p => p.Medicamentos)
                .HasForeignKey(d => d.IdPerfilUsuario)
                .HasConstraintName("FK__Medicamen__IdPer__74AE54BC");
        });

        modelBuilder.Entity<MensajeForo>(entity =>
        {
            entity.HasKey(e => e.IdMensaje).HasName("PK__MensajeF__E4D2A47FAF77820F");

            entity.ToTable("MensajeForo");

            entity.Property(e => e.FechaMensaje).HasColumnType("datetime");

            entity.HasOne(d => d.IdPerfilUsuariomensajeNavigation).WithMany(p => p.MensajeForos)
                .HasForeignKey(d => d.IdPerfilUsuariomensaje)
                .HasConstraintName("FK__MensajeFo__IdPer__71D1E811");

            entity.HasOne(d => d.IdTemaNavigation).WithMany(p => p.MensajeForos)
                .HasForeignKey(d => d.IdTema)
                .HasConstraintName("FK__MensajeFo__IdTem__4E88ABD4");

            entity.HasOne(d => d.IdUsuariomensajeNavigation).WithMany(p => p.MensajeForos)
                .HasForeignKey(d => d.IdUsuariomensaje)
                .HasConstraintName("FK__MensajeFo__IdUsu__4CA06362");
        });

        modelBuilder.Entity<Operacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Operacio__3214EC073780C877");

            entity.Property(e => e.Controller).HasMaxLength(50);
            entity.Property(e => e.FechaAccion).HasColumnType("datetime");
            entity.Property(e => e.Ip).HasMaxLength(50);
            entity.Property(e => e.Operacion).HasMaxLength(50);
        });

        modelBuilder.Entity<PerfilUsuario>(entity =>
        {
            entity.HasKey(e => e.IdPerfil).HasName("PK__tmp_ms_x__C7BD5CC18061CE61");

            entity.ToTable("PerfilUsuario");

            entity.HasIndex(e => e.Alias, "UQ__tmp_ms_x__70F4A9E0C0D1B11A").IsUnique();

            entity.Property(e => e.Alias)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Apellidos)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Avatar).IsUnicode(false);
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Usuario).WithMany(p => p.PerfilUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__PerfilUsu__Usuar__70DDC3D8");
        });

        modelBuilder.Entity<TemasForo>(entity =>
        {
            entity.HasKey(e => e.IdTema).HasName("PK__TemasFor__9F3A411726AA3AA5");

            entity.ToTable("TemasForo");

            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.Titulo).HasMaxLength(60);

            entity.HasOne(d => d.IdTemaUsuarioNavigation).WithMany(p => p.TemasForos)
                .HasForeignKey(d => d.IdTemaUsuario)
                .HasConstraintName("FK__TemasForo__IdTem__49C3F6B7");
        });

        modelBuilder.Entity<Testimonio>(entity =>
        {
            entity.HasKey(e => e.IdTestimonio).HasName("PK__Testimon__3EFC389F9E21B7D8");

            entity.Property(e => e.Titulo).HasMaxLength(60);

            entity.HasOne(d => d.IdPerfilUsuarioNavigation).WithMany(p => p.Testimonios)
                .HasForeignKey(d => d.IdPerfilUsuario)
                .HasConstraintName("FK__Testimoni__IdPer__160F4887");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF97C3198A5F");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.EnlaceCambioPass).HasMaxLength(50);
            entity.Property(e => e.FechaEnvioEnlace).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Rol).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
