using Dominio.Core;

namespace Dominio.Context.Entidades.Seguridad
{
    public class Permisos : Entity
    {
        public required string RolId { get; set; }
        public required string PantallaId { get; set; }
        public required bool Ver { get; set; }
        public required bool Editar { get; set; }
        public required bool Eliminar { get; set; }

        public virtual Rol? Rol { get; set; }
    }
}
