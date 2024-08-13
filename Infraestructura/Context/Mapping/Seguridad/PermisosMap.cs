using Dominio.Context.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Context.Mapping.Seguridad
{
    internal class PermisosMap : EntityMap<Permisos>
    {
        public override void Configure(EntityTypeBuilder<Permisos> builder)
        {
            builder.ToTable("Permisos", "Seguridad");
            builder.HasKey(p => new { p.RolId, p.PantallaId });
            builder.Property(r => r.PantallaId).HasColumnName("PantallaId").IsRequired();
            builder.Property(r => r.RolId).HasColumnName("RolId").IsRequired();
            builder.Property(r => r.Ver).HasColumnName("Ver").IsRequired();
            builder.Property(r => r.Editar).HasColumnName("Editar").IsRequired();
            builder.Property(r => r.Eliminar).HasColumnName("Eliminar").IsRequired();

            builder.HasOne(r => r.Rol).WithMany(r => r.Permisos).HasForeignKey(r => r.RolId);


            base.Configure(builder);
        }
    }
}
