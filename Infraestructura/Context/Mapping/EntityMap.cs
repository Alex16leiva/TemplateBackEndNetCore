using Dominio.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Context.Mapping
{
    public class EntityMap<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(t => t.FechaTransaccion).HasColumnName("FechaTransaccion");
            builder.Property(t => t.DescripcionTransaccion).HasColumnName("DescripcionTransaccion").IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(t => t.ModificadoPor).HasColumnName("ModificadoPor").IsRequired().IsUnicode(false).HasMaxLength(25);
            builder.Property(t => t.RowVersion).HasColumnName("RowVersion").ValueGeneratedOnAddOrUpdate();
            builder.Property(t => t.TipoTransaccion).HasColumnName("TipoTransaccion").IsRequired().IsUnicode(false).HasMaxLength(50);
            builder.Property(t => t.TransaccionUId).HasColumnName("TransaccionUId");
        }
    }
}
