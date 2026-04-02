using Dominio.Core;

namespace Dominio.Context.Entidades.ConfiguracionesAgg
{
    public class Configuraciones : Entity
    {
        public required string ConfiguracionId { get; set; }
        public required string Descripcion { get; set; }
        public virtual ICollection<ConfiguracionesDetalle>? ConfiguracionesDetalle { get; set; }
    }
}
