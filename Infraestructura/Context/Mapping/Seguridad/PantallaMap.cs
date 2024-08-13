using Dominio.Context.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Context.Mapping.Seguridad
{
    internal class PantallaMap : EntityMap<Pantalla>
    {
        public override void Configure(EntityTypeBuilder<Pantalla> builder)
        {
            builder.HasKey(r => r.PantallaId);
            builder.ToTable("Pantalla", "Seguridad");
            builder.Property(r => r.PantallaId).HasColumnName("PantallaId").IsRequired();
            builder.Property(r => r.Descripcion).HasColumnName("Descripcion").IsRequired().IsUnicode(false);

            base.Configure(builder);
        }
    }
}
