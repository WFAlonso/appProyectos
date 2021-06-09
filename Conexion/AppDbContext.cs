using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Base_de_datos.Entidades
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Perfile> Perfiles { get; set; }
        public virtual DbSet<Proyecto> Proyectos { get; set; }
        public virtual DbSet<Sesion> Sesions { get; set; }
        public virtual DbSet<Tarea> Tareas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("data source=WILMERALONSO;initial catalog=Database1;user id=sa;password=123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Perfile>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Perfil)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Proyecto>(entity =>
            {
                entity.Property(e => e.ProyectoId).ValueGeneratedNever();

                entity.Property(e => e.Descripcon)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sesion>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Sesion");

                entity.Property(e => e.FechaIni).HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Usuario)
                    .WithMany()
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Sesion_usuario");
            });

            modelBuilder.Entity<Tarea>(entity =>
            {
                entity.Property(e => e.TareaId).ValueGeneratedNever();

                entity.Property(e => e.Descripcon)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Proyecto)
                    .WithMany(p => p.Tareas)
                    .HasForeignKey(d => d.ProyectoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tareas_proyecto");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Habilitado)
                    .IsRequired()
                    .HasColumnName("habilitado")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Nombre ");

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("User ");
                
                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Correo ");

                entity.HasOne(d => d.Perfil)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.PerfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Usuarios_Perfil");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
