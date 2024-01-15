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

    public virtual DbSet<MensajesForo> MensajesForos { get; set; }

    public virtual DbSet<PerfilUsuario> PerfilUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=adaptatech;Persist Security Info=False;User ID=SA;Password=r48950015n;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MensajesForo>(entity =>
        {
            entity.HasKey(e => e.IdMensaje).HasName("PK__Mensajes__E4D2A47F67F6C5D5");

            entity.ToTable("MensajesForo");

            entity.Property(e => e.FechaMensaje).HasColumnType("datetime");

            entity.HasOne(d => d.Usuario).WithMany(p => p.MensajesForos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__MensajesF__Usuar__3D5E1FD2");
        });

        modelBuilder.Entity<PerfilUsuario>(entity =>
        {
            entity.HasKey(e => e.IdPerfil).HasName("PK__PerfilUs__C7BD5CC15E3E78C2");

            entity.ToTable("PerfilUsuario");

            entity.HasIndex(e => e.Alias, "UQ__PerfilUs__70F4A9E08E2CA3C5").IsUnique();

            entity.Property(e => e.Alias)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Apellidos)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Usuario).WithMany(p => p.PerfilUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__PerfilUsu__Usuar__3A81B327");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF97C3198A5F");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Rol).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
