using Microsoft.EntityFrameworkCore;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;
using food_heaven_backend.PlanComidas.Domain.Models.Entities;
using food_heaven_backend.Security.Domain.Entities;

namespace food_heaven_backend.Shared.Infraestructure.Persistence.Configuration
{
    public class FoodHeavenContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<TipoProveedor> TiposProveedor { get; set; }
        public DbSet<TipoComida> TipoComidas { get; set; }
        public DbSet<Comida> Comidas { get; set; }
        public DbSet<PlanComida> PlanesComida { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.Username).HasColumnName("username").IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHashed).HasColumnName("password_hashed").IsRequired().HasMaxLength(255);
                entity.Property(u => u.Subscription).HasColumnName("suscription").IsRequired().HasMaxLength(50);
                entity.Property(u => u.City).HasColumnName("city").IsRequired().HasMaxLength(50);
                entity.Property(u => u.Phone).HasColumnName("phone").IsRequired();



            });
            builder.Entity<TipoProveedor>(entity =>
            {
                entity.ToTable("TipoProveedor");

                entity.HasKey(tp => tp.Id);

                entity.Property(tp => tp.Id).HasColumnName("id_tipo_proveedor");
                entity.Property(tp => tp.Descripcion).HasColumnName("Descripcion").IsRequired().HasMaxLength(100);
            });

            builder.Entity<Proveedor>(entity =>
            {
                entity.ToTable("Proveedor");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id).HasColumnName("id_proveedor");
                entity.Property(p => p.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(100);
                entity.Property(p => p.Distrito).HasColumnName("distrito").IsRequired().HasMaxLength(100);
                entity.Property(p => p.Contacto).HasColumnName("contacto").IsRequired().HasMaxLength(100);
                entity.Property(p => p.TipoProveedorId).HasColumnName("id_tipo_proveedor").IsRequired();

                entity.HasOne(p => p.TipoProveedor)
                      .WithMany()
                      .HasForeignKey(p => p.TipoProveedorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<Comida>(entity =>
            {
                entity.ToTable("Comida");

                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).HasColumnName("id_comida");

                entity.Property(c => c.Nombre)
                    .HasColumnName("nombre")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Complemento)
                    .HasColumnName("complemento")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(c => c.Url)
                    .HasColumnName("url")
                    .IsRequired();

                entity.Property(c => c.Cal)
                    .HasColumnName("cal")
                    .IsRequired();

                entity.Property(c => c.Prote)
                    .HasColumnName("prote")
                    .IsRequired();

                entity.Property(c => c.Carbo)
                    .HasColumnName("carbo")
                    .IsRequired();

                entity.Property(c => c.Grasa)
                    .HasColumnName("grasa")
                    .IsRequired();

                entity.Property(c => c.es_especial)
                    .HasColumnName("es_especial")
                    .IsRequired();

                entity.Property(c => c.id_tipo_comida)
                    .HasColumnName("id_tipo_comida")
                    .IsRequired();

                entity.Property(c => c.Id_Proveedor)
                    .HasColumnName("id_proveedor")
                    .IsRequired();

                entity.HasOne(c => c.TipoComida)
                    .WithMany()
                    .HasForeignKey(c => c.id_tipo_comida)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Proveedor)
                    .WithMany()
                    .HasForeignKey(c => c.Id_Proveedor)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<TipoComida>(entity =>
            {
                entity.ToTable("TipoComida");

                entity.HasKey(tc => tc.Id);

                entity.Property(tc => tc.Id)
                    .HasColumnName("id_tipo_comida");

                entity.Property(tc => tc.Descripcion)
                    .HasColumnName("descripcion")
                    .IsRequired();
            });
            builder.Entity<PlanComida>(entity =>
            {
                entity.ToTable("PlanComida");

                entity.HasKey(pc => pc.Id);

                entity.Property(pc => pc.Id).HasColumnName("id_plan");
                entity.Property(pc => pc.IdUsuario).HasColumnName("id_usuario").IsRequired();
                entity.Property(pc => pc.FechaInicio).HasColumnName("fecha_inicio").IsRequired();
                entity.Property(pc => pc.FechaFin).HasColumnName("fecha_fin").IsRequired();
                
                entity.Property(pc => pc.ListaComidas)
                    .HasColumnName("lista_comidas")
                    .HasConversion(
                        v => string.Join(",", v),  // Convertir el array de enteros en un string separado por comas
                        v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()  // Convertir el string de vuelta a un array de enteros
                    );
            });
        }
    }
}