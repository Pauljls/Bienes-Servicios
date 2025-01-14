using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BienesYServicios.Models;

public partial class RequerimientosDbContext : DbContext
{
    public RequerimientosDbContext()
    {
    }

    public RequerimientosDbContext(DbContextOptions<RequerimientosDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriasRequerimiento> CategoriasRequerimientos { get; set; }

    public virtual DbSet<EstadosRequerimiento> EstadosRequerimientos { get; set; }

    public virtual DbSet<HistorialRequerimiento> HistorialRequerimientos { get; set; }

    public virtual DbSet<Oficina> Oficinas { get; set; }

    public virtual DbSet<Presupuesto> Presupuestos { get; set; }

    public virtual DbSet<Requerimiento> Requerimientos { get; set; }

    public virtual DbSet<RolUsuario> RolUsuarios { get; set; }

    public virtual DbSet<SubcategoriaRequerimiento> SubcategoriaRequerimientos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriasRequerimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__categori__3214EC070AE20F7E");

            entity.ToTable("categoriasRequerimiento");

            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.IdSubcategoriaNavigation).WithMany(p => p.CategoriasRequerimientos)
                .HasForeignKey(d => d.IdSubcategoria)
                .HasConstraintName("FK_Categoria_Subcategoria");
        });

        modelBuilder.Entity<EstadosRequerimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EstadosR__3214EC07109D5209");

            entity.ToTable("EstadosRequerimiento");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<HistorialRequerimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Historia__3214EC0753112E31");

            entity.ToTable("HistorialRequerimiento");

            entity.HasIndex(e => e.IdEstado, "IX_HistorialRequerimiento_Estado");

            entity.HasIndex(e => e.IdRequerimiento, "IX_HistorialRequerimiento_Requerimiento");

            entity.HasIndex(e => e.IdUsuario, "IX_HistorialRequerimiento_Usuario");

            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.HistorialRequerimientos)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Historial_Estado");

            entity.HasOne(d => d.IdRequerimientoNavigation).WithMany(p => p.HistorialRequerimientos)
                .HasForeignKey(d => d.IdRequerimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Historial_Requerimiento");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HistorialRequerimientos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Historial_Usuario");

            entity.HasOne(d => d.Presupuesto).WithMany(p => p.HistorialRequerimientos)
                .HasForeignKey(d => d.PresupuestoId)
                .HasConstraintName("FK_Historial_Presupuesto");
        });

        modelBuilder.Entity<Oficina>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Oficina__3214EC07C4DAA3C9");

            entity.ToTable("Oficina");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Presupuesto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__presupue__3214EC07785CFF35");

            entity.ToTable("presupuesto");

            entity.Property(e => e.Divisa)
                .HasMaxLength(10)
                .HasColumnName("divisa");
            entity.Property(e => e.ValorReferencial)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("valorReferencial");
        });

        modelBuilder.Entity<Requerimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Requerim__3214EC07952DA7C0");

            entity.ToTable("Requerimiento");

            entity.HasIndex(e => e.CategoriaRequerimientoId, "IX_Requerimiento_Categoria");

            entity.Property(e => e.Nombre).HasMaxLength(200);

            entity.HasOne(d => d.CategoriaRequerimiento).WithMany(p => p.Requerimientos)
                .HasForeignKey(d => d.CategoriaRequerimientoId)
                .HasConstraintName("FK_Requerimiento_Categoria");
        });

        modelBuilder.Entity<RolUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RolUsuar__3214EC070770793A");

            entity.ToTable("RolUsuario");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<SubcategoriaRequerimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__subcateg__3214EC079C134D10");

            entity.ToTable("subcategoriaRequerimientos");

            entity.Property(e => e.Codigo).HasMaxLength(20);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC07CD27E215");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.RolUsuarioId, "IX_Usuario_RolUsuario");

            entity.Property(e => e.Apellidos).HasMaxLength(100);
            entity.Property(e => e.Celular).HasMaxLength(20);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(100)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);

            entity.HasOne(d => d.Oficina).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.OficinaId)
                .HasConstraintName("FK__Usuario__Oficina__3C69FB99");

            entity.HasOne(d => d.RolUsuario).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolUsuarioId)
                .HasConstraintName("FK__Usuario__RolUsua__3B75D760");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
