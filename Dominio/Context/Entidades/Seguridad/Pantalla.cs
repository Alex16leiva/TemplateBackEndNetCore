using Dominio.Core;

namespace Dominio.Context.Entidades.Seguridad
{
    public class Pantalla : Entity
    {
        public required string PantallaId { get; set; }
        public required string Descripcion { get; set; }
    }
}
