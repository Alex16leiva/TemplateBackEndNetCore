using Dominio.Context.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Context.Mapping.Seguridad
{
    public class UsuarioMap : EntityMap<Usuario>
    {
        public override void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(r => r.UsuarioId);
            builder.ToTable("Usuario", "Seguridad");
            builder.Property(r => r.UsuarioId).HasColumnName("UsuarioId").IsRequired().IsUnicode(false).HasMaxLength(25);
            builder.Property(r => r.Nombre).HasColumnName("Nombre").IsRequired().HasMaxLength(50);
            builder.Property(r => r.Apellido).HasColumnName("Apellido").IsRequired().HasMaxLength(50);
            builder.Property(r => r.Contrasena).HasColumnName("Contrasena").IsRequired().HasMaxLength(25);
            builder.Property(r => r.RolId).HasColumnName("RolId").IsRequired().IsUnicode(false).HasMaxLength(25);

            base.Configure(builder);
        }
    }
}
