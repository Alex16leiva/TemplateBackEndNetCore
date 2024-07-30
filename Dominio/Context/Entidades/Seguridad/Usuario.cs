using Dominio.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Context.Entidades.Seguridad
{
    public class Usuario : Entity
    {
        public required string UsuarioId { get; set; }
        public required string Contrasena { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public string? RolId { get; set; }

        [NotMapped]
        public string? Token { get; set; }
        //public virtual Rol Rol { get; set; }
    }
}
