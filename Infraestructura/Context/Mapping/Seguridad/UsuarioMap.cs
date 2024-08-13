using Dominio.Context.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Context.Mapping.Seguridad
{
    internal class UsuarioMap : EntityMap<Usuario>
    {
        public override void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(r => r.UsuarioId);
            builder.ToTable("Usuario", "Seguridad");
            builder.Property(r => r.UsuarioId).HasColumnName("UsuarioId").IsRequired().IsUnicode(false).HasMaxLength(25);
            builder.Property(r => r.Nombre).HasColumnName("Nombre").IsRequired().HasMaxLength(50);
            builder.Property(r => r.Apellido).HasColumnName("Apellido").IsRequired().HasMaxLength(50);
            builder.Property(r => r.Contrasena).HasColumnName("Contrasena").IsRequired().HasMaxLength(250);
            builder.Property(r => r.RolId).HasColumnName("RolId").IsRequired().IsUnicode(false).HasMaxLength(25);
            builder.Property(r => r.Activo).HasColumnName("Activo").IsRequired();

            builder.HasOne(x => x.Rol).WithMany(r => r.Usuarios).HasForeignKey(x => x.RolId);

            base.Configure(builder);
        }
    }
}
