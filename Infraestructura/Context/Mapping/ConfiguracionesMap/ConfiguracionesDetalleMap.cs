using Dominio.Context.Entidades.ConfiguracionesAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Context.Mapping.ConfiguracionesMap
{
    internal class ConfiguracionesDetalleMap : EntityMap<ConfiguracionesDetalle>
    {
        public override void Configure(EntityTypeBuilder<ConfiguracionesDetalle> builder)
        {
            builder.HasKey(r => new { r.ConfiguracionId, r.Atributo });
            builder.ToTable("ConfiguracionesDetalle","Comunes");
            builder.Property(r => r.ConfiguracionId).HasColumnName("ConfiguracionId").HasMaxLength(150).IsRequired();
            builder.Property(r => r.Atributo).HasColumnName("Atributo").HasMaxLength(150).IsRequired();
            builder.Property(r => r.Valor).HasColumnName("Valor").HasMaxLength(150).IsRequired();
            builder.Property(r => r.Descripcion).HasColumnName("Descripcion").HasMaxLength(100);
            builder.HasOne(r => r.Configuraciones)
           .WithMany(p => p.ConfiguracionesDetalle)
           .HasForeignKey(r => r.ConfiguracionId)
           .IsRequired();
            base.Configure(builder);
        }
    }
}
