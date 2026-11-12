using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Restaurante.Entidades.Models;

namespace Restaurante.Datos.Models;

public partial class RestauranteContext : DbContext
{
    public RestauranteContext(DbContextOptions<RestauranteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasIndex(e => e.Numero, "UQ_Mesas_Numero").IsUnique();

            entity.Property(e => e.Ubicacion).HasMaxLength(80);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasIndex(e => e.FechaHora, "IX_Reservas_FechaHora");

            entity.HasIndex(e => e.MesaId, "IX_Reservas_MesaId");

            entity.Property(e => e.FechaHora).HasPrecision(0);
            entity.Property(e => e.NombreCliente).HasMaxLength(100);
            entity.Property(e => e.Observaciones).HasMaxLength(300);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasOne(d => d.Mesa).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.MesaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservas_Mesas");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
