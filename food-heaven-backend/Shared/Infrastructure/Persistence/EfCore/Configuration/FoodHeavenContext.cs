using Microsoft.EntityFrameworkCore;
using food_heaven_backend.FoodCatalogContext.Domain.Model.Entities;
using food_heaven_backend.PlanComidas.Domain.Model.Entities;
using food_heaven_backend.Security.Domain.Model.Entities;

namespace food_heaven_backend.Shared.Infrastructure.Persistence.EfCore.Configuration
{
    public class FoodHeavenContext(DbContextOptions options) : DbContext(options)
    {
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
                entity.Property(u => u.FullName).HasColumnName("full_name").IsRequired().HasMaxLength(120);
                entity.Property(u => u.Username).HasColumnName("username").IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHashed).HasColumnName("password_hashed").IsRequired().HasMaxLength(255);
                entity.Property(u => u.Subscription).HasColumnName("suscription").IsRequired().HasMaxLength(50);
                entity.Property(u => u.City).HasColumnName("city").IsRequired().HasMaxLength(50);
                entity.Property(u => u.Phone).HasColumnName("phone").IsRequired();
                entity.Property(u => u.Address).HasColumnName("address").IsRequired().HasMaxLength(180);
                entity.Property(u => u.PaymentMethod).HasColumnName("payment_method").IsRequired().HasMaxLength(80);

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

                entity.Property(c => c.NombreEn)
                    .HasColumnName("nombre_en")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Complemento)
                    .HasColumnName("complemento")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(c => c.ComplementoEn)
                    .HasColumnName("complemento_en")
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

                entity.Property(c => c.id_tipo_comida)
                    .HasColumnName("id_tipo_comida")
                    .IsRequired();

                entity.Property(c => c.CatalogSourceId)
                    .HasColumnName("id_proveedor")
                    .IsRequired();

                entity.HasOne(c => c.TipoComida)
                    .WithMany()
                    .HasForeignKey(c => c.id_tipo_comida)
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
