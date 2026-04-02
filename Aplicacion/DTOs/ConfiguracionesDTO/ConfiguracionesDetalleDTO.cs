namespace Aplicacion.DTOs.ConfiguracionesDTO
{
    public class ConfiguracionesDetalleDTO : ResponseBase
    {
        public string? ConfiguracionId { get; set; }
        public string? Atributo { get; set; }
        public string? Valor { get; set; }
        public string? Descripcion { get; set; }
    }
}
