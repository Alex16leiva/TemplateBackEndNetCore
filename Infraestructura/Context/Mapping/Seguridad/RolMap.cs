using Dominio.Context.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Context.Mapping.Seguridad
{
    public class RolMap : EntityMap<Rol>
    {
        public override void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.HasKey(r => r.RolId);
            builder.ToTable("Rol", "Seguridad");
            builder.Property(r => r.RolId).HasColumnName("RolId").IsRequired().IsUnicode(false).HasMaxLength(25);
            builder.Property(r => r.Descripcion).HasColumnName("Descripcion").IsRequired().IsUnicode(false).HasMaxLength(25);


            base.Configure(builder);
        }
    }
}
