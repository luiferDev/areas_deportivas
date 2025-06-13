using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace areas_deportivas.Models;

public partial class DeportesDbContext : DbContext
{
    public DeportesDbContext()
    {
    }

    public DeportesDbContext(DbContextOptions<DeportesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AreaDeportiva> AreaDeportivas { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AreaDeportiva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("area_deportiva_pkey");

            entity.ToTable("area_deportiva");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Disponibilidad).HasColumnName("disponibilidad");
            entity.Property(e => e.Nombre)
                .HasColumnType("character varying")
                .HasColumnName("nombre");
            entity.Property(e => e.TipoArea)
                .HasColumnName("tipo_area").HasConversion(
						v => v.ToString(),
						v => Enum.Parse<Tipo>(v)
				);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reserva_pkey");

            entity.ToTable("reserva");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.EstadoReserva)
                .HasColumnName("estado_reserva").HasConversion(
						v => v.ToString(),
						v => Enum.Parse<Estado>(v)
				);
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.HoraFin)
                .HasPrecision(0)
                .HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio)
                .HasPrecision(0)
                .HasColumnName("hora_inicio");
            entity.Property(e => e.IdAreaDeportiva).HasColumnName("id_area_deportiva");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdAreaDeportivaNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdAreaDeportiva)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reserva_id_area_deportiva_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reserva_id_usuario_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuario_pkey");

            entity.ToTable("usuario");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasColumnType("character varying")
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.ReservaId).HasColumnName("reserva_id");
            entity.Property(e => e.Role)
                .HasColumnName("role").HasConversion(
						v => v.ToString(),
						v => Enum.Parse<Role>(v)
				);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
