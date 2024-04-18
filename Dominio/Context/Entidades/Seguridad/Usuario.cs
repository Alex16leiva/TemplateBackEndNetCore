using Dominio.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Context.Entidades.Seguridad
{
    public class Usuario : Entity
    {
        public string UsuarioId { get; set; }
        public string Contrasena { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string RolId { get; set; }

        [NotMapped]
        public string Token { get; set; }
        //public virtual Rol Rol { get; set; }
    }
}
