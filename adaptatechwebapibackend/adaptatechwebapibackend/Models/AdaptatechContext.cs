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

    public virtual DbSet<MensajeForo> MensajeForos { get; set; }

    public virtual DbSet<Operacione> Operaciones { get; set; }

    public virtual DbSet<PerfilUsuario> PerfilUsuarios { get; set; }

    public virtual DbSet<TemasForo> TemasForos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=adaptatech;Persist Security Info=False;User ID=SA;Password=r48950015n;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MensajeForo>(entity =>
        {
            entity.HasKey(e => e.IdMensaje).HasName("PK__MensajeF__E4D2A47FAF77820F");

            entity.ToTable("MensajeForo");

            entity.Property(e => e.FechaMensaje).HasColumnType("datetime");

            entity.HasOne(d => d.IdPerfilUsuariomensajeNavigation).WithMany(p => p.MensajeForos)
                .HasForeignKey(d => d.IdPerfilUsuariomensaje)
                .HasConstraintName("FK__MensajeFo__IdPer__4D94879B");

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
            entity.HasKey(e => e.IdPerfil).HasName("PK__PerfilUs__C7BD5CC15E3E78C2");

            entity.ToTable("PerfilUsuario");

            entity.HasIndex(e => e.Alias, "UQ__PerfilUs__70F4A9E08E2CA3C5").IsUnique();

            entity.Property(e => e.Alias)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Apellidos)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
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
