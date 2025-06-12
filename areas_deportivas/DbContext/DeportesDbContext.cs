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

	public virtual DbSet<Administrador> Administradors { get; set; }

	public virtual DbSet<AreaDeportiva> AreaDeportivas { get; set; }

	public virtual DbSet<Instructor> Instructors { get; set; }

	public virtual DbSet<Persona> Personas { get; set; }

	public virtual DbSet<Reserva> Reservas { get; set; }

	public virtual DbSet<Usuario> Usuarios { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
		=> optionsBuilder.UseNpgsql("Host=localhost;Database=areas_deportivas;Username=postgres;Password=123456");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Administrador>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("Administrador_pkey");

			entity.ToTable("Administrador");

			entity.Property(e => e.Id)
				.ValueGeneratedNever()
				.HasColumnName("id");
		});

		modelBuilder.Entity<AreaDeportiva>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("area_deportiva_pkey");

			entity.ToTable("area_deportiva");

			entity.Property(e => e.Id)
				.ValueGeneratedNever()
				.HasColumnName("id");
			entity.Property(e => e.Disponibilidad).HasColumnName("disponibilidad");
			entity.Property(e => e.Nombre)
				.HasColumnType("character varying")
				.HasColumnName("nombre");
			entity.Property(e => e.TipoArea)
				.HasColumnType("character varying")
				.HasColumnName("tipo_area").HasConversion(
					type => type.ToString(),
					type => Enum.Parse<Tipo>(type)
				);
		});

		modelBuilder.Entity<Instructor>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("Instructor_pkey");

			entity.ToTable("Instructor");

			entity.Property(e => e.Id)
				.ValueGeneratedNever()
				.HasColumnName("id");
			entity.Property(e => e.Especialidad)
				.HasColumnType("character varying")
				.HasColumnName("especialidad");
		});

		modelBuilder.Entity<Persona>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("FK_PERSONA");

			entity.ToTable("Persona");

			entity.HasIndex(e => e.TipoPersonaId, "Persona_tipo_persona_id_key").IsUnique();

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
			entity.Property(e => e.Role)
				.HasColumnType("character varying")
				.HasColumnName("role").HasConversion(
					rol => rol.ToString(),
					rol => Enum.Parse<Role>(rol)
				);
			entity.Property(e => e.TipoPersonaId).HasColumnName("tipo_persona_id");
			entity.Property(e => e.Username)
				.HasColumnType("character varying")
				.HasColumnName("username");

			entity.HasOne(d => d.TipoPersona).WithOne(p => p.Persona)
				.HasForeignKey<Persona>(d => d.TipoPersonaId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Persona_tipo_persona_id_fkey1");

			entity.HasOne(d => d.TipoPersonaNavigation).WithOne(p => p.Persona)
				.HasForeignKey<Persona>(d => d.TipoPersonaId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Persona_tipo_persona_id_fkey2");

			entity.HasOne(d => d.TipoPersona1).WithOne(p => p.Persona)
				.HasForeignKey<Persona>(d => d.TipoPersonaId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Persona_tipo_persona_id_fkey");
		});

		modelBuilder.Entity<Reserva>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("reserva_pkey");

			entity.ToTable("reserva");

			entity.Property(e => e.Id)
				.ValueGeneratedNever()
				.HasColumnName("id");
			entity.Property(e => e.EstadoReserva)
				.HasColumnType("character varying")
				.HasColumnName("estado_reserva").HasConversion(
						state => state.ToString(),
						state => Enum.Parse<Estado>(state)
				);
			entity.Property(e => e.Fecha).HasColumnName("fecha");
			entity.Property(e => e.HoraFin)
				.HasColumnType("time with time zone")
				.HasColumnName("hora_fin");
			entity.Property(e => e.HoraInicio)
				.HasColumnType("time with time zone")
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
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
