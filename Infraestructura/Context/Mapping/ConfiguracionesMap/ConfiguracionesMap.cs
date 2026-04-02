using Dominio.Context.Entidades.ConfiguracionesAgg;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Context.Mapping.ConfiguracionesMap
{
    internal class ConfiguracionesMap : EntityMap<Configuraciones>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Configuraciones> builder)
        {
            builder.HasKey(r => r.ConfiguracionId);
            builder.ToTable("Configuraciones", "comunes");
            builder.Property(r => r.ConfiguracionId).HasColumnName("ConfiguracionId").HasMaxLength(150).IsRequired();
            builder.Property(r => r.Descripcion).HasColumnName("Descripcion").HasMaxLength(100).IsRequired().IsUnicode(false);
            base.Configure(builder);
        }
    }
}
