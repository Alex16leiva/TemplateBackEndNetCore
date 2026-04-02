using Dominio.Core;

namespace Dominio.Context.Entidades.ConfiguracionesAgg
{
    public class ConfiguracionesDetalle : Entity
    {
        public required string ConfiguracionId { get; set; }
        public required string Atributo { get; set; }
        public required string Valor { get; set; }
        public required string Descripcion { get; set; }
        public virtual Configuraciones? Configuraciones { get; set; }
    }
}
