using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LibreriaPW.Models
{
    public partial class librosContext : DbContext
    {
        public librosContext()
        {
        }

        public librosContext(DbContextOptions<librosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Genero> Generos { get; set; } = null!;
        public virtual DbSet<Libro> Libros { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=libros", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Genero>(entity =>
            {
                entity.ToTable("genero");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            modelBuilder.Entity<Libro>(entity =>
            {
                entity.ToTable("libro");

                entity.HasIndex(e => e.Idgenero, "FKLibroGenero");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Autor).HasMaxLength(50);

                entity.Property(e => e.Descripcion).HasColumnType("text");

                entity.Property(e => e.Editorial).HasMaxLength(60);

                entity.Property(e => e.Idgenero).HasColumnName("IDGenero");

                entity.Property(e => e.Precio).HasPrecision(19, 4);

                entity.Property(e => e.Titulo).HasMaxLength(60);

                entity.HasOne(d => d.IdgeneroNavigation)
                    .WithMany(p => p.Libros)
                    .HasForeignKey(d => d.Idgenero)
                    .HasConstraintName("FKLibroGenero");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
