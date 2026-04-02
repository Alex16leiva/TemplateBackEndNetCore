namespace Aplicacion.DTOs.ConfiguracionesDTO
{
    public class ConfiguracionesRequest : RequestBase
    {
        public ConfiguracionesDTO? Configuraciones { get; set; }
        public ConfiguracionesDetalleDTO? ConfiguracionesDetalle { get; set; }
    }
}
