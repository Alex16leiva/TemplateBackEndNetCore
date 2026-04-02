namespace Aplicacion.DTOs.ConfiguracionesDTO
{
    public class ConfiguracionesDTO : ResponseBase
    {
        public required string ConfiguracionId { get; set; }
        public required string Descripcion { get; set; }
        public List<ConfiguracionesDetalleDTO> ConfiguracionesDetalle { get; set; }
    }
}
